using System.Collections.Generic;
using UnityEngine;

namespace WaifuTaxi
{
    public class Car : MonoBehaviour
    {
        private Queue<Vector2Int> _path = new Queue<Vector2Int>();
        
        private Vector2Int _currentPoint;

        public float speed = 0.5f;

        public void Start()
        {
            var path = new Queue<Vector2Int>();
            path.Enqueue(new Vector2Int(2, 3));
            path.Enqueue(new Vector2Int(5, 3));
            path.Enqueue(new Vector2Int(5, 10));
            this.SetPath(path);
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
            this._currentPoint = path.Dequeue();
        }

    }
}