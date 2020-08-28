using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public abstract class Pathfinding
    {
        public Map _mapOfPath;
        public MapPosition _start;
        public MapPosition _end;
        public Node _startingNode
        {
            get
            {
                return _mapOfPath._map[_start._x, _start._y];
            }
        }

        public Node _endingNode
        {
            get
            {
                return _mapOfPath._map[_end._x, _end._y];
            }
        }

        public int _loop;
        protected bool _found = false;

        public Pathfinding(Map mapOfPath, MapPosition start, MapPosition end)
        {
            _mapOfPath = mapOfPath;
            _start = start;
            _end = end;

            _startingNode._state = Node.NodeStates.START;
            _endingNode._state = Node.NodeStates.END;

            _loop = 0;
        }
        protected int GetHeuristicDistance(Node nodeA, Node nodeB)
        {
            int dstX = Mathf.Abs(nodeA._x - nodeB._x);
            int dstY = Mathf.Abs(nodeA._y - nodeB._y);

            return 14 * Mathf.Min(dstX, dstY) + 10 * Mathf.Abs(dstX - dstY);
        }

        public virtual bool NextStep()
        {
            if (!_found)
            {
                _loop++;
                _found = Algorithm();
            }
            return _found;
        }

        protected abstract bool Algorithm();
    }
}