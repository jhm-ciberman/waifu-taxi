using UnityEngine;

namespace WaifuDriver
{
    public class CameraController : MonoBehaviour
    {
        private Transform _target;

        public float aheadDistanceMin = 0.4f;
        public float aheadDistanceMulti = 0.1f;

        private Vector2 _velocity;
        private float _angleVelocity;

        public float positionSmoothTime = 0.5f;
        public float rotationSmoothTime = 0.5f;

        private Camera _camera;

        public float minSize = 0.75f;
        public float sizeScale = 0.1f;

        public void Start()
        {
            this._camera = this.GetComponent<Camera>();
        }

        public void SetTarget(Transform target)
        {
            this._target = target;
            this.transform.position = this._target.position;
            this.transform.rotation = this._target.rotation;
        }

        private Vector3 _prevTargetPos;

        void Update()
        {
            if (this._target == null) return;

            var current = this.transform.eulerAngles;
            var target = this._target.eulerAngles;
            var angle = Mathf.SmoothDampAngle(current.z, target.z, ref this._angleVelocity, this.rotationSmoothTime);
            this.transform.eulerAngles = new Vector3(current.x, current.y, angle);
            
            var speedMagnitude = this._velocity.magnitude;
            var aheadDistance = (this.aheadDistanceMulti * speedMagnitude + this.aheadDistanceMin);
            var targetPos = this._target.position + this._target.up * aheadDistance;
            var v = Vector2.SmoothDamp(this.transform.position, targetPos, ref this._velocity, this.positionSmoothTime);
            this.transform.position = new Vector3(v.x, v.y, -1f);

            this._camera.orthographicSize = speedMagnitude * this.sizeScale + this.minSize;
            this._prevTargetPos = this.transform.position;
        }
    }
}