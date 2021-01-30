using System;
using UnityEngine;

namespace WaifuTaxi
{
    public class CarPathfinder : Pathfinder
    {
        private World _world;

        public CarPathfinder(World world, Vector2Int start, Vector2Int end) : base(world.size, start, end)
        {
            this._world = world;
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
            return 1f;
        }
    }
}