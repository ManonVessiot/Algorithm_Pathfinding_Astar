using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class FindObstaclesWithPhysics : MonoBehaviour
    {
        public MapToBuild _map;
        public ViewMap _viewMap;
        public LayerMask _unwalkableMaskForObstacles;
        public LayerMask _startMaskForPoint;
        public LayerMask _endMaskForPoint;
        public bool _clearObstaclesListBefore = true;

        public void FindObstacles()
        {
            if (_clearObstaclesListBefore)
            {
                _map._notWalkableCells.Clear();
            }

            for (int w = 0; w < _map._width; w++)
            {
                for (int h = 0; h < _map._height; h++)
                {
                    bool walkable = !Physics.CheckBox(_viewMap.GetWorldPostionOfNode(w, h), new Vector3(_viewMap._cellSize.x, _viewMap._cellSize.y, 1) / 2f,
                        Quaternion.identity, _unwalkableMaskForObstacles);
                    if (!walkable)
                    {
                        _map._notWalkableCells.Add(new MapPosition(w, h));
                    }
                    bool startPoint = !Physics.CheckBox(_viewMap.GetWorldPostionOfNode(w, h), new Vector3(_viewMap._cellSize.x, _viewMap._cellSize.y, 1) / 2f,
                        Quaternion.identity, _startMaskForPoint);
                    if (!startPoint)
                    {
                        _map._start = new MapPosition(w, h);
                    }
                    bool endPoint = !Physics.CheckBox(_viewMap.GetWorldPostionOfNode(w, h), new Vector3(_viewMap._cellSize.x, _viewMap._cellSize.y, 1) / 2f,
                        Quaternion.identity, _endMaskForPoint);
                    if (!endPoint)
                    {
                        _map._end = new MapPosition(w, h);
                    }
                }
            }
        }
    }
}