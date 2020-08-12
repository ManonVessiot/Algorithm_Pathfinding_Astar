using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class L_BuildGrid
{
    [Range(1, 100)]
    public int _xGridNodes;
    [Range(1, 100)]
    public int _yGridNodes;

    [Range(0.05f, 10)]
    public float _nodeWorldSize;

    public float _xGridWorldSize
    {
        get
        {
            return _xGridNodes * _nodeWorldSize;
        }
    }
    public float _yGridWorldSize
    {
        get
        {
            return _yGridNodes * _nodeWorldSize;
        }
    }

    public int _xGridSize
    {
        get
        {
            return Mathf.RoundToInt(_xGridWorldSize / _nodeWorldSize);
        }
    }
    public int _yGridSize
    {
        get
        {
            return Mathf.RoundToInt(_yGridWorldSize / _nodeWorldSize);
        }
    }

    public Vector3 _playerSize;

    public List<Vector3> _obtaclesPosition;
    public List<float> _obtaclesRadius;

    public List<Vector3> _pathPoints = new List<Vector3>();

    public L_BuildGrid(int xGridNodes, int yGridNodes, float nodeWorldSize)
    {
        _xGridNodes = xGridNodes;
        _yGridNodes = yGridNodes;
        _nodeWorldSize = nodeWorldSize > 0.1f ? nodeWorldSize : 1;

        _playerSize = Vector3.one * _nodeWorldSize * 0.9f;
        _obtaclesPosition = new List<Vector3>();
        _obtaclesRadius = new List<float>();
    }

    public void AddObstacle(Vector3 position, float radius)
    {
        _obtaclesPosition.Add(position);
        _obtaclesRadius.Add(radius);
    }

    public void RemoveObstacle(int index)
    {
        _obtaclesPosition.RemoveAt(index);
        _obtaclesRadius.RemoveAt(index);
    }

    public void ModifyObstacle(int index, Vector3 position, float radius)
    {
        _obtaclesPosition[index] = position;
        _obtaclesRadius[index] = radius;
    }

    public void AddPoint(Vector3 point, bool remplace = true)
    {
        if (_pathPoints.Count < 2)
        {
            _pathPoints.Add(point);
        }
        else if (remplace)
        {
            _pathPoints[0] = _pathPoints[1];
            _pathPoints[1] = point;
        }
    }
    public void RemovePoint(int index)
    {
        _pathPoints.RemoveAt(index);
    }

    public L_Grid Build()
    {
        L_Grid area = new L_Grid(_xGridSize, _yGridSize);

        for (int i = 0; i < _obtaclesPosition.Count; i++)
        {
            int[] topLeftPlayer = GetNodeOfWorldPostion(_obtaclesPosition[i] + new Vector3(-_obtaclesRadius[i], _obtaclesRadius[i], 0) + new Vector3(-_playerSize.x, _playerSize.z, 0));
            int[] BottomRightPlayer = GetNodeOfWorldPostion(_obtaclesPosition[i] + new Vector3(_obtaclesRadius[i], -_obtaclesRadius[i], 0) + new Vector3(_playerSize.x, -_playerSize.z, 0));

            int[] topLeft = GetNodeOfWorldPostion(_obtaclesPosition[i] + new Vector3(-_obtaclesRadius[i], _obtaclesRadius[i], 0));
            int[] BottomRight = GetNodeOfWorldPostion(_obtaclesPosition[i] + new Vector3(_obtaclesRadius[i], -_obtaclesRadius[i], 0));

            for (int x = topLeftPlayer[0]; x <= BottomRightPlayer[0]; x++)
            {
                for (int y = BottomRightPlayer[1]; y <= topLeftPlayer[1]; y++)
                {
                    if (x >= 0 && x < area._xGridSize && y >= 0 && y < area._yGridSize)
                    {
                        if (x < topLeftPlayer[0] || x > BottomRightPlayer[0] || y < BottomRightPlayer[1] || y > topLeftPlayer[1])
                        {
                            area._grid[x, y]._state = L_NodeStates.NOTWALKABLE;
                        }
                        else
                        {
                            area._grid[x, y]._state = L_NodeStates.OBSTACLE;
                        }
                    }
                }
            }
        }
        AddPointsToArea(area);

        return area;
    }

    protected void AddPointsToArea(L_Grid area, bool remove = false)
    {
        if (_pathPoints.Count > 0)
        {
            bool startWalkable = true;
            int[] start = GetNodeOfWorldPostion(_pathPoints[0]);
            if (start[0] >= 0 && start[0] < area._xGridSize && start[1] >= 0 && start[1] < area._yGridSize)
            {
                if (area._grid[start[0], start[1]]._state == L_NodeStates.WALKABLE)
                {
                    area._grid[start[0], start[1]]._state = L_NodeStates.START;
                }
                else if (remove)
                {
                    _pathPoints.RemoveAt(0);
                    startWalkable = false;
                }
            }
            if (_pathPoints.Count > 1)
            {
                int index = startWalkable ? 1 : 0;
                int[] end = GetNodeOfWorldPostion(_pathPoints[index]);
                if (end[0] >= 0 && end[0] < area._xGridSize && end[1] >= 0 && end[1] < area._yGridSize)
                {
                    if (area._grid[end[0], end[1]]._state == L_NodeStates.WALKABLE)
                    {
                        area._grid[end[0], end[1]]._state = L_NodeStates.END;
                    }
                    else if (remove)
                    {
                        _pathPoints.RemoveAt(index);
                    }
                }
            }
        }
    }

    public Vector3 GetWorldPostionOfNode(int xPos, int yPos)
    {
        float xPercent = (float)xPos / _xGridSize;
        float yPercent = (float)yPos / _yGridSize;

        float worldPositionX = _xGridWorldSize * xPercent - (_xGridWorldSize - _nodeWorldSize) / 2f;
        float worldPositionY = _yGridWorldSize * yPercent - (_yGridWorldSize - _nodeWorldSize) / 2f;

        return new Vector3(worldPositionX, worldPositionY, 0);
    }
    public int[] GetNodeOfWorldPostion(Vector3 worldPosition)
    {
        float xPercent = Mathf.Clamp01((worldPosition.x + (_xGridWorldSize - _nodeWorldSize) / 2f) / _xGridWorldSize);
        float yPercent = Mathf.Clamp01((worldPosition.y + (_yGridWorldSize - _nodeWorldSize) / 2f) / _yGridWorldSize);

        int xPos = Mathf.RoundToInt(xPercent * _xGridSize);
        int yPos = Mathf.RoundToInt(yPercent * _yGridSize);

        if (xPos == _xGridSize) xPos--;
        if (yPos == _yGridSize) yPos--;

        return new int []{ xPos, yPos };
    }
}
