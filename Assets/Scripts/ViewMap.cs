using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding_Astar
{
    public class ViewMap : MonoBehaviour
    {
        public MapToBuild _viewMap;
        public Pathfinding_Astar _pathfinding;
        public Color _colorOfWalkableCells = Color.white;
        public Color _colorOfNotWalkableCells = Color.black;
        public Color _colorOfStartingCells = Color.magenta;
        public Color _colorOfEndingCells = Color.cyan;
        public Color _colorOfPathCells = Color.blue;
        public Color _colorOfOpenedCells = Color.green;
        public Color _colorOfClosedCells = Color.red;

        public Vector2 _epsilonBetweenCells = 0.1f * Vector2.one;
        public float _secondBetweenStages = 1f;

        [System.NonSerialized]
        public bool _solving = false;
        [System.NonSerialized]
        public bool _found = false;

        [System.NonSerialized]
        public bool _whenInPlayMode = true;
        [System.NonSerialized]
        public bool _whenInEditMode = true;

        public Vector2 _cellSize;

        private void Start()
        {
            Reset();
        }

        public void StartSolving()
        {
            StartCoroutine(Solve());
        }
        IEnumerator Solve()
        {
            _solving = true;
            while (_solving)
            {
                yield return new WaitForSeconds(_secondBetweenStages);
                _solving = !EvolveGameNextStage();
            }
        }
        public bool EvolveGameNextStage()
        {
            if (!_found && _pathfinding != null)
            {
                _found = _pathfinding.NextStep();
                Debug.Log("stage : " + _pathfinding._loop);
                UpdateView();
            }
            return _found;
        }


        public void StopSolving()
        {
            StopAllCoroutines();
            _solving = false;
        }

        public virtual void Reset()
        {
            ResetMap();
        }
        protected void ResetMap()
        {
            _found = false;

            _viewMap._start._x += _viewMap._cellShift._x;
            _viewMap._start._y += _viewMap._cellShift._y;

            _viewMap._end._x += _viewMap._cellShift._x;
            _viewMap._end._y += _viewMap._cellShift._y;
            foreach (MapPosition cell in _viewMap._notWalkableCells)
            {
                cell._x += _viewMap._cellShift._x;
                cell._y += _viewMap._cellShift._y;
            }
            _viewMap._cellShift._x = 0;
            _viewMap._cellShift._y = 0;

            Map map = new Map(_viewMap._width, _viewMap._height, _viewMap._notWalkableCells);
            _pathfinding = new Pathfinding_Astar(map, _viewMap._start, _viewMap._end);
            _solving = false;
            UpdateView();
        }

        protected virtual void UpdateView()
        {
            UnityEditor.SceneView.RepaintAll();
        }

        public virtual Vector3 GetWorldPostionOfNode(int w, int h)
        {
            return new Vector3(w, h, 0);
        }
    }
}