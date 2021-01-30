using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class Car : Entity
    {
        private Queue<Vector2Int> _path = null;
        
        private Vector2Int _prevPoint;
        private Vector2Int _currentPoint;

        public float speed = 0.5f;
        public float roadSeparation = 0.15f;

        private World _world;

        public void Awake()
        {
            this._prevPoint = this.currentCoord;
            this._currentPoint = this.currentCoord;
        }

        public void SetWorld(World world)
        {
            this._world = world;
            this.StartNewRandomPath();
        }

        public void StartNewRandomPath()
        {
            this._path = null;
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

            var target = this.GetTarget();
            var pos = new Vector2(this.transform.position.x, this.transform.position.y);
            var dir = target - pos;

            if (dir.magnitude >= 0.10f) {
                // Stear torwards current point
                var dirNormalized = dir.normalized;
                this._angle = Vector2.SignedAngle(Vector2.up, dirNormalized);
                this.transform.position += new Vector3(dirNormalized.x, dirNormalized.y, 0f) * Time.deltaTime * this.speed;
            } else if (this._path.Count > 0) {
                this._NextPoint();
            } else {
                this.StartNewRandomPath();
            }

            var dirVec = this.currentDirVector;
            this.transform.rotation = Quaternion.AngleAxis(this._angle, Vector3.forward);
        }

        private Vector2 GetTarget()
        {
            var pos = new Vector2(this.transform.position.x, this.transform.position.y);
            var goalDir = (this._currentPoint - this._prevPoint);
            var offset = new Vector2(-goalDir.y, -goalDir.x) * this.roadSeparation;
            return this._currentPoint + offset;
        }

        public void SetPath(IEnumerable<Vector2Int> path) 
        {
            this._path = new Queue<Vector2Int>(path);
            this._NextPoint();

            if (this._prevPoint == this._currentPoint) {
                this._NextPoint();
            }
        }

        private void _NextPoint()
        {
            this._prevPoint = this._currentPoint;
            this._currentPoint = this._path.Dequeue();
        }

    }
}