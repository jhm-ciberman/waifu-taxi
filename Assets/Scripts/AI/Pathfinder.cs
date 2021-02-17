using System;
using UnityEngine;

namespace WaifuDriver
{
    public class Pathfinder : INavigator<Vector2Int>
    {
        private World _world;

        private Vector2Int _startCoord;

        private Vector2Int _endCoord;

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
            this._endCoord = end;
            var cameFrom = start - startingDir;
            var points = this._pathfinder.Pathfind(start, end);
            if (points.Count > 0) {
                return new Path(points, roadSeparation);
            }
            return null; //Invalid goal!!
        }

        public Vector2Int RandomDestination(Vector2Int startCoord)
        {
            float minDistance = (this._world.size.x + this._world.size.y) * 0.3f;
            Vector2Int end;
            int tries = 0;
            do {
                end = this._world.RandomRoad();
                tries++;
                if (tries > 1000) return end;
            } while (end == startCoord && ManhatanDistance(startCoord, end) < minDistance);
            return end;
        }

        private static float ManhatanDistance(Vector2Int start, Vector2Int end)
        {
            var d = (end - start);
            return Math.Abs(d.x) + Math.Abs(d.y); // Manhatan distance
        }

        float INavigator<Vector2Int>.HeuristicDistance(Vector2Int start, Vector2Int end)
        {
            return ManhatanDistance(start, end);
        }

        float INavigator<Vector2Int>.WeightFunction(Vector2Int fromCoord, Vector2Int toCoord, Vector2Int cameFromCoord)
        {
            // Penalize non mandatory U turns
            var currentDir = (toCoord - fromCoord);
            currentDir.Clamp(-Vector2Int.one, Vector2Int.one);

            var previousDir = fromCoord - cameFromCoord;
            previousDir.Clamp(-Vector2Int.one, Vector2Int.one);

            if (previousDir == -currentDir) {
                if (! this._HasMandatoryUTurn(fromCoord)) {
                    Debug.Log("PENALIZED: " + fromCoord + " to " + toCoord);
                    return 1000f;
                } else {
                    Debug.Log("MANDATORY U TURN: " + fromCoord + " to " + toCoord);
                }
            }

            return ManhatanDistance(fromCoord, toCoord);
        }

        private bool _HasMandatoryUTurn(Vector2Int coord)
        {
            var connection = this._world.GetRoadConnectionAt(coord);
            return (connection == RoadConnection.Top 
                || connection == RoadConnection.Bottom
                || connection == RoadConnection.Left 
                || connection == RoadConnection.Right 
            );
        }

        void INavigator<Vector2Int>.VisitNodeNeighbours(INodeVisitor<Vector2Int> nodeVisitor, Vector2Int coord)
        {
            this._VisitNextIntersection(nodeVisitor, coord, Vector2Int.left);
            this._VisitNextIntersection(nodeVisitor, coord, Vector2Int.right);
            this._VisitNextIntersection(nodeVisitor, coord, Vector2Int.up);
            this._VisitNextIntersection(nodeVisitor, coord, Vector2Int.down);
        }

        private void _VisitNextIntersection(INodeVisitor<Vector2Int> nodeVisitor, Vector2Int startCoord, Vector2Int searchDir)
        {
            Vector2Int coord = startCoord + searchDir;
            while (this._world.HasRoad(coord)) {
                if (this._world.HasIntersection(coord) || coord == this._endCoord) {
                    nodeVisitor.VisitNode(coord);
                    return;
                }
                coord += searchDir;
            }
        }
    }
}