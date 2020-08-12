using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V1_Grid : MonoBehaviour
{
    public float _xGridSize = 10.0f;
    public float _yGridSize = 10.0f;

    [Range(0.2f, 10f)]
    public float _nodeSize = 1.0f;

    public Vector3 _playerSize = Vector3.one;

    public int _xSize
    {
        get
        {
            return Mathf.RoundToInt(_xGridSize / _nodeSize);
        }
    }
    public int _ySize
    {
        get
        {
            return Mathf.RoundToInt(_yGridSize / _nodeSize);
        }
    }

    public V1_Node[,] _grid;

    public Vector3 GetWorldPostionOfNode(V1_Node node)
    {
        return GetWorldPostionOfNode(node._gridPositionX, node._gridPositionY);
    }
    public Vector3 GetWorldPostionOfNode(int xPos, int yPos)
    {
        float xPercent = (float)xPos / _xSize;
        float yPercent = (float)yPos / _ySize;

        float worldPositionX = _xGridSize * xPercent - (_xGridSize - _nodeSize) / 2f;
        float worldPositionY = _yGridSize * yPercent - (_yGridSize - _nodeSize) / 2f;

        return new Vector3(worldPositionX, worldPositionY, 0);
    }
    public V1_Node GetNodeOfWorldPostion(Vector3 worldPosition)
    {
        float xPercent = Mathf.Clamp01((worldPosition.x + (_xGridSize - _nodeSize) / 2f) / _xGridSize);
        float yPercent = Mathf.Clamp01((worldPosition.y + (_yGridSize - _nodeSize) / 2f) / _yGridSize);

        int xPos = Mathf.RoundToInt(xPercent * _xSize);
        int yPos = Mathf.RoundToInt(yPercent * _ySize);

        return _grid[xPos, yPos];
    }
}
