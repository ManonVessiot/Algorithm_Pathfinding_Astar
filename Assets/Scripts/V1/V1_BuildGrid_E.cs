using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(V1_BuildGrid))]
public class V1_BuildGrid_E : Editor
{
    public override void OnInspectorGUI()
    {
        V1_BuildGrid grid = (V1_BuildGrid)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Build Grid With Physics"))
        {
            grid.BuildGridWithPhysics();
        }
    }
}
