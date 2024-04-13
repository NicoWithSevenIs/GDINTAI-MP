using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Reflection;



/*
   
    References:

        Based on my code off that spelunky activity:

            https://www.youtube.com/watch?v=-L-WgKMFuhE
            https://en.wikipedia.org/wiki/A*_search_algorithm
    
        As well as my BFS Pathfinding Implementation in my group's GDADPRG MP. (which is pretty similar to the one below)

        For unity based pathfinding, I used this as reference:

            https://www.youtube.com/watch?v=alU04hvz6L4
    
 */

public class Pathfinding 
{

    public Node[,] nodes;
    public LayerMask layerMask;

    public Pathfinding() { }
    public Pathfinding (LayerMask layerMask) { this.layerMask = layerMask;}



    public List<Node> getPathAStar(Vector2Int originCell, Vector2Int targetCell)
    {

        ConsolidateGrid();

        Vector2Int originIndex = TilemapManager.instance.CellToIndex(originCell);
        Vector2Int targetIndex = TilemapManager.instance.CellToIndex(targetCell);

    
        Node originNode = nodes[originIndex.x, originIndex.y];
        Node targetNode = nodes[targetIndex.x, targetIndex.y];


        originNode.gCost = 0;
        originNode.hCost = getDistance(originNode, targetNode);
        originNode.CalculateFCost();
       
        List<Node> openList = new List<Node>() {  originNode };
        List<Node> closedList = new List<Node>();

        
        while(openList.Count > 0)
        {
            Node currentNode = getMinFCost(openList);

            if (currentNode == targetNode)
                break;

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            List<Node> neighbors = getValidNeighbors(currentNode);

            foreach (var n in neighbors)
            {
                if (closedList.Contains(n))
                    continue;

                float tentativeGCost = currentNode.gCost + getDistance(currentNode, n);

                if(tentativeGCost < n.gCost)
                {
                    n.assignPrevious(currentNode);
                    n.gCost = tentativeGCost;
                    n.hCost = getDistance(n, targetNode) + getPotionCost(n);
                    n.CalculateFCost();

                    if(!openList.Contains(n))
                        openList.Add(n);
                }
            }

        }
       
        List<Node> path = new List<Node>();

        for (Node i = targetNode; i.previous != null; i = i.previous)
        {
            path.Add(i);
        }

        path.Reverse();

        if (path.Count == 0) Debug.Log("No Path");

        return path;
    }

    private void ConsolidateGrid()
    {
        BoundsInt b = TilemapManager.instance.maxBoundsData.Value;
        nodes = new Node[b.size.x, b.size.y];
        nodes.Initialize();


        for (int row = 0; row < b.size.y; row++)
        {
            for (int col = 0; col < b.size.x; col++)
            {

                Vector2Int cell = TilemapManager.instance.IndexToCell(col, row);

                nodes[col, row] = new Node(cell);
                nodes[col, row].gCost = int.MaxValue;
                nodes[col, row].CalculateFCost();
                nodes[col, row].assignPrevious(null);

            }

        }

    }


