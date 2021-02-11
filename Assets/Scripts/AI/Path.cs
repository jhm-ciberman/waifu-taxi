using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WaifuDriver
{
    public class Path
    {
        public static void DrawPathGizmos(IEnumerable<Vector2Int> path)
        {
            if (! path.Any()) return;

            float radius = 1f / 8f;
            Vector2 first = path.First();
            Vector3 prev;
            Vector3 current = new Vector3(first.x, first.y, 0.2f);
            foreach (var p in path) {
                prev = current;
                current = new Vector3(p.x, p.y, 0.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(prev, current);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(current, radius);
            }
        }
    }
}