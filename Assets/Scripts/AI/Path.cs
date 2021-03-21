using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace WaifuDriver
{
    public class Path
    {
        private PathPoint[] _points;

        public readonly float length = 0;

        public Path(List<Intersection> points)
        {
            this._points = this._MakePathPoints(points);
            this.length = this._points[this._points.Length - 1].length;
        }

        private PathPoint[] _MakePathPoints(List<Intersection> points)
        {
            var list = new List<PathPoint>();

            Vector2 prevPoint;
            Vector2 currentPoint = points.First().position;
            float length = 0f;
            for (int i = 0; i < points.Count; i++) {
                prevPoint = currentPoint;
                currentPoint = points[i].position;
                length += Vector2.Distance(prevPoint, currentPoint);
                list.Add(new PathPoint(currentPoint, length));
            }

            return list.ToArray();
        }

        public int pointsCount => this._points.Length;

        public Vector2 GetPositionAtIndex(int index)
        {
            return this._points[index].position;
        }

        public Vector2 GetPosition(float length)
        {
            (var prevIndex, var nextIndex) = this.GetPointsIndicesBetween(length);
            PathPoint prev = this._points[prevIndex];
            PathPoint next = this._points[nextIndex];
            float t = _InverseLerp(prev.length, next.length, length);
            return Vector2.LerpUnclamped(prev.position, next.position, t);
        }

        private static float _InverseLerp(float a, float b, float value) 
        {
            // Faster than Mathf.InverseLerp because it does not clamp the value
            return (a != b) ? (value - a) / (b - a) : 0f;
        }

        public (int prev, int next) GetPointsIndicesBetween(float length)
        {
            if (length <= 0f) {
                return (0, 0);
            }

            if (length >= this.length) {
                int index = this._points.Length - 1;
                return (index, index);
            }

            // Linear search :( 
            // O(n) is not good! 
            // But I think for n < 10 points it's faster than a bin search

            int i = 0;
            while (i < this._points.Length) {
                if (this._points[i++].length >= length) break;
            }
            i--;
            int prevIndex = (i > 0) ? i - 1 : i;
            return (prevIndex, i);
        }

        public IEnumerable<Vector2> points
        {
            get
            {
                for (int i = 0; i < this._points.Length; i++) {
                    yield return this._points[i].position;
                }
            }
        }

        public PathPoint ClosestPoint(Vector2 searchPoint)
        {
            return this.ClosestPoint(searchPoint, 0f, this.length, 0.05f);
        }

        public PathPoint ClosestPoint(Vector2 searchPoint, float startSearchLength, float maxSearchLength, float precision = 0.05f) 
        {
            // linear search

            var maxLength = Math.Min(this.length, startSearchLength + maxSearchLength);

            float bestLength = startSearchLength;
            Vector2 bestPoint = this.GetPosition(bestLength);
            float bestDist = (searchPoint - bestPoint).sqrMagnitude;
            
            for (float length = startSearchLength + precision; length <= maxLength; length += precision) {
                Vector2 scan = this.GetPosition(length);
                float dist = (searchPoint - scan).sqrMagnitude;
                if (dist < bestDist) {
                    bestPoint = scan;
                    bestLength = length;
                    bestDist = dist;
                }
            }
            return new PathPoint(bestPoint, bestLength);
        }

    }
}