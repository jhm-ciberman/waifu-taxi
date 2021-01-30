using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class RoutePlanner
    {
        private World _world;

        public RoutePlanner(World world)
        {
            this._world = world;
        }


        public Queue<Vector2Int> CalculatePath(Vector2 pos, float currentAngle)
        {
            var start = this._PositionToTileCoord(pos);
            var end = this._world.RandomRoad();
            var startingDir = this._GetDirVector(currentAngle);
            CarPathfinder pathfinder = new CarPathfinder(this._world, start, end, startingDir);
            var path = pathfinder.Pathfind();

            if (path == null) {
                Debug.Log("PATH NOT FOUND start=" + start + " end = " + end);
                return null;
            } else {
                //Debug.Log("PATH FOUND start=" + start + " end =" + end);
                //foreach (var p in path) Debug.Log(p);

                var queue = new Queue<Vector2Int>(path);
                return queue;
            }
        }

        public Vector2Int _GetDirVector(float angle)
        {
            angle += 90f;
            angle %= 360f;
            int dir = Mathf.CeilToInt((angle - 45f) / 90f);

            switch (dir) {
                case 0: return new Vector2Int(1, 0);
                case 1: return new Vector2Int(0, 1);
                case 2: return new Vector2Int(-1, 0);
                case 3: return new Vector2Int(0, -1);
            }
            return Vector2Int.zero; // error!
        }

        private Vector2Int _PositionToTileCoord(Vector2 pos)
        {
            int x = Mathf.RoundToInt(pos.x);
            int y = Mathf.RoundToInt(pos.y);
            return new Vector2Int(x, y);
        }

    }
}