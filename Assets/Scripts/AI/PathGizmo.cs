using UnityEngine;

namespace WaifuDriver
{
    public static class PathGizmo
    {
        public static void DrawPath(Path path)
        {
            float radius = 1f / 8f;
            Vector2 first = path.currentPosition;
            Vector3 prev;
            Vector3 current = new Vector3(first.x, first.y, 0.2f);
            foreach (var p in path.remainingPoints) {
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