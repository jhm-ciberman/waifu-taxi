using System;
using UnityEngine;

namespace WaifuDriver
{
    public class CarPathfinder : Pathfinder
    {
        private World _world;

        private Vector2Int _startCoord;

        private Vector2Int _startDir;

        public CarPathfinder(World world, Vector2Int start, Vector2Int end, Vector2Int startingDir) : base(world.size, start, end)
        {
            this._world = world;
            this._startCoord = start;
            this._startDir = startingDir;
        }

        protected override float _HeuristicDistance(Vector2Int start, Vector2Int end)
        {
            var d = (end - start);
            return Math.Abs(d.x) + Math.Abs(d.y); // Manhatan distance
        }

        protected override float _WeightFunction(Vector2Int fromCoord, Vector2Int toCoord, Vector2Int cameFromCoord)
        {
            if (! this._world.HasRoad(toCoord)) {
                return float.PositiveInfinity;
            }

            // Detect U turns at the start of the path
            if (fromCoord == this._startCoord) {
                var dir = toCoord - fromCoord;
                if (this._startDir == -dir) {
                    return 10000f;
                }
            }

            return 1f;
        }
    }
}