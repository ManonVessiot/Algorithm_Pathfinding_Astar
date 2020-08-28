using UnityEngine;
using UnityEditor;

namespace Pathfinding_Astar
{
    [CustomEditor(typeof(ViewMap), true)]
    public class ViewMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ViewMap viewMap = (ViewMap)target;

            GUI.enabled = (!Application.isPlaying || !viewMap._solving);
            if (DrawDefaultInspector() && Application.isPlaying)
            {
                viewMap.Reset();
            }
            GUI.enabled = true;

            if (Application.isPlaying)
            {
                GUI.enabled = viewMap._whenInPlayMode;
                if (viewMap._solving)
                {
                    if (GUILayout.Button("Stop solving"))
                    {
                        viewMap.StopSolving();
                    }
                }
                else
                {
                    if (GUILayout.Button("Start solving"))
                    {
                        viewMap.StartSolving();
                    }
                }

                if (GUILayout.Button("Reset"))
                {
                    if (viewMap._solving)
                    {
                        viewMap.StopSolving();
                    }
                    viewMap.Reset();
                }
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = viewMap._whenInEditMode;
                if (GUILayout.Button("Next Stage"))
                {
                    viewMap.EvolveGameNextStage();
                }
                if (GUILayout.Button("Reset"))
                {
                    viewMap.Reset();
                }
                GUI.enabled = true;
            }
        }
    }
}
