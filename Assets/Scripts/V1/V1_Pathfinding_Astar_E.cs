using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(V1_Pathfinding_Astar))]
public class V1_Pathfinding_Astar_E : Editor
{
    public override void OnInspectorGUI()
    {
        V1_Pathfinding_Astar pathfinding = (V1_Pathfinding_Astar)target;

        DrawDefaultInspector();

        if (GUILayout.Button("FindPath"))
        {
            pathfinding.StartAstar();
        }
    }
}
