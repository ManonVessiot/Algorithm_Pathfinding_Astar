using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class BuildObstaclesWithMouse : MonoBehaviour
    {
        public MapToBuild _map; // _map._width && _map._height
        public ViewMap _viewMap; // _viewMap._cellSize

        Vector2 _ScreenSize = new Vector2(Screen.width, Screen.height);
        bool addCellsToObstacles = true;

        private void Start()
        {
            Debug.Log("_ScreenSize : " + _ScreenSize);
        }

        // Update is called once per frame
        void Update()
        {
            if (!_viewMap._solving && (0 <= Input.mousePosition.x && Input.mousePosition.x < _ScreenSize.x) && (0 <= Input.mousePosition.y && Input.mousePosition.y < _ScreenSize.y))
            {
                MapPosition mousePose = GetMapPostionOfScreenPoint(Input.mousePosition);

                if (Input.GetMouseButton(0))
                {
                    _map._start = mousePose;
                    _viewMap.Reset();
                }
                else if (Input.GetMouseButton(1))
                {
                    _map._end = mousePose;
                    _viewMap.Reset();
                }
                else if (Input.GetMouseButtonDown(2))
                {
                    if (_map._notWalkableCells.Contains(mousePose))
                    {
                        addCellsToObstacles = false;
                    }
                    else
                    {
                        addCellsToObstacles = true;
                    }
                }
                else if (Input.GetMouseButton(2))
                {
                    bool contains = _map._notWalkableCells.Contains(mousePose);
                    if (!addCellsToObstacles && contains)
                    {
                        _map._notWalkableCells.Remove(mousePose);
                    }
                    else if (addCellsToObstacles && !contains)
                    {
                        _map._notWalkableCells.Add(mousePose);
                    }
                    _viewMap.Reset();
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_viewMap._solving)
                {
                    Debug.Log("StopSolving");
                    _viewMap.StopSolving();
                }
                else
                {
                    Debug.Log("StartSolving");
                    _viewMap.StartSolving();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (_viewMap._solving)
                {
                    Debug.Log("StopSolving");
                    _viewMap.StopSolving();
                }
                Debug.Log("Reset");
                _viewMap.Reset();
            }
        }

        public virtual MapPosition GetMapPostionOfScreenPoint(Vector2 screenPoint)
        {
            int w = Mathf.FloorToInt(screenPoint.x / _viewMap._cellSize.x);
            int h = Mathf.FloorToInt(screenPoint.y / _viewMap._cellSize.y);

            return new MapPosition(w, h);
        }
    }
}