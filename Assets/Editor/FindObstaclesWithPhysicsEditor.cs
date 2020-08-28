using UnityEngine;
using UnityEditor;

namespace Pathfinding_Astar
{
    [CustomEditor(typeof(FindObstaclesWithPhysics), true)]
    public class FindObstaclesWithPhysicsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FindObstaclesWithPhysics finder = (FindObstaclesWithPhysics)target;

            DrawDefaultInspector();
            if (GUILayout.Button("Find obstacles"))
            {
                finder.FindObstacles();
            }
        }
    }
}