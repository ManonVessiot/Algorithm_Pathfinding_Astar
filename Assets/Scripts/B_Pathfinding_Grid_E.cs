using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(B_Pathfinding_Grid))]
public class B_Pathfinding_Grid_E : Editor
{
    float _last_timeBetweenLoop = -2;
    float _last_epsilon = -2;

    public override void OnInspectorGUI()
    {
        B_Pathfinding_Grid pathFinding_grid = (B_Pathfinding_Grid)target;

        if (DrawDefaultInspector())
        {
            //if (((_last_timeBetweenLoop < -1) || (Equal(_last_timeBetweenLoop, pathFinding_grid._timeBetweenLoop))) && ((_last_epsilon < -1) || (Equal(_last_epsilon, pathFinding_grid._epsilon))))
            {
                _last_timeBetweenLoop = pathFinding_grid._timeBetweenLoop;
                _last_epsilon = pathFinding_grid._epsilon;
                pathFinding_grid.StopSearching();
            }
        }
    }

    bool Equal(float float1, float float2)
    {
        return Mathf.Abs(float1 - float2) < 0.001f;
    }


}
