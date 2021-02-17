using System.Collections.Generic;
using UnityEngine;
using static WaifuDriver.World;

namespace WaifuDriver
{
    public class RoadGraph
    {
        private World _world;
        
        private Dictionary<Vector2Int, Intersection> _intersections = new Dictionary<Vector2Int, Intersection>();

        public RoadGraph(World world)
        {
            this._world = world;

            for (int x = 0; x < this._world.size.x; x++) {
                for (int y = 0; y < this._world.size.y; y++) {
                    Vector2Int coord = new Vector2Int(x, y);
                    if (! this._world.HasRoad(coord)) continue;

                    if (this._world.HasIntersection(coord)) {
                        var intersection = new Intersection(coord);
                        this._intersections.Add(coord, intersection);

                        this._AddRoad(intersection, Vector2Int.left);
                        this._AddRoad(intersection, Vector2Int.right);
                        this._AddRoad(intersection, Vector2Int.up);
                        this._AddRoad(intersection, Vector2Int.down);
                    }
                }
            }
        }

        public Intersection FindIntersection(Vector2Int coord)
        {
            this._intersections.TryGetValue(coord, out Intersection intersection);
            return intersection;
        }

        private void _AddRoad(Intersection startIntersection, Vector2Int searchDir)
        {
            Vector2Int coord = startIntersection.coord;
            while (this._world.HasRoad(coord)) {
                coord += searchDir;
                var endIntersection = this.FindIntersection(coord);
                if (endIntersection != null) {
                    startIntersection.AddRoadTo(endIntersection);
                    endIntersection.AddRoadTo(startIntersection);
                    return;
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (this._intersections == null) return;
            
            foreach (var intersection in this._intersections.Values) {
                var size = 0.4f;
                var pos = new Vector3(intersection.coord.x, intersection.coord.y, 0f);
                Gizmos.color = Color.red;
                Gizmos.DrawCube(pos, size * Vector3.one);

                foreach (var road in intersection.roads) {
                    Gizmos.color = Color.blue;
                    var start = new Vector3(road.start.coord.x, road.start.coord.y, 0f);
                    var end = new Vector3(road.end.coord.x, road.end.coord.y, 0f);

                    var goalDir = (end - start).normalized;
                    var offset  = new Vector3(goalDir.y, goalDir.x, 0f) * 0.18f;

                    Gizmos.DrawLine(start, end + offset);
                }
            }
        }
    }
}