using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Pathfinding_Astar : L_Pathfinding
{
    List<L_Node> _open = new List<L_Node>();
    List<L_Node> _closed = new List<L_Node>();

    L_Node _current;


    public L_Pathfinding_Astar(int startX, int startY, int endX, int endY, L_Grid area, float timeBetweenLoop) : base(startX, startY, endX, endY, area, timeBetweenLoop)
    {
        _open.Add(_startingNode);
    }

    public override IEnumerator SearchPath(bool lookAlgorithmSearching)
    {
        _searching = true;
        // int _loop = 0;
        while (_searching)
        {
            // _loop++;
            _found = Algorithme();
            if (lookAlgorithmSearching)
            {
                yield return new WaitForSeconds(_timeBetweenLoop);
            }
        }
    }

    protected override bool Algorithme()
    {
        _current = GetLowest_fCost(_open);
        _open.Remove(_current);
        _closed.Add(_current);
        if (_current != _startingNode)
        {
            _current._state = L_NodeStates.CLOSED;
        }

        if (_current == _endingNode)
        {
            return true;
        }

        List<L_Node> neighbours = _area.GetNeighbours(_current);
        foreach (L_Node neighbour in neighbours)
        {
            if (!neighbour._walkable || _closed.Contains(neighbour))
            {
                continue;
            }
            int costCurrentToNeighbour = GetHeuristicDistance(_current, neighbour);
            int newgCost = _current._gCost + costCurrentToNeighbour;
            if (neighbour._gCost > newgCost || !_open.Contains(neighbour))
            {
                neighbour._gCost = newgCost;
                neighbour.parent = _current;
                if (!_open.Contains(neighbour))
                {
                    _open.Add(neighbour);
                    neighbour._state = L_NodeStates.OPEN;
                }
            }
        }

        if (_open.Count == 0 || _endingNode.parent != null)
        {
            _searching = false;
        }

        return _endingNode.parent != null;
    }

    private L_Node GetLowest_fCost(List<L_Node> open)
    {
        if (open.Count == 0)
        {
            return null;
        }

        L_Node node = open[0];
        for (int i = 1; i < open.Count; i++)
        {
            if ((open[i]._fCost < node._fCost) || (open[i]._fCost == node._fCost && open[i]._hCost < node._hCost))
            {
                node = open[i];
            }
        }
        return node;
    }
}
