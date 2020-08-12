using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(B_Pathfinding_Grid))]
public class B_Pathfinding_Grid_E : Editor
{

    public override void OnInspectorGUI()
    {
        B_Pathfinding_Grid pathFinding_grid = (B_Pathfinding_Grid)target;

        if (DrawDefaultInspector() || GUILayout.Button("Search"))
        {
            pathFinding_grid.StopSearching();
        }
    }


}
