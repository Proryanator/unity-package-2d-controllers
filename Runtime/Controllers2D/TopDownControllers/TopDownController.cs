using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Script only deals with the 2D force adding to a sprite.
    /// Does nothing to change sprite's direction.
    /// </summary>
    public class TopDownController : Base2DController {
        [Tooltip("If true, will add force to the object times the force multiplier.")]
        [SerializeField] private bool _useForce = false;

        [Tooltip("The maximum velocity this object will travel, regardless if you use force or not.")]
        [SerializeField] private float _maxVelocity = 20f;

        private bool _allowedToMove = true;

        protected new void Start() {
            base.Start();
        }

        // for top-down scripts, you typically want to move in all directions,
        // so we'll simply apply force to whichever direction you're moving
        protected void FixedUpdate() {
            if (!_allowedToMove) {
                return;
            }

            if (_useForce) {
                rigidbody2D.AddForce(direction * moveForceMultiplier);

                // limit the max speed of the player
                rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity, _maxVelocity);
            } else if (direction != Vector2.zero) {
                rigidbody2D.velocity = GetVector2From(direction, _maxVelocity);
            }
        }

        /// <summary>
        /// You may want to postpone movement of this script for some reason. This allows you to do just that.
        /// </summary>
        public void PostponeMovement(float seconds) {
            StartCoroutine(PostponeMovementRoutine(seconds));
        }

        private IEnumerator PostponeMovementRoutine(float seconds) {
            _allowedToMove = false;
            yield return new WaitForSeconds(seconds);
            _allowedToMove = true;
        }

        private static Vector2 GetVector2From(float angle, float magnitude) {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
        }

        private static Vector2 GetVector2From(Vector2 direction, float magnitude) {
            // calculate the angle to use in returning you the Vector2 in that direction
            float angle = Mathf.Sign(direction.y) * Mathf.Acos(direction.x);
            return GetVector2From(angle, magnitude);
        }
    }
}