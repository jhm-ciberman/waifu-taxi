using UnityEngine;

namespace WaifuTaxi
{
    public class Player : Entity
    {
        private float _speed = 0f;

        public float globalMultiplier = 200f;
        public float maxSpeed = 0.6f;
        public float friction = 1f;
        public float aceleration = 10f;
        public float deaceleration = 0.6f;
        public float turnSpeed = 30f;
        public float requiredRotationSpeed = 0.2f;

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
                this._angle += this.turnSpeed * Time.deltaTime * rotationSpeed;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this._angle -= this.turnSpeed * Time.deltaTime * rotationSpeed;
            }

            if (this._speed > 0f) {
                this._speed -= this.friction * Time.deltaTime;
            }

            this._angle %= 360f;

            var rot = Quaternion.AngleAxis(this._angle, Vector3.forward);
            this.transform.rotation = rot;
            this.transform.position += rot * Vector3.up * this._speed / this.globalMultiplier;
        }
    }
}