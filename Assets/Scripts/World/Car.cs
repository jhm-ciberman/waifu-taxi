using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class Car : MonoBehaviour
    {
        private Queue<Vector2Int> _path = null;
        
        private Vector2Int _currentPoint;

        public float speed = 0.5f;

        private RoutePlanner _planner;

        private float _currentAngle;

        public void SetWorld(World world)
        {
            this._planner = new RoutePlanner(world);
            this.StartNewRandomPath();
        }

        public void StartNewRandomPath()
        {
            if (this._planner == null) return;

            var path = this._planner.CalculatePath(this.transform.position, this._currentAngle);
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
            this._currentAngle = Vector2.SignedAngle(Vector2.up, dir);

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

            var dirVec = this._planner._GetDirVector(this._currentAngle);
            this.transform.rotation = Quaternion.AngleAxis(this._currentAngle, Vector3.forward);
        }

        public void SetPath(Queue<Vector2Int> path) 
        {
            this._path = path;
            Debug.Log("STARTING PATH LENGTH = " + path.Count);
            this._currentPoint = path.Dequeue();
        }

    }
}