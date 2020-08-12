using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class B_Pathfinding_Grid : MonoBehaviour
{
    public L_BuildGrid _buildGrid;

    public LayerMask _unwalkableMaskFormObstacles;

    public Transform _start;
    public Transform _end;

    public L_Pathfinding_Astar _pathfinding;

    [Range(0, 1)]
    public float _timeBetweenLoop = 0.5f;
    [Range(0, 1)]
    public float _epsilon = 0.1f;
    public bool _autoBuild = false;

    L_Grid _area;

    bool _searching = false;

    public void BuildSimpleArea()
    {
        _area = _buildGrid.Build();
    }

    public void BuildObstacles()
    {
        for (int x = 0; x < _area._xGridSize; x++)
        {
            for (int y = 0; y < _area._yGridSize; y++)
            {
                bool walkable = !Physics.CheckBox(_buildGrid.GetWorldPostionOfNode(x, y), Vector3.one * _buildGrid._nodeWorldSize / 2f, Quaternion.identity, _unwalkableMaskFormObstacles);
                if (!walkable)
                {
                    _area._grid[x, y]._state = L_NodeStates.OBSTACLE;
                }
            }
        }
    }

    public void PlaceStartAndEnd()
    {
        if (_start != null && _end != null)
        {
            _start.localScale = new Vector3(_buildGrid._nodeWorldSize / 2f, 2 * _buildGrid._nodeWorldSize, _buildGrid._nodeWorldSize / 2f);
            _end.localScale = new Vector3(_buildGrid._nodeWorldSize / 2f, 2 * _buildGrid._nodeWorldSize, _buildGrid._nodeWorldSize / 2f);

            _buildGrid._pathPoints = new List<Vector3>{ _start.position, _end.position };
        }
    }

    public IEnumerator FindPath(bool loop = false)
    {
        if (_buildGrid._pathPoints.Count >= 2)
        {
            Debug.Log("FindPath running");
            _searching = true;

            int[] start = _buildGrid.GetNodeOfWorldPostion(_buildGrid._pathPoints[0]);
            int[] end = _buildGrid.GetNodeOfWorldPostion(_buildGrid._pathPoints[1]);
            _pathfinding = new L_Pathfinding_Astar(start[0], start[1], end[0], end[1], _area, _timeBetweenLoop);

            yield return _pathfinding.SearchPath();

            if (_pathfinding._found)
            {
                Debug.Log("Path Found");

                // Mark path
                L_Node current = _area._grid[end[0], end[1]];
                while (current.parent != null)
                {
                    current._state = L_NodeStates.PATH;
                    current = current.parent;
                }
                current._state = L_NodeStates.PATH;
            }
            else
            {
                Debug.Log("Path not Found");
            }
            if (loop)
            {
                _searching = false;
            }
        }
    }

    public void StopSearching()
    {
        StopCoroutine(FindPath());
        _searching = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_buildGrid._xGridWorldSize, _buildGrid._yGridWorldSize, 1));

        if (_pathfinding != null)
        {
            _pathfinding._timeBetweenLoop = _timeBetweenLoop;
        }

        if (_autoBuild && _buildGrid._nodeWorldSize > 0.01f)
        {
            if (!_searching)
            {
                // Build simple grid
                BuildSimpleArea();

                // Build obstacles in grid
                BuildObstacles();

                // Place start and end of path
                PlaceStartAndEnd();

                Debug.Log("Start searching");
                // PathFinding
                StartCoroutine(FindPath());
            }
        }

        if (_area != null)
        {
            for (int x = 0; x < _area._xGridSize; x++)
            {
                for (int y = 0; y < _area._yGridSize; y++)
                {
                    switch (_area._grid[x, y]._state)
                    {
                        case L_NodeStates.WALKABLE:
                            Gizmos.color = Color.white;
                            break;
                        case L_NodeStates.NOTWALKABLE:
                            Gizmos.color = Color.red;
                            break;
                        case L_NodeStates.OBSTACLE:
                            Gizmos.color = Color.black;
                            break;
                        case L_NodeStates.START:
                            Gizmos.color = Color.blue;
                            break;
                        case L_NodeStates.END:
                            Gizmos.color = Color.blue;
                            break;
                        case L_NodeStates.PATH:
                            Gizmos.color = Color.blue;
                            break;
                        case L_NodeStates.OPEN:
                            Gizmos.color = Color.green;
                            break;
                        case L_NodeStates.CLOSED:
                            Gizmos.color = Color.red;
                            break;
                    }
                    Gizmos.DrawCube(_buildGrid.GetWorldPostionOfNode(x, y), new Vector3(_buildGrid._nodeWorldSize - _epsilon, _buildGrid._nodeWorldSize - _epsilon, 1));
                }
            }
        }
    }
}
