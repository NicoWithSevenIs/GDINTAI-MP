using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class Pathfinding 
{

    public Node[,] nodes;

    public Pathfinding()
    {
    
    }


    List<Node> getPathAStar(Vector2Int originCell, Vector2Int targetCell)
    {

        ConsolidateGrid();

        List<Node> openList = new List<Node>() {  new Node(originCell) };
        List<Node> closeList = new List<Node>();





        List<Node> path = new List<Node>();
        return path;
    }

    private void ConsolidateGrid()
    {
        BoundsInt b = TilemapManager.instance.maxBoundsData.Value;
        nodes = new Node[b.size.x, b.size.y];

        for (int row = 0; row < b.size.y; row++)
        {
            for (int col = 0; col < b.size.x; col++)
            {

                Vector2Int cell = TilemapManager.instance.IndexToCell(col, row);

                nodes[col, row] = new Node(cell);
                nodes[col, row].setGCost(int.MaxValue);
                nodes[col, row].CalculateFCost();


            }

        }

    }

    private float getPotionCost(Vector2Int cellPos)
    {
        string potionName = PowerUpManager.instance.getNameAt(cellPos);

        switch(potionName)
        {
            case "Invincibility":  
                
            break;

            case "Mine": 
                
            break;

            case "Chaos":  
                
            break;
        }

        return 0f;
    }

    private float getHeuristic(Vector2Int cellPos)
    {
            
        return 0;
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
