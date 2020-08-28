using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    [System.Serializable]
    #pragma warning disable CS0659 // Le type se substitue à Object.Equals(object o) mais pas à Object.GetHashCode()
    public class MapPosition
    #pragma warning restore CS0659 // Le type se substitue à Object.Equals(object o) mais pas à Object.GetHashCode()
    {
        public int _x;
        public int _y;

        public MapPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;

            MapPosition c = obj as MapPosition;
            if ((System.Object)c == null)
                return false;

            return _x == c._x && _y == c._y;
        }

        public override string ToString()
        {
            return "(" + _x + ", " + _y + ")";
        }
    }
}
