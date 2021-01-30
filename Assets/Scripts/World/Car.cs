using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class Car : MonoBehaviour
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
            var start = new Vector2Int((int) this.transform.position.x, (int) this.transform.position.y);
            var end = this._world.RandomRoad();
            CarPathfinder pathfinder = new CarPathfinder(this._world, start, end);
            var path = pathfinder.Pathfind();
            if (path == null) {
                Debug.Log("PATH NOT FOUND start=" + start + " " + end);
                return;
            }
            var queue = new Queue<Vector2Int>(path);
            this.SetPath(queue);
        }

        public void Update()
        {
            if (this._path == null) return;

            var dest = new Vector3(this._currentPoint.x, this._currentPoint.y, 0f);
            var pos = this.transform.position;
            var dir = dest - pos;

            if (dir.magnitude >= 0.1f) {
                this.transform.position += dir.normalized * Time.deltaTime * this.speed;
            } else if (this._path.Count > 0) {
                this._currentPoint = this._path.Dequeue();
            } else {
                this._path = null;
            }

            var angle = Vector3.Angle(Vector3.right, dir);
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void SetPath(Queue<Vector2Int> path) 
        {
            this._path = path;
            Debug.Log("STARTING PATH LENGTH = " + path.Count);
            this._currentPoint = path.Dequeue();
        }

    }
}