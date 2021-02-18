using UnityEngine;

namespace WaifuDriver
{
    public class RoadGraphGenerator
    {
        private static Vector2Int[] _dirs = new Vector2Int[] { 
            Vector2Int.left, 
            Vector2Int.right, 
            Vector2Int.up, 
            Vector2Int.down
        };

        private World _world;

        private float _roadSeparation;

        public RoadGraphGenerator(World world, float roadSeparation)
        {
            this._world = world;
            this._roadSeparation = roadSeparation;
        }

        public RoadGraph Generate()
        {
            var graph = new RoadGraph(this._world);

            for (int x = 0; x < this._world.size.x; x++) {
                for (int y = 0; y < this._world.size.y; y++) {
                    Vector2Int coord = new Vector2Int(x, y);
                    if (! this._world.HasRoad(coord)) continue;

                    if (this._world.HasIntersection(coord)) {
                        foreach (var dir in _dirs) {
                            this._CreateIntersectionEnter(graph, coord, dir);
                        }
                        foreach (var dir in _dirs) {
                            this._CreateIntersectionsLeave(graph, coord, dir);
                        }
                        foreach (var dir in _dirs) {
                            this._LinkIntersectionsEnterAndLeave(graph, coord, dir);
                        }
                    }
                }
            }

            foreach (var intersection in graph.intersectionsLeave) {
                foreach (var dir in _dirs) {
                    this._LinkIntersectionLeaveAndEnter(graph, intersection);
                }
            }

            return graph;
        }

        private void _CreateIntersectionEnter(RoadGraph graph, Vector2Int coord, Vector2Int searchDir)
        {
            if (graph.world.HasRoad(coord - searchDir)) {
                var offset = this._GetOffset(searchDir, -45f - 45 / 2f);
                graph.AddIntersectionEnter(coord, searchDir, coord + offset);
            }
        }

        private void _CreateIntersectionsLeave(RoadGraph graph, Vector2Int coord, Vector2Int searchDir)
        {
            if (graph.world.HasRoad(coord + searchDir)) {
                var offset = this._GetOffset(searchDir, 45 + 45f / 2f);
                graph.AddIntersectionLeave(coord, searchDir, coord + offset);
            }
        }

        private void _LinkIntersectionLeaveAndEnter(RoadGraph graph, Intersection intersectionLeave)
        {
            var searchDir = intersectionLeave.dir;
            var intersectionEnter = graph.FindNextIntersectionEnter(intersectionLeave.coord + searchDir, searchDir);
            if (intersectionEnter == null) return;
            intersectionLeave.AddRoadTo(intersectionEnter);
        }

        private void _LinkIntersectionsEnterAndLeave(RoadGraph graph, Vector2Int coord, Vector2Int enterDir)
        {
            var intersectionEnter = graph.FindIntersectionEnter(coord, enterDir);
            if (intersectionEnter == null) return;

            foreach (var dir in _dirs) {
                var intersectionLeave = graph.FindIntersectionLeave(coord, dir);
                if (intersectionLeave == null) continue;
                if (intersectionLeave.dir == -enterDir && ! this._HasMandatoryUTurn(graph.world, coord)) continue;

                intersectionEnter.AddRoadTo(intersectionLeave);
            }            
        }

        private Vector2 _GetOffset(Vector2 dir, float extraAngle)
        {
            var angle = (Vector2.SignedAngle(Vector2.up, dir) + extraAngle) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * this._roadSeparation;
        }

        private bool _HasMandatoryUTurn(World world, Vector2Int coord)
        {
            var connection = world.GetRoadConnectionAt(coord);
            return (connection == RoadConnection.Top 
                || connection == RoadConnection.Bottom
                || connection == RoadConnection.Left 
                || connection == RoadConnection.Right 
            );
        }
    }
}