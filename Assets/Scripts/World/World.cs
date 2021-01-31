using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class World
    {
        private Dictionary<Vector2Int, bool> _road;

        public Vector2Int size {get ; private set;}

        private System.Random _random = new System.Random();

        private int[,] _city = new int[,] {
           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           { 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
           { 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0 },
           { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
           { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0 },
           { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 },
           { 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
           { 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0 },
           { 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 },
           { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
           { 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0 },
           { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0 },
           { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        };

        public int roadCount = 0;

        public World(Vector2Int size)
        {
            this.size = size;
            this._road = new Dictionary<Vector2Int, bool>();

            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    this._road[new Vector2Int(x, y)] = false;
                }
            }

            for (int x = 1; x < this.size.x - 1; x++) {
                for (int y = 1; y < this.size.y - 1; y++) {
                    if (x % 3 == 0 || y % 3 == 0) {
                        this._road[new Vector2Int(x, y)] = true;
                    }
                }
            }

            //for (int x = 0; x < this._city.GetLength(0); x++) {
            //    for (int y = 0; y < this._city.GetLength(1); y++) {
            //        this._road[new Vector2Int(x, y)] = (this._city[x, y] == 1);
            //    }
            //}

            //this.size = new Vector2Int(this._city.GetLength(0), this._city.GetLength(1));


            foreach (var hasRoad in this._road.Values) {
                if (hasRoad) this.roadCount++;
            }
        }

        public bool HasRoad(Vector2Int pos)
        {
            this._road.TryGetValue(pos, out bool hasRoad);
            return hasRoad;
        }

        public Vector2Int RandomRoad()
        {
            int tries = 0;
            while (tries < 1000) 
            {
                int x = this._random.Next(1, this.size.x - 1);
                int y = this._random.Next(1, this.size.y - 1);
                var pos = new Vector2Int(x, y);
                if (this.HasRoad(pos)) {
                    return pos;
                }

                tries++;
            }
            return new Vector2Int(0, 0); // Error! 
        }

        public Vector2Int RandomDestination(Vector2Int startCoord)
        {
            Vector2Int end;
            int tries = 0;
            do {
                end = this.RandomRoad();
                tries++;
                if (tries > 1000) return end;
            } while (end == startCoord);
            return end;
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