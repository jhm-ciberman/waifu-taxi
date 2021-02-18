using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Pathfinder : INavigator<Intersection>
    {
        private RoadGraph _roadGraph;

        private World _world;

        private Vector2Int _startCoord;

        private Vector2Int _endCoord;

        private Vector2Int _startDir;

        private AStarPathfinder<Intersection> _pathfinder;

        public Pathfinder(RoadGraph roadGraph)
        {
            this._roadGraph = roadGraph;
            this._world = roadGraph.world;
            this._pathfinder = new AStarPathfinder<Intersection>(this);
        }

        public Path Pathfind(Vector2Int start, Vector2Int end, Vector2Int startingDir, float roadSeparation)
        {
            this._startDir = startingDir;
            this._startCoord = start;
            this._endCoord = end;
            var cameFrom = start - startingDir;      

            var nodeStart = this._roadGraph.CreateStart(start, startingDir);
            if (nodeStart == null) {
                Debug.LogWarning($"Cannot create starting node at {start} (startingDir = {startingDir})");
                return null;
            }

            var nodeEnd = this._roadGraph.CreateEnd(end);
            if (nodeEnd == null) {
                Debug.LogWarning($"Cannot create ending node at {end}");
                return null;
            }

            var points = this._pathfinder.Pathfind(nodeStart, nodeEnd);

            this._roadGraph.RemoveTemporaryRoads();

            if (points == null || points.Count == 0) {
                return null; //Invalid goal!!
            }

            return new Path(points, roadSeparation);
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
            } while (end == startCoord && Vector2.Distance(startCoord, end) < minDistance);
            return end;
        }

        float INavigator<Intersection>.HeuristicDistance(Intersection start, Intersection end)
        {
            return Vector2.Distance(start.position, end.position);
        }

        float INavigator<Intersection>.WeightFunction(Intersection from, Intersection to, Intersection cameFrom)
        {
            return Vector2.Distance(from.position, to.position);
        }

        /*
        // Penalize non mandatory U turns
        var currentDir = (to.start.coord - from.);
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
        */

        void INavigator<Intersection>.VisitNodeNeighbours(INodeVisitor<Intersection> nodeVisitor, Intersection intersection)
        {
            var roads = intersection.roads;
            for (int i = 0; i < roads.Count; i++) {
                nodeVisitor.VisitNode(roads[i].end);
            }
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