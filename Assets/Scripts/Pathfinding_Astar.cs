using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class Pathfinding_Astar : Pathfinding
    {
        List<Node> _open = new List<Node>();
        List<Node> _closed = new List<Node>();

        Node _current;

        public Pathfinding_Astar(Map mapOfPath, MapPosition start, MapPosition end) : base(mapOfPath, start, end)
        {
            _open.Add(_startingNode);
        }

        protected override bool Algorithm()
        {
            if (_open.Count > 0 && _endingNode.parent == null)
            {
                _current = GetLowest_fCost(_open);
                _open.Remove(_current);
                _closed.Add(_current);
                if (_current != _startingNode)
                {
                    _current._state = Node.NodeStates.CLOSED;
                }

                if (_current == _endingNode)
                {
                    return true;
                }

                List<Node> neighbours = _mapOfPath.GetNeighbours(_current);
                foreach (Node neighbour in neighbours)
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
                            neighbour._state = Node.NodeStates.OPEN;
                        }
                    }
                }
            }
            
            if (_endingNode.parent != null)
            {
                // Mark path
                Node current = _endingNode;
                while (current.parent != null)
                {
                    current._state = Node.NodeStates.PATH;
                    current = current.parent;
                }
                current._state = Node.NodeStates.PATH;
            }

            return (_open.Count == 0 || _endingNode.parent != null);
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
}