using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class MapToBuild : MonoBehaviour
    {
        public int _width = 5;
        public int _height = 3;
        public MapPosition _start;
        public MapPosition _end;
        public List<MapPosition> _notWalkableCells;
        public MapPosition _cellShift;
    }
}