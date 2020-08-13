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

    public bool _lookAlgorithmSearching = true;

    [Range(0, 1)]
    public float _timeBetweenLoop = 0.5f;
    [Range(0, 1)]
    public float _epsilon = 0.1f;

    L_Grid _area;
    bool _searching = true;

    Coroutine currentSearch;

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

            _buildGrid._pathPoints = new List<Vector3>();
            int[] startingNodePose = _buildGrid.GetNodeOfWorldPostion(_start.position);
            int[] endingNodePose = _buildGrid.GetNodeOfWorldPostion(_end.position);

            if (_area._grid[startingNodePose[0], startingNodePose[1]]._state != L_NodeStates.OBSTACLE)
            {
                _buildGrid._pathPoints.Add(_start.position);
            }
            if (_area._grid[endingNodePose[0], endingNodePose[1]]._state != L_NodeStates.OBSTACLE && (endingNodePose[0] != startingNodePose[0] || endingNodePose[1] != startingNodePose[1]))
            {
                _buildGrid._pathPoints.Add(_end.position);
            }
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

            float _startingTime = Time.time;
            yield return _pathfinding.SearchPath(_lookAlgorithmSearching);
            float _endingTime = Time.time;

            if (_pathfinding._found)
            {
                int ms = Mathf.RoundToInt((_endingTime - _startingTime) * 1000);

                string lastingTime = ms + " ms";
                if (ms >= 1000 && ms < 60000)
                {
                    lastingTime = Mathf.RoundToInt((ms / 1000f)) + " s (" + lastingTime + ")";
                }

                Debug.Log("Path Found in " + lastingTime);

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
        else
        {
            _pathfinding = null;
        }
    }

    public void StopSearching(bool searching = false)
    {
        if (currentSearch != null)
        {
            StopCoroutine(currentSearch);
            Debug.Log("Last search stopped");
        }
        currentSearch = null;
        _searching = searching;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_buildGrid._xGridWorldSize, _buildGrid._yGridWorldSize, 1));

        if (_pathfinding != null)
        {
            // update _timeBetweenLoop
            _pathfinding._timeBetweenLoop = _timeBetweenLoop;
        }

        if (_buildGrid._nodeWorldSize > 0.01f)
        {
            // nodes not too small
            if (_start != null && _end != null && _pathfinding != null)
            {
                int[] startingNode = _buildGrid.GetNodeOfWorldPostion(_start.position);
                bool pointsMoved = !(_pathfinding._startingNode._gridPositionX == startingNode[0] && _pathfinding._startingNode._gridPositionY == startingNode[1]);
                if (!pointsMoved)
                {
                    int[] endingNode = _buildGrid.GetNodeOfWorldPostion(_end.position);
                    pointsMoved = !(_pathfinding._endingNode._gridPositionX == endingNode[0] && _pathfinding._endingNode._gridPositionY == endingNode[1]);
                    if (pointsMoved)
                    {
                        _pathfinding._endX = endingNode[0];
                        _pathfinding._endY = endingNode[1];
                    }
                }
                else
                {
                    _pathfinding._startX = startingNode[0];
                    _pathfinding._startY = startingNode[1];
                }

                if (pointsMoved)
                {
                    StopSearching();
                }
            }

            if (currentSearch == null)
            {
                // Build simple grid
                BuildSimpleArea();

                // Build obstacles in grid
                BuildObstacles();

                // Place start and end of path
                PlaceStartAndEnd();

                if (!_searching)
                {
                    // PathFinding
                    currentSearch = StartCoroutine(FindPath());
                }
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
                            Gizmos.color = Color.cyan;
                            break;
                        case L_NodeStates.END:
                            Gizmos.color = Color.cyan;
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
