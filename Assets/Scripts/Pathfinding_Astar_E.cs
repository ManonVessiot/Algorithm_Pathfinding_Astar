using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathfinding_Astar))]
public class Pathfinding_Astar_E : Editor
{
    public override void OnInspectorGUI()
    {
        Pathfinding_Astar pathfinding = (Pathfinding_Astar)target;

        DrawDefaultInspector();

        if (GUILayout.Button("FindPath"))
        {
            pathfinding.StartAstar();
        }
    }
}
