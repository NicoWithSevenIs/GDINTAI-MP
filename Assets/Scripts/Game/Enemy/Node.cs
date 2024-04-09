using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{

    public int x { get; private set; }
    public int y { get; private set; }

    public float gCost { get; private set; } = 0f;
    public float hCost { get; private set; } = 0f;
    public float fCost { get; private set; } = 0f;

    Node previous = null;

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

    public void setGCost(float gCost)
    {
        this.gCost = gCost;
    }

    public void setHCost(float hCost) { 
        this.hCost = hCost;
    }


    void CalculateFCost()
    {
        fCost = gCost + hCost;
    }   

}
