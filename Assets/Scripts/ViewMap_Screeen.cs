using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pathfinding_Astar
{
    public class ViewMap_Screeen : ViewMap
    {
        public GameObject _cellImage;
        Image[,] _mapGameObjects;

        Vector2 _ScreenSize = new Vector2(Screen.width, Screen.height);

        private void Awake()
        {
            _whenInPlayMode = true;
            _whenInEditMode = false;
        }

        public override void Reset()
        {
            _cellSize.x = _ScreenSize.x / (float)_viewMap._width;
            _cellSize.y = _ScreenSize.y / (float)_viewMap._height;

            if (_mapGameObjects == null || (_mapGameObjects.GetLength(0) != _viewMap._width || _mapGameObjects.GetLength(1) != _viewMap._height))
            {
                if (_mapGameObjects != null)
                {
                    foreach (Image obj in _mapGameObjects)
                    {
                        if (obj != null)
                        {
                            Destroy(obj.gameObject);
                        }
                    }
                }

                _mapGameObjects = new Image[_viewMap._width, _viewMap._height];
                for (int w = 0; w < _viewMap._width; w++)
                {
                    for (int h = 0; h < _viewMap._height; h++)
                    {
                        GameObject obj = Instantiate(_cellImage, transform);
                        obj.name = "_mapGameObjects[" + w + ", " + h + "]";

                        _mapGameObjects[w, h] = obj.GetComponent<Image>();

                        RectTransform rect = _mapGameObjects[w, h].GetComponent<RectTransform>();
                        rect.position = GetWorldPostionOfNode(w, h);
                    }
                }
            }
            for (int w = 0; w < _viewMap._width; w++)
            {
                for (int h = 0; h < _viewMap._height; h++)
                {
                    RectTransform rect = _mapGameObjects[w, h].GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(_cellSize.x, _cellSize.y) - _epsilonBetweenCells;
                }
            }

            ResetMap();
        }

        protected override void UpdateView()
        {
            if (_cellImage != null && _mapGameObjects != null)
            {
                int width = _mapGameObjects.GetLength(0);
                int height = _mapGameObjects.GetLength(1);
                for (int w = 0; w < width; w++)
                {
                    for (int h = 0; h < height; h++)
                    {
                        Color cellColor = Color.white;
                        switch (_pathfinding._mapOfPath._map[w, h]._state)
                        {
                            case Node.NodeStates.WALKABLE:
                                cellColor = _colorOfWalkableCells;
                                break;
                            case Node.NodeStates.NOTWALKABLE:
                                cellColor = _colorOfNotWalkableCells;
                                break;
                            case Node.NodeStates.START:
                                cellColor = _colorOfStartingCells;
                                break;
                            case Node.NodeStates.END:
                                cellColor = _colorOfEndingCells;
                                break;
                            case Node.NodeStates.PATH:
                                cellColor = _colorOfPathCells;
                                break;
                            case Node.NodeStates.OPEN:
                                cellColor = _colorOfOpenedCells;
                                break;
                            case Node.NodeStates.CLOSED:
                                cellColor = _colorOfClosedCells;
                                break;
                        }

                        _mapGameObjects[w, h].color = cellColor;
                    }
                }
            }
        }
        public override Vector3 GetWorldPostionOfNode(int w, int h)
        {
            Vector3 position = new Vector3((w + 0.5f) * _cellSize.x, (h + 0.5f) * _cellSize.y, 0);
            return position;
        }
    }
}