using System;
using UnityEngine;

namespace WaifuDriver
{
    public class Pathfinder : INavigator<Vector2Int>
    {
        private World _world;

        private Vector2Int _startCoord;

        private Vector2Int _startDir;

        private AStarPathfinder<Vector2Int> _pathfinder;

        public Pathfinder(World world)
        {
            this._world = world;
            this._pathfinder = new AStarPathfinder<Vector2Int>(this);
        }

        public Path Pathfind(Vector2Int start, Vector2Int end, Vector2Int startingDir, float roadSeparation)
        {
            this._startDir = startingDir;
            this._startCoord = start;
            var points = this._pathfinder.Pathfind(start, end);
            if (points.Count > 0) {
                return new Path(points, roadSeparation);
            }
            return null; //Invalid goal!!
        }

        public Vector2Int RandomDestination(Vector2Int startCoord)
        {
            Vector2Int end;
            int tries = 0;
            do {
                end = this._world.RandomRoad();
                tries++;
                if (tries > 1000) return end;
            } while (end == startCoord);
            return end;
        }

        float INavigator<Vector2Int>.HeuristicDistance(Vector2Int start, Vector2Int end)
        {
            var d = (end - start);
            return Math.Abs(d.x) + Math.Abs(d.y); // Manhatan distance
        }

        float INavigator<Vector2Int>.WeightFunction(Vector2Int fromCoord, Vector2Int toCoord, Vector2Int cameFromCoord)
        {
            if (! this._world.HasRoad(toCoord)) {
                return float.PositiveInfinity;
            }

            // Detect U turns at the start of the path
            if (fromCoord == this._startCoord) {
                var dir = toCoord - fromCoord;
                if (this._startDir == -dir) {
                    return 100f;
                }
            }

            return 1f;
        }

        void INavigator<Vector2Int>.VisitNodeNeighbours(INodeVisitor<Vector2Int> nodeVisitor, Vector2Int node)
        {
            nodeVisitor.VisitNode(new Vector2Int(node.x - 1, node.y));
            nodeVisitor.VisitNode(new Vector2Int(node.x + 1, node.y));
            nodeVisitor.VisitNode(new Vector2Int(node.x, node.y - 1));
            nodeVisitor.VisitNode(new Vector2Int(node.x, node.y + 1));
        }
    }
}