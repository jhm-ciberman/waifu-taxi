using UnityEngine;

namespace WaifuDriver
{
    public class RoutePlanner
    {
        private const float RECALCULATE_PATH_DIST_SQR = 1.2f * 1.2f;

        private Pathfinder _pathfinder;
        
        private Entity _entity;

        private Path _path;

        private PathPoint _currentPoint;

        private Vector2Int _finalDestination;

        private Indication _currentIndication = Indication.None;

        public System.Action<IndicationEvent> onIndication;
        public System.Action onPathFinished;
        
        private bool _pathWasRecentlyRestarted = false;

        public RoutePlanner(Pathfinder pathfinder, Entity entity)
        {
            this._pathfinder = pathfinder;
            this._entity = entity;
        }

        public void UpdatePath()
        {
            if (this._path == null) {
                this.StartNewPath();
            }

            var currentPos = this._entity.currentPosition;

            this._currentPoint = this._path.ClosestPoint(currentPos);
            (_, int targetIndex) = this._path.GetPointsIndicesBetween(this._currentPoint.length);
            var nextGoal = this._path.GetPositionAtIndex(targetIndex);
            float sqrDist = (nextGoal - currentPos).sqrMagnitude;
            if (sqrDist > RECALCULATE_PATH_DIST_SQR) {
                this._RecalculatePath(); // Wrong path
            } else if (this._currentPoint.length > this._path.length - 0.5f) {
                this._OnPathFinished();
            } else {
                this._CalculateIndications(targetIndex);
            }
        }

        public void OnDrawGizmos()
        {
            if (this._path == null) return;
            RouteGizmo.DrawRoute(this._path);

            var p = this._path.ClosestPoint(this._entity.currentPosition);
            (int prev, int next) = this._path.GetPointsIndicesBetween(p.length);
            var v0 = this._path.GetPositionAtIndex(prev);
            var v1 = this._path.GetPositionAtIndex(next);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(new Vector3(v0.x, v0.y, 0.2f), 0.05f);
            Gizmos.DrawSphere(new Vector3(v1.x, v1.y, 0.2f), 0.05f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(p.position.x, p.position.y, 0.2f), 0.05f);
        }

        private void _OnPathFinished()
        {
            this.onPathFinished?.Invoke();
            this.StartNewPath();
        }

        private void _CalculateIndications(int goalIndex)
        {
            var nextIndex = goalIndex < this._path.pointsCount - 1 ? goalIndex + 1 : this._path.pointsCount - 1;
            var nextGoal = this._path.GetPositionAtIndex(goalIndex);
            var nextFolowingGoal = this._path.GetPositionAtIndex(nextIndex);

            var dirVector = nextFolowingGoal - nextGoal;
            var goalAngle = Vector2.SignedAngle(Vector2.up, dirVector);
            var indication = this._DirVectorToIndication(this._entity.angle, goalAngle);
            this._SetNewIndication(indication);
        }

        private Indication _DirVectorToIndication(float currentAngle, float goalAngle)
        {
            var angleDelta = Mathf.DeltaAngle(currentAngle, goalAngle);
            var sign = Mathf.Sign(angleDelta);
            var abs = Mathf.Abs(angleDelta);

            Indication indication;
            if (abs < 45f) {
                indication = Indication.Continue;
            } else if (abs < 135f) {
                indication = (sign > 0) ? Indication.TurnLeft : Indication.TurnRight;
            } else {
                indication = Indication.TurnU;
            }
            return indication;
        }

        private void _SetNewIndication(Indication indication)
        {
            if (indication == this._currentIndication) return;
            if (this._currentIndication == Indication.TurnU) {
                if (indication == Indication.TurnLeft || indication == Indication.TurnRight) {
                    return;
                }
            }

            var prevIndication = this._currentIndication;
            this._currentIndication = indication;

            var e = new IndicationEvent(indication, prevIndication, this._pathWasRecentlyRestarted);
            this.onIndication.Invoke(e);
        }

        public void StartNewPath()
        {
            this._finalDestination = this._pathfinder.RandomDestination(this._entity.currentCoord);
            this._RecalculatePath(); // No destination
        }

        private void _RecalculatePath()
        {
            if (this._path != null) {
                this._pathWasRecentlyRestarted = true;
            }
            var path = this._pathfinder.Pathfind(this._entity.currentCoord, this._finalDestination, this._entity.currentDirVector, 0f);
            if (path != null) {
                this._path = path;
                Debug.Log("Tiles: " + this._path.length);
            } else {
                this._path = null; 
                Debug.Log("Invalid goal");
            }
        }

        public Path path => this._path;
    }
}