    private Node getMinFCost(List<Node> nodeList)
    {
        Node min = nodeList[0];

        for(int i = 1; i < nodeList.Count; i++)
        {
            if (nodeList[i].fCost < min.fCost)
                min = nodeList[i];
        }

        return min;
    }
    
    
    //This part's all by myself
    private List<Node> getValidNeighbors(Node node)
    {
        BoundsInt b = TilemapManager.instance.maxBoundsData.Value;
        
        var neighbors = new List<Node>();

        var dir = new Dictionary<string, Vector2Int>
        {
            {"North",  new Vector2Int(0, 1)},
            {"South",  new Vector2Int(0, -1)},
            {"West",  new Vector2Int(-1, 0)},
            {"East",  new Vector2Int(1, 0)},
        };

        var index = TilemapManager.instance.CellToIndex(node.toVector2i());


        Func< Vector2Int, Vector2Int, bool> withinBounds = (Vector2Int index, Vector2Int t) =>
        {
            return index.x + t.x >= 0 && index.x + t.x < b.size.x && index.y + t.y >= 0 && index.y + t.y < b.size.y;
        };

        Func<Vector2Int, bool> isValidCell = (Vector2Int index) =>
        {
           
            Vector2 nodePos = TilemapManager.instance.CellToWorld(node.toVector2i());
            Vector2Int neighborCell = TilemapManager.instance.IndexToCell(index.x, index.y);
            Vector2 neighborPos = TilemapManager.instance.CellToWorld(neighborCell);

            RaycastHit2D ray = Physics2D.Raycast(nodePos, neighborPos - nodePos, 1, layerMask);

           // Debug.DrawRay(nodePos, neighborPos - nodePos, Color.black);

            return TilemapManager.instance.tileMapTypes[index.x, index.y] == TileType.Path && !ray;
        };

        
        /*
         
         Checks if the corner is traversable by checking if both direction tiles corresponding to it is a valid tile
         This is done to avoid cases where the agent gets stuck because the diagonal tile is valid but its direction
         tiles are not


                                        ________ <- This is the debug draw line
                Gets stuck here ->     /________
                                      /|
                                     | | Tilemap Collider
                     
            
          I'm pretty proud of this one (the illustration hahahhaha)

         */

        Action<string, string> tryAddCorner = (string vertical, string horizontal) =>
        {
            bool containsHorizontal = false;
            bool containsVertical = false;

            foreach (var n in neighbors)
            {
                Vector2Int vertTrav = index + dir[vertical];

                if (n.toVector2i() == TilemapManager.instance.IndexToCell(vertTrav.x, vertTrav.y))
                    containsVertical = true;

                Vector2Int horiTrav = index + dir[horizontal];
                if (n.toVector2i() == TilemapManager.instance.IndexToCell(horiTrav.x, horiTrav.y))
                    containsHorizontal = true;
            }

            if (containsVertical && containsHorizontal)
            {
                var tIndex = new Vector2Int(index.x + dir[horizontal].x, index.y + dir[vertical].y);
                neighbors.Add(nodes[tIndex.x, tIndex.y]);
         
            }


        };

        foreach (var i in dir)
        {
            if(withinBounds(index, i.Value))
            {
                var tIndex = new Vector2Int(index.x + i.Value.x, index.y + i.Value.y);
                if (isValidCell(tIndex))
                    neighbors.Add(nodes[tIndex.x, tIndex.y]);
            }
        }


        tryAddCorner("North", "West");
        tryAddCorner("North", "East");
        tryAddCorner("South", "West");
        tryAddCorner("South", "East");

    
        neighbors.Reverse();

        return neighbors;
    }
    
    
    private float getPotionCost(Node node)
    {
        float cost = 0f;
        string potionName = PowerUpManager.instance.getNameAt(node.toVector2i());

        //based on current implementation (where only mine has a static heuristic)
        switch (potionName)
        {
            case "Mine": return int.MaxValue;
        }


        return cost;
    }

    private float getDistance(Node a, Node b)
    {
        int xDist = Mathf.Abs(a.x - b.x);
        int yDist = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDist - yDist);

        return 14 * Mathf.Min(xDist, yDist) + 10 * remaining;

    }

    //test


    public void writeToTextFile()
    {

        BoundsInt? maxBoundsData = TilemapManager.instance.maxBoundsData;

        if (!maxBoundsData.HasValue)
            return;

        BoundsInt tileMapBounds = maxBoundsData.Value;


        string filename = Application.dataPath + "/PotionTracker.txt";

        if (!File.Exists(filename))
        {
            Debug.Log("File " + filename + "not Found!");
            return;
        }
        string buffer;

        File.WriteAllText(filename, "");


        for (int i = 0; i < tileMapBounds.size.y; i++)
        {
            buffer = "";
            for (int j = 0; j < tileMapBounds.size.x; j++)
            {
                Vector2Int cell = TilemapManager.instance.IndexToCell(j, i);

                switch (PowerUpManager.instance.getNameAt(cell))
                {
                    case "Invincibility": buffer += "I";  break;
                    case "Mine": buffer += "M"; break;
                    case "Chaos": buffer += "C";  break;
                    default: buffer += "."; break;
                }

                if (j + 1 != tileMapBounds.size.x)
                    buffer += " ";
            }
            if (i + 1 != tileMapBounds.size.y)
                buffer += "\n";
            File.AppendAllText(filename, buffer);
        }

    }

}
