using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Applies force in the given horizontal direction.
    /// </summary>
    public class SideScrollController : Base2DController {
        [Tooltip("The force applied in the up direction upon jumping.")]
        [SerializeField] protected float _jumpForceMultiplier = 100f;

        // used to make sure your player is not jumping already
        private bool _isJumping = true;

        new void Start() {
            base.Start();
        }

        protected void FixedUpdate() {
            if (direction == Vector2.right || direction == Vector2.left) {
                rigidbody2D.AddForce(direction * moveForceMultiplier);
            }
        }

        public void Jump(InputAction.CallbackContext context) {
            if (!_isJumping) {
                rigidbody2D.AddForce(Vector2.up * _jumpForceMultiplier);

                _isJumping = true;
            }
        }

        // if you've hit a ground object, then you're no longer jumping
        // TODO: could add in a timer to prevent you from instantly jumping again
        public void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Ground") {
                _isJumping = false;
            }
        }
    }
}
