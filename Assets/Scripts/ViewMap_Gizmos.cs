using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class ViewMap_Gizmos : ViewMap
    {
        private void OnValidate()
        {
            Reset();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_viewMap._width * _cellSize.x, _viewMap._height * _cellSize.x, 1));

            int width = _pathfinding._mapOfPath._map.GetLength(0);
            int height = _pathfinding._mapOfPath._map.GetLength(1);
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    switch (_pathfinding._mapOfPath._map[w, h]._state)
                    {
                        case Node.NodeStates.WALKABLE:
                            Gizmos.color = _colorOfWalkableCells;
                            break;
                        case Node.NodeStates.NOTWALKABLE:
                            Gizmos.color = _colorOfNotWalkableCells;
                            break;
                        case Node.NodeStates.START:
                            Gizmos.color = _colorOfStartingCells;
                            break;
                        case Node.NodeStates.END:
                            Gizmos.color = _colorOfEndingCells;
                            break;
                        case Node.NodeStates.PATH:
                            Gizmos.color = _colorOfPathCells;
                            break;
                        case Node.NodeStates.OPEN:
                            Gizmos.color = _colorOfOpenedCells;
                            break;
                        case Node.NodeStates.CLOSED:
                            Gizmos.color = _colorOfClosedCells;
                            break;
                    }
                    Gizmos.DrawCube(GetWorldPostionOfNode(w, h), _cellSize - _epsilonBetweenCells);
                }
            }
        }
        public override Vector3 GetWorldPostionOfNode(int w, int h)
        {
            int width = _pathfinding._mapOfPath._map.GetLength(0);
            int height = _pathfinding._mapOfPath._map.GetLength(1);

            Vector3 position = transform.position + new Vector3(_cellSize.x * (w + 0.5f - width * 0.5f), _cellSize.y * (h + 0.5f - height * 0.5f), 0);
            return position;
        }
    }
}