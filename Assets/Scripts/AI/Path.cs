using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaifuDriver
{
    public class Path
    {
        private Vector2[] _points;

        public readonly float length = 0;

        private int _currentPointIndex = 0;

        public Path(List<Vector2Int> points, float roadSeparation)
        {
            this._points = new Vector2[points.Count];

            Vector2Int prevPoint;
            Vector2Int currentPoint = points.FirstOrDefault();
            this._points[0] = currentPoint;

            for (int i = 1; i < points.Count; i++) {
                prevPoint = currentPoint;
                currentPoint = points[i];
                var goalDir = (currentPoint - prevPoint);
                var offsetWide   = new Vector2(goalDir.y, goalDir.x) * roadSeparation;
                var offsetLenght = new Vector2(-goalDir.x, -goalDir.y) * roadSeparation;
                this._points[i] = currentPoint + offsetWide + offsetLenght;
                this.length += goalDir.magnitude;
            }
        }

        public void Advance()
        {
            this._currentPointIndex++;
            if (this.targetReached) {
                this._currentPointIndex = this._points.Length;
            }
        }

        public int pointsCount => this._points.Length;

        public bool targetReached => (this._currentPointIndex >= this._points.Length);

        public Vector2 currentPosition => this._GetPosition(this._currentPointIndex);

        public Vector2Int currentTile => Path._ToTilePos(this.currentPosition);

        public Vector2 targetPosition => this._GetPosition(this._points.Length - 1);

        public Vector2Int targetTile => Path._ToTilePos(this.targetPosition);

        public Vector2 PeekPosition(int index = 0) => this._GetPosition(this._currentPointIndex + index);
        public Vector2Int PeekTile(int index = 0) => Path._ToTilePos(this._GetPosition(this._currentPointIndex + index));

        private Vector2 _GetPosition(int index)
        {
            if (index >= this._points.Length) {
                return this.targetPosition;
            }
            return this._points[index];
        }

        private static Vector2Int _ToTilePos(Vector2 p) => new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y));

        public IEnumerable<Vector2> remainingPoints
        {
            get
            {
                for (int i = this._currentPointIndex; i < this._points.Length; i++) {
                    yield return this._points[i];
                }
            }
        }
    }
}