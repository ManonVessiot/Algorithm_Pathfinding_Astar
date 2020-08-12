using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class L_Pathfinding
{
    public int _startX;
    public int _startY;

    public int _endX;
    public int _endY;

    public L_Node _startingNode
    {
        get
        {
            return _area._grid[_startX, _startY];
        }
    }

    public L_Node _endingNode
    {
        get
        {
            return _area._grid[_endX, _endY];
        }
    }

    public L_Grid _area;

    public bool _searching = false;
    public bool _found = false;
    public float _timeBetweenLoop = 0.5f;

    public L_Pathfinding(int startX, int startY, int endX, int endY, L_Grid area, float timeBetweenLoop)
    {
        _startX = startX;
        _startY = startY;
        _endX = endX;
        _endY = endY;
        _area = area;
        _timeBetweenLoop = timeBetweenLoop;

        _startingNode._state = L_NodeStates.START;
        _endingNode._state = L_NodeStates.END;
    }

    public abstract IEnumerator SearchPath(bool lookAlgorithmSearching);

    protected abstract bool Algorithme();

    protected int GetHeuristicDistance(L_Node nodeA, L_Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gridPositionX - nodeB._gridPositionX);
        int dstY = Mathf.Abs(nodeA._gridPositionY - nodeB._gridPositionY);

        return 14 * Mathf.Min(dstX, dstY) + 10 * Mathf.Abs(dstX - dstY);
    }
}
