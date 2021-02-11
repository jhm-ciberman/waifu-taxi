using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Car : Entity
    {
        private Queue<Vector2> _path = null;
        
        private enum State
        {
            Moving,
            Waiting,
            Crashed,
        }

        private Vector2 _prevPoint;
        private Vector2 _currentPoint;

        private float _speed = 0f;
        public float acceleration = 0.5f;
        public float maxSpeed = 0.5f;
        public float roadSeparation = 0.15f;

        private State _state = State.Moving;
        private float _waitTimeout = 0f;

        private Rigidbody2D _rb;

        public System.Action onCollision;

        private Pathfinder _pathfinder;

        public void Awake()
        {
            this._prevPoint = this.currentCoord;
            this._currentPoint = this.currentCoord;
            this._rb = this.GetComponent<Rigidbody2D>();
        }

        public void SetPathfinder(Pathfinder pathfinder)
        {
            this._pathfinder = pathfinder;
            this.StartNewRandomPath();
        }

        public void SetDeltaSpeed(float deltaSpeed)
        {
            this._speed += deltaSpeed;
        }

        public void StartNewRandomPath()
        {
            this._path = null;
            if (this._pathfinder == null) return;

            var end = this._pathfinder.RandomDestination(this.currentCoord);
            var path = this._pathfinder.Pathfind(this.currentCoord, end, this.currentDirVector);
            if (path != null) {
                var pathSoft = PathRetracer.Retrace(path, this.roadSeparation);
                this.SetPath(pathSoft);
            }
        }

        public void FixedUpdate()
        {
            if (this._path == null) return;
            if (this._state == State.Waiting) {
                this._waitTimeout -= Time.deltaTime;
                if (this._waitTimeout < 0f) {
                    this._state = State.Moving;
                }
            };

            var target = this._currentPoint;
            var pos = new Vector2(this.transform.position.x, this.transform.position.y);
            var dir = target - pos;

            if (dir.magnitude >= 0.10f) {
                // Stear torwards current point
                var dirNormalized = dir.normalized;
                this._angle = Vector2.SignedAngle(Vector2.up, dirNormalized);
                this._speed += this.acceleration * Time.fixedDeltaTime;
                if (this._speed > this.maxSpeed) {
                    this._speed = this.maxSpeed;
                }
                this._rb.position += dirNormalized * Time.fixedDeltaTime * this._speed;
            } else if (this._path.Count > 0) {
                this._NextPoint();
            } else {
                this.StartNewRandomPath();
            }

            var dirVec = this.currentDirVector;
            this._rb.rotation = this._angle; //Quaternion.AngleAxis(, Vector3.forward);

            
        }

        public IEnumerable<Vector2> path => this._path;

        public void SetPath(IEnumerable<Vector2> path) 
        {
            this._path = new Queue<Vector2>(path);
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