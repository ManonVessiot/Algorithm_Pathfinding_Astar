using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_Astar : MonoBehaviour
{
    public Grid _gridScript;
    public Transform _startingPoint;
    public Transform _endingPoint;

    [Range(0, 1)]
    public float _timeBetweenLoop = 0.5f;

    [System.NonSerialized]
    public Node _startingNode;
    [System.NonSerialized]
    public Node _endingNode;

    public GridDraw _gridDraw;

    public void FindStartAndEndNode()
    {
        _startingNode = _gridScript.GetNodeOfWorldPostion(_startingPoint.position);
        _endingNode = _gridScript.GetNodeOfWorldPostion(_endingPoint.position);

        foreach (Node node in _gridScript._grid)
        {
            node._hCost = GetDistance(node, _endingNode);
        }
    }

    int GetDistance(Node nodeA, Node nodeB)
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

        Node current = _endingNode;
        _gridDraw.SetPointNodes(current);
        while (current.parent != null)
        {
            current = current.parent;
            _gridDraw.SetPointNodes(current);
        }
    }
    

    private IEnumerator Astar()
    {
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(_startingNode);

        while (open.Count > 0)
        {
            yield return new WaitForSeconds(_timeBetweenLoop);

            Node current = GetLowest_fCost(open);
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

            List<Node> neighbours = GetNeighbours(current);
            foreach (Node neighbour in neighbours)
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

    private List<Node> GetNeighbours(Node current)
    {
        List<Node> neighbours = new List<Node>();
        
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

    private Node GetLowest_fCost(List<Node> open)
    {
        if (open.Count == 0)
        {
            return null;
        }

        Node node = open[0];
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
