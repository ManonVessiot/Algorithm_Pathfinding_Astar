using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Grid _gridScript;
    public GridDraw _gridDrawScript;
    public Pathfinding_Astar _pathfinding;

    public void BuildGridWithPhysics()
    {
        _gridScript._grid = new Node[_gridScript._xSize, _gridScript._ySize];
        for (int x = 0; x < _gridScript._xSize; x++)
        {
            for (int y = 0; y < _gridScript._ySize; y++)
            {
                bool walkable = !Physics.CheckBox(_gridScript.GetWorldPostionOfNode(x, y), new Vector3(_gridScript._nodeSize / 2f, _gridScript._nodeSize / 2f, _gridScript._playerSize.z * 2), Quaternion.identity, unwalkableMask);
                _gridScript._grid[x, y] = new Node(walkable, x, y);
            }
        }
        _gridDrawScript.DrawGrid(_gridScript);

        _pathfinding.FindStartAndEndNode();

        _gridDrawScript.SetPointNodes(_pathfinding._startingNode);
        _gridDrawScript.SetPointNodes(_pathfinding._endingNode);
    }
}
