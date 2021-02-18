using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class World
    {

        public enum TileType
        {
            Building = 0,
            Plaza,
            Road,
        }

        private Dictionary<Vector2Int, TileType> _road = new Dictionary<Vector2Int, TileType>();
        private Dictionary<Vector2Int, bool> _isIntersection = new Dictionary<Vector2Int, bool>();

        public Vector2Int size {get ; private set;}

        private System.Random _random = new System.Random();

        public int roadCount = 0;

        public World(City city)
        {
            this.size = size;

            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    this._road[new Vector2Int(x, y)] = 0;
                }
            }

            var plan = city.GetCity();
            this.size = new Vector2Int(plan.GetLength(0), plan.GetLength(1));

            for (int x = 0; x < this.size.x; x++) {
                for (int y = 0; y < this.size.y; y++) {
                    this._road[new Vector2Int(x, y)] = plan[x, y];
                }
            }

            for (int x = 0; x < this.size.x; x++) {
                for (int y = 0; y < this.size.y; y++) {
                    var coord = new Vector2Int(x, y);
                    var connection = this.GetRoadConnectionAt(coord); 
                    this._isIntersection[coord] = connection.HasFlag(RoadConnection.Top) != connection.HasFlag(RoadConnection.Bottom)
                        || connection.HasFlag(RoadConnection.Left) != connection.HasFlag(RoadConnection.Right)
                        || connection == RoadConnection.Cross;
                }
            }

            foreach (var type in this._road.Values) {
                if (type == TileType.Road) this.roadCount++;
            }
        }

        public bool IsInside(Vector2Int pos)
        {
            return (pos.x >= 0 && pos.y >= 0 && pos.x < this.size.x && pos.y < this.size.y);
        }

        public bool HasRoad(Vector2Int pos)
        {
            this._road.TryGetValue(pos, out TileType hasRoad);
            return hasRoad == TileType.Road;
        }

        public TileType GetTileType(Vector2Int pos)
        {
            this._road.TryGetValue(pos, out TileType type);
            return type;
        }

        public bool HasIntersection(Vector2Int pos)
        {
            this._isIntersection.TryGetValue(pos, out bool hasIntersection);
            return hasIntersection;
        }

        public Vector2Int RandomRoad()
        {
            int tries = 0;
            while (tries < 1000) 
            {
                int x = this._random.Next(1, this.size.x - 1);
                int y = this._random.Next(1, this.size.y - 1);
                var pos = new Vector2Int(x, y);
                if (this.HasRoad(pos) && ! this.HasIntersection(pos)) {
                    return pos;
                }

                tries++;
            }
            return new Vector2Int(0, 0); // Error! 
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