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

        public static void DrawRoadGraph(RoadGraph graph)
        {            
            foreach (var intersection in graph.intersections) {
                var size = 0.1f;
                var pos = new Vector3(intersection.position.x, intersection.position.y, 0f);
                Gizmos.color = RouteGizmo._GetColor(intersection.dir);
                if (intersection.type == Intersection.Type.Enter) {
                    Gizmos.DrawCube(pos, size * Vector3.one);
                } else {
                    Gizmos.DrawSphere(pos, size / 2f);
                }
            }

            foreach (var intersection in graph.intersections) {
                foreach (var road in intersection.roads) {
                    Gizmos.color = Color.white;
                    var start = new Vector3(road.start.position.x, road.start.position.y, -.5f);
                    var end = new Vector3(road.end.position.x, road.end.position.y, -.5f);
                    Gizmos.DrawLine(start, end);
                }
            }
        }

        private static Color _GetColor(Vector2Int dir)
        {
            if (dir == Vector2Int.up)    return Color.blue;
            if (dir == Vector2Int.down)  return Color.yellow;
            if (dir == Vector2Int.left)  return Color.green;
            if (dir == Vector2Int.right) return Color.magenta;

            return Color.black;
        }


    }
}