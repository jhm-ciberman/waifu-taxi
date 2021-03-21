using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WaifuDriver
{
    public class Car : Entity
    {
        // See: https://asawicki.info/Mirror/Car%20Physics%20for%20Games/Car%20Physics%20for%20Games.html

        [SerializeField] private float _engineForce = 10f;
        [SerializeField] private float _dragCoeficient = 0.4257f;
        [SerializeField] private float _rollingResistanceCoeficient = 12.8f; 

        public float engineForce                 { get => this._engineForce;                 set { this._engineForce = value; this._UpdateMaxSpeed(); }}
        public float dragCoeficient              { get => this._dragCoeficient;              set { this._dragCoeficient = value; this._UpdateMaxSpeed(); }}
        public float rollingResistanceCoeficient { get => this._rollingResistanceCoeficient; set { this._rollingResistanceCoeficient = value; this._UpdateMaxSpeed(); }}

        public float wheelsStearingSpeed = 60f;
        public float maxStearAngle = 45f;

        public Transform leftWheel = null;
        public Transform rightWheel = null;

        private Rigidbody2D _rb;

        private Collider2D _collider;

        public System.Action onCollision;

        private ContactFilter2D _contactFilter;

        private RaycastHit2D[] _raycastResults = new RaycastHit2D[2];

        private float _steerAngle = 0f;

        private float _throttle = 0f;

        private float _wheelsCurrentDeltaAngle = 0f;

        private Vector2 _wheelsForwardVector;

        private float _axlesDistance;



        public void Awake()
        {
            this._rb = this.GetComponent<Rigidbody2D>();
            this._collider = this.GetComponent<BoxCollider2D>();
            if (this.leftWheel != null) {
                this._axlesDistance = this.leftWheel.localPosition.y;
            } else {
                this._axlesDistance = this._collider.bounds.size.y / 2f;
            }

            this._UpdateMaxSpeed();
        }

        public new Collider2D collider => this._collider;

        public float speed => this._rb.velocity.magnitude;

        public float maxSpeed { get; private set; }
        public float maxAcceleration { get; private set; }

        public void Throttle(float speed = 1f)
        {
            this._throttle = Mathf.Clamp(speed, -1f, 1f);
        }

        public void Rotate(float dir)
        {
            this._steerAngle = Mathf.Clamp(dir, -1f, 1f);
        }

        void FixedUpdate()
        {
            this._UpdateWheelsDirection(this._steerAngle);
            this._steerAngle = 0f;

            this._UpdateVisualWheelAngle(this._wheelsCurrentDeltaAngle);


            Vector2 headingVector = this._GetHeadingVector();
            
            var longitudinalForce = this._GetLongitudinalForce(this._throttle);
            this._rb.AddForce(longitudinalForce);
            //var speed = this._rb.velocity.magnitude + longitudinalForce.magnitude * Mathf.Sign(this._throttle) * Time.fixedDeltaTime;
            //this._rb.velocity = headingVector * speed;
            this._throttle = 0f;

            this._UpdateRotation(this._wheelsCurrentDeltaAngle);
        }

        private Vector2 _GetHeadingVector()
        {
            var rad = -(this._rb.rotation + this._wheelsCurrentDeltaAngle) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
        }

        private void _UpdateWheelsDirection(float wheelsMoveDeltaDir)
        {
            var maxRotation = this.wheelsStearingSpeed * Time.fixedDeltaTime;
            if (wheelsMoveDeltaDir != 0) {
                var angle = this._wheelsCurrentDeltaAngle - wheelsMoveDeltaDir * maxRotation;
                this._wheelsCurrentDeltaAngle = Mathf.Clamp(angle, -this.maxStearAngle, this.maxStearAngle);
            } else {
                this._wheelsCurrentDeltaAngle -= Mathf.Clamp(this._wheelsCurrentDeltaAngle, -maxRotation, maxRotation);
            }
        }

        private Vector2 _GetLongitudinalForce(float forwardDelta)
        {
            Vector2 headingVector = this._rb.transform.up;
            float speed = this._rb.velocity.magnitude;
            Vector2 tractionForce = headingVector * this._engineForce * forwardDelta;
            //Vector2 dragForce = -this._dragCoeficient * speed * this._rb.velocity; 
            Vector2 rollingResistanceForce = -this._rollingResistanceCoeficient * this._rb.velocity;
            return tractionForce + rollingResistanceForce;
        }

        private void _UpdateMaxSpeed() 
        {
            // Solves the quadratic equation for speed
            var a = this._rb.drag;
            var b = this._rollingResistanceCoeficient;
            var c = -this._engineForce;
            float preRoot = b * b - 4f * a * c;
            this.maxSpeed = (preRoot < 0)
                ? 0f
                : (Mathf.Sqrt(preRoot) - b) / (2.0f * a);

            // Update max acceleration (A = F / M)
            float v = this.maxSpeed;
            float netForce = this._engineForce - this._rb.drag * v * v - this._rollingResistanceCoeficient * v;
            this.maxAcceleration = netForce / this._rb.mass;
        }

        private void _UpdateRotation(float deltaAngle)
        {
            //if (deltaAngle == 0f) return;
            //var rotationRadius = this._axlesDistance / deltaAngle;
            //var angularVelocity =  / rotationRadius;

            this._rb.AddTorque(this.speed);
            //this._rb.MoveRotation(this._rb.rotation + angularVelocity * Time.fixedDeltaTime);
        }

        private void _UpdateVisualWheelAngle(float deltaAngle)
        {
            var wheelAngle = Quaternion.AngleAxis(deltaAngle, Vector3.forward);
            if (this.leftWheel != null)  this.leftWheel.localRotation = wheelAngle;
            if (this.rightWheel != null) this.rightWheel.localRotation = wheelAngle;
        }

        public void OnDrawGizmos()
        {
            if (this._rb == null) return;
            //Gizmos.DrawLine(this._rb.position, this._rb.position + this._wheelsForwardVector);

            var rad = -(this._rb.rotation + this._wheelsCurrentDeltaAngle) * Mathf.Deg2Rad;
            Vector2 headingVector = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
            Gizmos.DrawLine(this._rb.position, this._rb.position + headingVector);
        }

        public bool RaycastAhead(float aheadDist)
        {
            var resultsCount = Physics2D.Raycast(this.currentPosition, this.transform.up, this._contactFilter, this._raycastResults, aheadDist);
            for (int i = 0; i < resultsCount; i++) {
                var hit = this._raycastResults[i];
                if (hit.collider != this._collider) {
                    return true;
                }
            }
            return false;
        }
        
    }
}