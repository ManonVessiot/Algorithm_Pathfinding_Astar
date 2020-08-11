using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildGrid))]
public class BuildGrid_E : Editor
{
    public override void OnInspectorGUI()
    {
        BuildGrid grid = (BuildGrid)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Build Grid With Physics"))
        {
            grid.BuildGridWithPhysics();
        }
    }
}
