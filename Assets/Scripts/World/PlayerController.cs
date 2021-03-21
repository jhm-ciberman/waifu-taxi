using UnityEngine;

namespace WaifuDriver
{
    public class PlayerController : Entity
    {
        public System.Action onCollision;
        
        //private bool _maxSpeed = false;
        private float _speed = 0f;
        private Rigidbody2D _rb;

        private Car _car;

        void Awake()
        {
            this._car = this.GetComponent<Car>();
            this._rb = this.GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            /*
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                this._speed += this.aceleration * Time.deltaTime;
                if (this._speed >= this.maxSpeed) {
                    this._speed = this.maxSpeed;
                    if (! this._maxSpeed) {
                        AudioManager.Instance.PlaySound("car_maximum_speed");
                    }
                    this._maxSpeed = true;
                } else if (this._speed < this.maxSpeed * 0.5f) {
                    this._maxSpeed = false;
                }
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                this._speed -= this.deaceleration * Time.fixedDeltaTime;
            }

            var rotationSpeed = (this._speed / this.requiredRotationSpeed);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this._angle += this.turnSpeed * Time.fixedDeltaTime * rotationSpeed;
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this._angle -= this.turnSpeed * Time.fixedDeltaTime * rotationSpeed;
            }

            if (this._speed > 0f) {
                this._speed -= this.friction * Time.fixedDeltaTime;
            }

            this._angle %= 360f;

            //var rot = Quaternion.AngleAxis(this._angle, Vector3.forward);
            
            var radians = -this._angle * Mathf.Deg2Rad;
            var rot = new Vector2(Mathf.Sin(radians), Mathf.Cos(radians));
            this._rb.MoveRotation(this._angle);
            this._rb.position += rot * this._speed / this.globalMultiplier;
            */

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                this._car.Throttle(1f);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                this._car.Throttle(-1f);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                this._car.Rotate(-1f);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                this._car.Rotate(1f);
            }

        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (Mathf.Abs(this._speed) > 0.5f) {
                AudioManager.Instance.PlaySound("car_crash");
                this.onCollision?.Invoke();
            }
        }
    }
}