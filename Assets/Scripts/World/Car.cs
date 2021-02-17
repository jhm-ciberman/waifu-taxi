using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class Car : Entity
    {
        private Path _path = null;
        
        private enum State
        {
            Moving,
            Waiting,
            Crashed,
        }

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
            var path = this._pathfinder.Pathfind(this.currentCoord, end, this.currentDirVector, this.roadSeparation);
            if (path != null) {
                this._path = path;
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

            var p = this._path.ClosestPoint(this.currentPosition);

            if (p.length < this._path.length - 0.2f) {
                // Stear torwards current point
                var target = this.GetTarget(p.length);
                var dir = target - this.currentPosition;
                var dirNormalized = dir.normalized;
                this._angle = Vector2.SignedAngle(Vector2.up, dirNormalized);
                this._speed += this.acceleration * Time.fixedDeltaTime;
                if (this._speed > this.maxSpeed) {
                    this._speed = this.maxSpeed;
                }
                this._rb.position += dirNormalized * Time.fixedDeltaTime * this._speed;
            } else {
                this.StartNewRandomPath();
            }

            var dirVec = this.currentDirVector;
            this._rb.rotation = this._angle; //Quaternion.AngleAxis(, Vector3.forward);
        }

        private Vector2 GetTarget(float currentLength)
        {
            var v0 = this._path.GetPosition(currentLength);
            var v1 = this._path.GetPosition(currentLength + 0.3f);
            return (v0 + v1) / 2f;
        }

        
        private void OnDrawGizmos()
        {
            return;
            RouteGizmo.DrawRoute(this._path);

            var currentPosition = new Vector2(this.transform.position.x, this.transform.position.y);
            var p = this._path.ClosestPoint(currentPosition);
            var v0 = this._path.GetPosition(p.length);
            var v1 = this._path.GetPosition(p.length + 0.3f);
            var target = (v0 + v1) / 2f;
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(new Vector3(v0.x, v0.y, 0.2f), 0.05f);
            Gizmos.DrawSphere(new Vector3(v1.x, v1.y, 0.2f), 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(target.x, target.y, 0.2f), 0.05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(p.position.x, p.position.y, 0.2f), 0.05f);
        }

    }
}