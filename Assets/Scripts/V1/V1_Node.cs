using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Node
{
    public bool _walkable;
    public int _gridPositionX;
    public int _gridPositionY;

    public int _gCost;
    public int _hCost;
    public int _fCost
    {
        get
        {
            return _gCost + _hCost;
        }
    }

    public V1_Node parent = null;

    public V1_Node(bool walkable, int gridPositionX, int gridPositionY)
    {
        _walkable = walkable;
        _gridPositionX = gridPositionX;
        _gridPositionY = gridPositionY;
    }
}
