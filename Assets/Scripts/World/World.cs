using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi.World
{
    public class World
    {
        private Dictionary<Vector2Int, bool> _road;

        public Vector2Int size {get ; private set;}

        public World(Vector2Int size)
        {
            this.size = size;
            this._road = new Dictionary<Vector2Int, bool>();

            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    this._road[new Vector2Int(x, y)] = false;
                }
            }

            for (int x = 1; x < size.x - 1; x++) {
                for (int y = 1; y < size.y - 1; y++) {
                    if (x % 3 == 0 || y % 3 == 0) {
                        this._road[new Vector2Int(x, y)] = true;
                    }
                }
            }
        }

        public bool HasRoad(Vector2Int pos)
        {
            this._road.TryGetValue(pos, out bool hasRoad);
            return hasRoad;
        }

        public RoadConnection GetRoadConnectionAt(Vector2Int pos)
        {
            if (! this.HasRoad(pos)) return RoadConnection.None;

            RoadConnection connection = RoadConnection.None;
            if (this.HasRoad(pos + new Vector2Int(-1, 0))) connection |= RoadConnection.Left;
            if (this.HasRoad(pos + new Vector2Int(+1, 0))) connection |= RoadConnection.Right;
            if (this.HasRoad(pos + new Vector2Int(0, -1))) connection |= RoadConnection.Top;
            if (this.HasRoad(pos + new Vector2Int(0, +1))) connection |= RoadConnection.Bottom;
            return connection;
        }
    }
}