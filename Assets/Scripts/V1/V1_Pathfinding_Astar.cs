using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Pathfinding_Astar : MonoBehaviour
{
    public V1_Grid _gridScript;
    public Transform _startingPoint;
    public Transform _endingPoint;

    [Range(0, 1)]
    public float _timeBetweenLoop = 0.5f;

    [System.NonSerialized]
    public V1_Node _startingNode;
    [System.NonSerialized]
    public V1_Node _endingNode;

    public V1_GridDraw _gridDraw;

    public void FindStartAndEndNode()
    {
        _startingNode = _gridScript.GetNodeOfWorldPostion(_startingPoint.position);
        _endingNode = _gridScript.GetNodeOfWorldPostion(_endingPoint.position);

        foreach (V1_Node node in _gridScript._grid)
        {
            node._hCost = GetDistance(node, _endingNode);
        }
    }

    int GetDistance(V1_Node nodeA, V1_Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA._gridPositionX - nodeB._gridPositionX);
        int dstY = Mathf.Abs(nodeA._gridPositionY - nodeB._gridPositionY);

        return 14 * Mathf.Min(dstX, dstY) + 10 * Mathf.Abs(dstX - dstY);
    }

    public void StartAstar()
    {
        StartCoroutine(FindPath());
    }

    IEnumerator FindPath()
    {
        FindStartAndEndNode();

        yield return Astar();

        V1_Node current = _endingNode;
        _gridDraw.SetPointNodes(current);
        while (current.parent != null)
        {
            current = current.parent;
            _gridDraw.SetPointNodes(current);
        }
    }
    

    private IEnumerator Astar()
    {
        List<V1_Node> open = new List<V1_Node>();
        List<V1_Node> closed = new List<V1_Node>();

        open.Add(_startingNode);

        while (open.Count > 0)
        {
            yield return new WaitForSeconds(_timeBetweenLoop);

            V1_Node current = GetLowest_fCost(open);
            open.Remove(current);
            closed.Add(current);
            if (current != _startingNode && current != _endingNode)
            {
                _gridDraw.SetPointClosedNodes(current);
            }

            if (current == _endingNode)
            {
                break;
            }

            List<V1_Node> neighbours = GetNeighbours(current);
            foreach (V1_Node neighbour in neighbours)
            {
                if (!neighbour._walkable || closed.Contains(neighbour))
                {
                    continue;
                }
                int costCurrentToNeighbour = GetDistance(current, neighbour);
                int newgCost = current._gCost + costCurrentToNeighbour;
                if (neighbour._gCost > newgCost || !open.Contains(neighbour))
                {
                    neighbour._gCost = newgCost;
                    neighbour.parent = current;
                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                        _gridDraw.SetPointOpenNodes(neighbour);
                    }
                }
            }
        }
    }

    private List<V1_Node> GetNeighbours(V1_Node current)
    {
        List<V1_Node> neighbours = new List<V1_Node>();
        
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                int neihgbourX = current._gridPositionX + x;
                int neihgbourY = current._gridPositionY + y;

                if (0 <= neihgbourX && neihgbourX < _gridScript._xSize && 0 <= neihgbourY && neihgbourY < _gridScript._ySize)
                {
                    neighbours.Add(_gridScript._grid[neihgbourX, neihgbourY]);
                }
            }
        }


        return neighbours;
    }

    private V1_Node GetLowest_fCost(List<V1_Node> open)
    {
        if (open.Count == 0)
        {
            return null;
        }

        V1_Node node = open[0];
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
