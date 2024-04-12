using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//will optimize this if i have the time
public class Node
{

    public int x { get; private set; }
    public int y { get; private set; }

    public float gCost;
    public float hCost;
    public float fCost;

    public Node previous { get; private set; }

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Node(Vector2Int cell)
    {
        this.x = cell.x;
        this.y = cell.y;
    }

    public void assignPrevious(Node node)
    {
        if (node == this)
        {
            Debug.LogException(new System.Exception("Previous cannot be itself"));
            return;
        }


        this.previous = node;
    }


    public Vector2Int toVector2i()
    {
        return new Vector2Int(x, y);
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

   
}
