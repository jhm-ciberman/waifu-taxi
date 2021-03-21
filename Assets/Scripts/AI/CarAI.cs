using UnityEngine;

namespace WaifuDriver
{
    public class CarAI : MonoBehaviour
    {
        private Path _path = null;
        
        private enum State
        {
            Moving,
            Waiting,
            Crashed,
        }

        private Pathfinder _pathfinder;

        private Vector2 _targetSpeed = Vector2.zero;

        private Vector2 _lookAheadVector;

        private float _waitTimeout = 0f;

        private float _currentPathLength = 0f;

        private State _state = State.Moving;

        public bool drawRoute = false;

        private float _targetAngle;

        private Car _car;

        public float lookAheadMaxDist = 0.4f;
        public float lookAheadMixDist = 0.1f;

        void Awake()
        {
            this._car = this.GetComponent<Car>();
            this.StartNewRandomPath();
        }

        public void SetPathfinder(Pathfinder pathfinder)
        {
            this._pathfinder = pathfinder;
            if (this._path == null) {
                this.StartNewRandomPath();
            }
        }
        
        public void StartNewRandomPath()
        {
            this._path = null;
            if (this._pathfinder == null) return;

            var end = this._pathfinder.RandomDestination(this._car.currentCoord);
            var path = this._pathfinder.Pathfind(this._car.currentCoord, end, this._car.currentDirVector);
            if (path != null) {
                this._path = path;
                this._currentPathLength = 0f;
            }
        }

        public void Update()
        {
            if (this._path == null) return;
            if (this._state == State.Waiting) {
                this._waitTimeout -= Time.deltaTime;
                if (this._waitTimeout < 0f) {
                    this._state = State.Moving;
                }
            };

            
            var p = this._path.ClosestPoint(this._car.currentPosition, this._currentPathLength, 1f);
            this._currentPathLength = p.length;

            var t = this._car.speed / 2f;
            var aheadDist = Mathf.Lerp(this.lookAheadMixDist, this.lookAheadMaxDist, t);
            this._lookAheadVector = this._car.transform.up * aheadDist;

            if (this._car.RaycastAhead(aheadDist)) {
                this._state = State.Waiting;
                this._waitTimeout = 0.5f;
                this._targetSpeed = Vector2.zero;
                return;
            }

            if (p.length < this._path.length - 0.2f) {
                // Stear torwards current point
                var target = this.GetTarget(p.length);
                var dir = target - this._car.currentPosition;
                var dirNormalized = dir.normalized;
                this._targetAngle = Vector2.SignedAngle(Vector2.up, dirNormalized);
                this._targetSpeed = this._car.maxSpeed * dirNormalized;
            } else {
                this.StartNewRandomPath();
            }

            var currentSpeedMagnitude = this._car.speed;
            var targetSpeedMagnitude = this._targetSpeed.magnitude;

            if (currentSpeedMagnitude < targetSpeedMagnitude) {
                this._car.Throttle(1f);
            }

            var deltaAngle = Mathf.DeltaAngle(this._car.angle, this._targetAngle);
            this._car.Rotate(deltaAngle);
        }

        private Vector2 GetTarget(float currentLength)
        {
            var v0 = this._path.GetPosition(currentLength);
            var v1 = this._path.GetPosition(currentLength + 0.3f);
            return (v0 + v1) / 2f;
        }

        
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(this._car.currentPosition, this._car.currentPosition + this._lookAheadVector);

            if (this.drawRoute && this._path != null) {
                RouteGizmo.DrawRoute(this._path);

                var p = this._path.ClosestPoint(this._car.currentPosition, this._currentPathLength, 1f);
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
}