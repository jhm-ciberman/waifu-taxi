using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class RoutePlanner
    {
        private World _world;
        
        private Entity _entity;

        private List<Vector2Int> _path;

        private int _pathIndex = 0;

        private Vector2Int _nextGoal;

        private Vector2Int _currentCoord;

        private Vector2Int _finalDestination;

        private Indication _currentIndication = Indication.None;

        public System.Action<IndicationEvent> onIndication;
        
        private bool _pathWasRecentlyRestarted = false;

        public RoutePlanner(World world, Entity entity)
        {
            this._world = world;
            this._entity = entity;
        }

        public void UpdatePath()
        {
            var coord = this._entity.currentCoord;

            if (this._path == null) {
                this.StartNewPath();
            }

            if (coord == this._nextGoal) {
                this.AdvanceToNextGoal(); // Goal reached
            } else if (coord != this._currentCoord) {
                this.RecalculatePath(); // Wrong path
            } else {
                this.CalculateIndications();
            }
        }

        public void CalculateIndications()
        {
            var dirVector = this._nextGoal - this._currentCoord;

            var goalAngle = Vector2.SignedAngle(Vector2.up, dirVector);
            var angleDelta = Mathf.DeltaAngle(this._entity.angle, goalAngle);
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

            this._SetNewIndication(indication);
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
            this._finalDestination = this._GetRandomDestination();
            this.RecalculatePath(); // No destination
        }

        private void AdvanceToNextGoal()
        {
            this._pathIndex++; // Goal reached!
            
            if (this._pathIndex < this._path.Count) {
                this._currentCoord = this._path[this._pathIndex - 1];
                this._nextGoal = this._path[this._pathIndex];

                this.CalculateIndications();
            } else {
                Debug.Log("GOAL REACHED!!! ");
            }
            this._pathWasRecentlyRestarted = false;
        }

        private Vector2Int _GetRandomDestination()
        {
            var start = this._entity.currentCoord;
            Vector2Int end;
            int tries = 0;
            do {
                end = this._world.RandomRoad();
                tries++;
                if (tries > 1000) return end;
            } while (end == start);
            return end;
        }

        public void RecalculatePath()
        {
            if (this._path != null) {
                this._pathWasRecentlyRestarted = true;
            }
            var pathfinder = new CarPathfinder(this._world, this._entity.currentCoord, this._finalDestination, this._entity.currentDirVector);
            this._path = pathfinder.Pathfind();
            this._pathIndex = 0;
            if (this._path.Count > 1) {
                this.AdvanceToNextGoal();
            } else {
                this._path = null; 
                Debug.Log("Invalid goal");
            }
        }
    }
}