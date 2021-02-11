using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public static class PathRetracer
    {
        
        public static List<Vector2> Retrace(IReadOnlyList<Vector2Int> path, float roadSeparation)
        {
            List<Vector2> list = new List<Vector2>();

            Vector2Int prevPoint;
            Vector2Int currentPoint = path[0];

            for (int i = 1; i < path.Count; i++) {
                prevPoint = currentPoint;
                currentPoint = path[i];
                var goalDir = (currentPoint - prevPoint);
                var offsetWide   = new Vector2(goalDir.y, goalDir.x) * roadSeparation;
                var offsetLenght = new Vector2(-goalDir.x, -goalDir.y) * roadSeparation;
                list.Add(currentPoint + offsetWide + offsetLenght);
            }

            return list;
        }


        public static List<Vector2Int> Simplify(List<Vector2Int> path)
        {
            List<Vector2Int> list = new List<Vector2Int>();

            Vector2Int prevPoint;
            Vector2Int currentPoint = path[0];
            Vector2Int currentGoalDir = Vector2Int.zero;

            for (int i = 1; i < path.Count; i++) {
                prevPoint = currentPoint;
                currentPoint = path[i];
                var goalDir = (currentPoint - prevPoint);
                if (currentGoalDir != goalDir) {
                    currentGoalDir = goalDir;
                    list.Add(currentPoint);
                }
            }

            return list;
        }
    }

}
