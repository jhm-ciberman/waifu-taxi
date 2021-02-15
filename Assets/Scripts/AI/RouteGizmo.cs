using System.Linq;
using UnityEngine;

namespace WaifuDriver
{
    public static class RouteGizmo
    {
        public static void DrawRoute(Path path)
        {
            float radius = 1f / 16f;
            Vector2 first = path.points.First();
            Vector3 prev;
            Vector3 current = new Vector3(first.x, first.y, 0.2f);
            foreach (var p in path.points) {
                prev = current;
                current = new Vector3(p.x, p.y, 0.2f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(prev, current);
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(current, radius);
            }
        }
    }
}