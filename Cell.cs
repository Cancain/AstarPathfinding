using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    //Constructor
    public Cell(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        gridX = _gridX;
        gridY = _gridY;
    }

    //World positions
    public Vector3 worldPosition;

    //Grid positions
    public int gridX;
    public int gridY;

    //Pathfinding
    public int gCost;
    public int hCost;
    public Cell parent;
    public bool walkable;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


}
