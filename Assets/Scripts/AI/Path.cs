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

        public Path(List<Vector2Int> points, float roadSeparation)
        {
            this._points = this._MakePathPoints(points, roadSeparation);
            this.length = this._points[this._points.Length - 1].length;
        }

        private PathPoint[] _MakePathPoints(List<Vector2Int> points, float roadSeparation)
        {
            var list = new List<PathPoint>();

            Vector2 prevPoint;
            Vector2 currentPoint = points.FirstOrDefault();
            list.Add(new PathPoint(currentPoint, 0f));
            float length = 0f;
            for (int i = 1; i < points.Count; i++) {
                prevPoint = currentPoint;
                currentPoint = points[i];
                var goalVec = (currentPoint - prevPoint);
                var goalDir = goalVec.normalized;
                var offsetWide   = new Vector2(goalDir.y, goalDir.x) * roadSeparation;
                var offsetLength = new Vector2(-goalDir.x, -goalDir.y) * roadSeparation;
                var position = currentPoint + offsetWide + offsetLength;
                length += goalVec.magnitude;
                list.Add(new PathPoint(position, length));
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
            // Adapted from: https://bl.ocks.org/mbostock/8027637

            var pathLength = this.length;

            // linear scan for coarse approximation
            Vector2 best;
            float bestLength = 0f;
            float bestDist = float.PositiveInfinity;
            float precision = 1f;
            for (float length = 0; length <= pathLength; length += precision) {
                Vector2 scan = this.GetPosition(length);
                float dist = (searchPoint - scan).sqrMagnitude;
                if (dist < bestDist) {
                    best = scan;
                    bestLength = length;
                    bestDist = dist;
                }
            }
            // binary search for precise estimate
            float currentPrecision = 1f;

            Vector2 bestPoint = this.GetPosition(bestLength);
            float bestDistance = (bestPoint - searchPoint).sqrMagnitude;

            {
                float length;
                float dist;
                Vector2 point;
                float desiredPrecision = 0.025f;
                while (currentPrecision > desiredPrecision) {

                    length = bestLength - currentPrecision;
                    if (length >= 0f) {
                        point = this.GetPosition(length);
                        dist = (point - searchPoint).sqrMagnitude;
                        if (dist < bestDistance) {
                            bestPoint = point;
                            bestLength = length;
                            bestDistance = dist;
                            continue;
                        } 
                    }

                    length = bestLength + currentPrecision;
                    if (length <= this.length) {
                        point = this.GetPosition(length);
                        dist = (point - searchPoint).sqrMagnitude;
                        if (dist < bestDistance) {
                            bestPoint = point;
                            bestLength = length;
                            bestDistance = dist;
                            continue;
                        }
                    }

                    currentPrecision /= 2;
                }
            }

            return new PathPoint(bestPoint, bestLength);
        }

    }
}