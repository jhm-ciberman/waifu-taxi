using UnityEngine;

namespace WaifuTaxi
{
    public class Player : Entity
    {
        private float _speed = 0f;

        public float globalMultiplier = 200f;
        public float maxSpeed = 0.7f;
        public float friction = 0.2f;
        public float aceleration = 0.6f;
        public float deaceleration = 0.5f;
        public float turnSpeed = 35f;
        public float requiredRotationSpeed = 0.2f;

        private Rigidbody2D _rb;

        void Awake()
        {
            this._rb = this.GetComponent<Rigidbody2D>();
        }

        public System.Action onCollision;

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

            //var rot = Quaternion.AngleAxis(this._angle, Vector3.forward);
            
            var radians = -this._angle * Mathf.Deg2Rad;
            var rot = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
            this._rb.rotation = this._angle;
            this._rb.position += rot * this._speed / this.globalMultiplier;
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            this.onCollision?.Invoke();
        }
    }
}