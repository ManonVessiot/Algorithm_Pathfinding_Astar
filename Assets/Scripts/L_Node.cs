using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Node
{
    public L_NodeStates _state;
    public int _gridPositionX;
    public int _gridPositionY;

    public bool _walkable
    {
        get
        {
            return _state == L_NodeStates.WALKABLE || _state == L_NodeStates.START || _state == L_NodeStates.END;
        }
    }

    public int _gCost;
    public int _hCost;
    public int _fCost
    {
        get
        {
            return _gCost + _hCost;
        }
    }

    public L_Node parent;

    public L_Node(L_NodeStates state, int gridPositionX, int gridPositionY)
    {
        _state = state;
        _gridPositionX = gridPositionX;
        _gridPositionY = gridPositionY;
        _gCost = -1;
        _hCost = -1;
        parent = null;
    }

}
