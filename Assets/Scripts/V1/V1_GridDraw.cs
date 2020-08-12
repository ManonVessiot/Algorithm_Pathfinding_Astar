using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class V1_GridDraw : MonoBehaviour
{
    [Range(0.0f, 0.9f)]
    public float _nodeEpsilon = 0.1f;

    public Material _planeMat;
    public Material _walkableMat;
    public Material _notWalkableMat;
    public Material _pointsMat;
    public Material _pointsClosedMat;
    public Material _pointsOpenMat;

    public GameObject[,] _grid;

    public void DrawGrid(V1_Grid gridScript)
    {
        DestroyNodeCube(gridScript);

        GameObject _gridPlane;

        _gridPlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _gridPlane.name = "GridPlane";
        _gridPlane.GetComponent<Renderer>().material = _planeMat;
        _gridPlane.transform.SetParent(gridScript.transform);
        _gridPlane.transform.localScale = new Vector3(gridScript._xGridSize, gridScript._yGridSize, gridScript._nodeSize * (1 - _nodeEpsilon) / 2f);
        _gridPlane.transform.position = gridScript.transform.position;


        _grid = new GameObject[gridScript._xSize, gridScript._ySize];
        for (int x = 0; x < gridScript._xSize; x++)
        {
            for (int y = 0; y < gridScript._ySize; y++)
            {
                _grid[x,y] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _grid[x, y].name = "Node (" + x + ", " + y + ")";
                _grid[x, y].GetComponent<Renderer>().material = gridScript._grid[x,y]._walkable? _walkableMat:_notWalkableMat;
                _grid[x, y].transform.SetParent(gridScript.transform);
                _grid[x, y].transform.localScale = gridScript._nodeSize * (1 - _nodeEpsilon) * Vector3.one;
                _grid[x, y].transform.position = gridScript.GetWorldPostionOfNode(gridScript._grid[x, y]);
            }
        }
    }

    private void DestroyNodeCube(V1_Grid gridScript)
    {
        foreach (Transform child in gridScript.GetComponentsInChildren<Transform>())
        {
            if (child != gridScript.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    public void SetPointNodes(V1_Node point)
    {
        _grid[point._gridPositionX, point._gridPositionY].GetComponent<Renderer>().material = _pointsMat;
    }

    public void SetPointClosedNodes(V1_Node point)
    {
        _grid[point._gridPositionX, point._gridPositionY].GetComponent<Renderer>().material = _pointsClosedMat;
    }

    public void SetPointOpenNodes(V1_Node point)
    {
        _grid[point._gridPositionX, point._gridPositionY].GetComponent<Renderer>().material = _pointsOpenMat;
    }
}
