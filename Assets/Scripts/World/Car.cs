using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class Car : Entity
    {
        private Queue<Vector2Int> _path = null;
        
        private Vector2Int _currentPoint;

        public float speed = 0.5f;

        private World _world;

        public void SetWorld(World world)
        {
            this._world = world;
            this.StartNewRandomPath();
        }

        public void StartNewRandomPath()
        {
            if (this._world == null) return;

            var end = this._world.RandomRoad();
            CarPathfinder pathfinder = new CarPathfinder(this._world, this.currentCoord, end, this.currentDirVector);
            var path = pathfinder.Pathfind();
            if (path != null) {
                this.SetPath(path);
            }
        }

        public void Update()
        {
            if (this._path == null) return;

            var dest = new Vector3(this._currentPoint.x, this._currentPoint.y, 0f);
            var pos = this.transform.position;
            var dir = dest - pos;
            this._angle = Vector2.SignedAngle(Vector2.up, dir);

            if (dir.magnitude >= 0.1f) {
                // Stear torwards current point
                this.transform.position += dir.normalized * Time.deltaTime * this.speed;
            } else if (this._path.Count > 0) {
                // Next point in path
                this._currentPoint = this._path.Dequeue();
            } else {
                // Path finished!
                this._path = null;
                this.StartNewRandomPath();
            }

            var dirVec = this.currentDirVector;
            this.transform.rotation = Quaternion.AngleAxis(this._angle, Vector3.forward);
        }

        public void SetPath(IEnumerable<Vector2Int> path) 
        {
            this._path = new Queue<Vector2Int>(path);
            this._currentPoint = this._path.Dequeue();
        }

    }
}