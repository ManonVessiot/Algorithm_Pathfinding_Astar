using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_Grid
{
    public int _xGridSize = 10;
    public int _yGridSize = 10;

    public L_Node[,] _grid;

    public L_Grid(int xGridSize, int yGridSize)
    {
        _xGridSize = xGridSize;
        _yGridSize = yGridSize;

        _grid = new L_Node[_xGridSize, _yGridSize];
        for (int x = 0; x < _xGridSize; x++)
        {
            for (int y = 0; y < _yGridSize; y++)
            {
                _grid[x, y] = new L_Node(L_NodeStates.WALKABLE, x, y);
            }
        }
    }

    internal List<L_Node> GetNeighbours(L_Node current)
    {
        List<L_Node> neighbours = new List<L_Node>();

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

                if (0 <= neihgbourX && neihgbourX < _xGridSize && 0 <= neihgbourY && neihgbourY < _yGridSize)
                {
                    neighbours.Add(_grid[neihgbourX, neihgbourY]);
                }
            }
        }
        return neighbours;
    }
}
