using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool IsBlocked;
    public Vector3 Position;

    public int GridX;
    public int GridY;

    public int GCost;
    public int HCost;

    public Node Parent;

    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    public Node(bool isBlocked, Vector3 position, int gridX, int gridY)
    {
        IsBlocked = isBlocked;
        Position = position;
        GridX = gridX;
        GridY = gridY;
    }
}
