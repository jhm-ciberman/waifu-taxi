using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Pathfinder : AStarPathfinder.INavigator
    {
        private World _world;

        private Vector2Int _startCoord;

        private Vector2Int _startDir;

        private AStarPathfinder _pathfinder;

        public Pathfinder(World world)
        {
            this._world = world;
            this._pathfinder = new AStarPathfinder(this);
        }

        public IReadOnlyList<Vector2Int> Pathfind(Vector2Int start, Vector2Int end, Vector2Int startingDir)
        {
            this._startDir = startingDir;
            return this._pathfinder.Pathfind(start, end);
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

        float AStarPathfinder.INavigator.HeuristicDistance(Vector2Int start, Vector2Int end)
        {
            var d = (end - start);
            return Math.Abs(d.x) + Math.Abs(d.y); // Manhatan distance
        }

        float AStarPathfinder.INavigator.WeightFunction(Vector2Int fromCoord, Vector2Int toCoord, Vector2Int cameFromCoord)
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
    }
}