using UnityEngine;

namespace WaifuTaxi
{
    public class Player : MonoBehaviour
    {
        private float _speed;
        private float _direction;

        public float globalMultiplier = 200f;
        public float maxSpeed = 0.6f;
        public float friction = 1f;
        public float aceleration = 10f;
        public float deaceleration = 0.6f;
        public float turnSpeed = 30f;
        public float requiredRotationSpeed = 0.2f;

        private World _world;

        public void SetWorld(World world)
        {
            this._world = world;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                this._speed += this.aceleration * Time.deltaTime;
                if (this._speed >= this.maxSpeed) this._speed = this.maxSpeed;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                this._speed -= this.deaceleration * Time.deltaTime;
            }

            var rotationSpeed = (this._speed / this.requiredRotationSpeed);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this._direction += this.turnSpeed * Time.deltaTime * rotationSpeed;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this._direction -= this.turnSpeed * Time.deltaTime * rotationSpeed;
            }

            if (this._speed > 0f) {
                this._speed -= this.friction * Time.deltaTime;
            }

            var rot = Quaternion.AngleAxis(this._direction, Vector3.forward);
            this.transform.rotation = rot;
            this.transform.position += rot * Vector3.up * this._speed / this.globalMultiplier;

            //Debug.Log(this._world.PositionToTileCoord(this.transform.position));
        }
    }
}