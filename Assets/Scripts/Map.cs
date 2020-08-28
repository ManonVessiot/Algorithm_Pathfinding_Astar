using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class Map
    {
        public Node[,] _map;

        public Map(int width, int height, List<MapPosition> noneWalkableCells)
        {
            _map = new Node[width, height];
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    Node.NodeStates state = noneWalkableCells.Contains(new MapPosition(w, h)) ? Node.NodeStates.NOTWALKABLE : Node.NodeStates.WALKABLE;

                    _map[w, h] = new Node(state, w, h);
                }
            }
        }

        internal List<Node> GetNeighbours(Node current)
        {
            List<Node> neighbours = new List<Node>();

            int width = _map.GetLength(0);
            int height = _map.GetLength(1);
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int neihgbourX = current._x + x;
                    int neihgbourY = current._y + y;

                    if (0 <= neihgbourX && neihgbourX < width && 0 <= neihgbourY && neihgbourY < height)
                    {
                        neighbours.Add(_map[neihgbourX, neihgbourY]);
                    }
                }
            }
            return neighbours;
        }
    }
}