using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{

    public Node[,] nodes;

    public Pathfinding()
    {
        BoundsInt b = TilemapManager.instance.maxBoundsData.Value;
        nodes = new Node[b.size.x, b.size.y];

    }


    List<Node> getPathAStar(Vector2Int originCell, Vector2Int targetCell)
    {

        List<Node> openList = new List<Node>() {  new Node(originCell) };
        List<Node> closeList = new List<Node>();





        List<Node> path = new List<Node>();
        return path;
    }

    void ConsolidateGrid()
    {

    }

}
