using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Keeps a player crawling along a wall or surface.
    /// </summary>
    public class StickyController : Base2DController {

        [SerializeField] private float _jumpModifier = 30f;

        // tracks the direction of gravity
        private Vector2 _directionOfGravity;
        private const float GRAVITY = 9.8f;

        [SerializeField] private float _gravityMultiplier = 1f;

        // these are the allowed directions of movement
        private ArrayList _allowedDirections = new ArrayList();

        private bool _isJumping = false;

        [Tooltip("Using this will force default gravity when you jump off of walls.")]
        [SerializeField] private bool _useActualGravity = false;

        new void Start() {
            base.Start();

            _directionOfGravity = Physics2D.gravity;

            if (rigidbody2D.gravityScale != 0) {
                Debug.LogWarning("You have gravity still enabled for this object, you need to disable it for this script to work.");
            }
        }

        public void FixedUpdate() {
            // constantly applying gravity force in the given direction
            rigidbody2D.AddForce(_directionOfGravity * _gravityMultiplier);

            // also applies force in the direction you want to move!
            if (GetAllowedDirections(_directionOfGravity).Contains(direction)) {
                rigidbody2D.AddForce(direction * moveForceMultiplier);
            }
        }

        // when you hit with another object, we'll turn gravity off,
        // and start applying force in the direction of the collision
        public void OnCollisionEnter2D(Collision2D collision) {
            // get the normal of the collision
            _directionOfGravity = collision.GetContact(0).normal * -1 * GRAVITY;

            _isJumping = false;
        }

        // could either just jump you upwards, or swap gravity, depending
        // on your preference
        public void Jump(InputAction.CallbackContext context) {
            Vector2 _inverseOfGravity = _directionOfGravity * -1;
            if (!_useActualGravity) {
                _directionOfGravity = _inverseOfGravity;
            } else {
                _directionOfGravity = Physics2D.gravity;

                if (!_isJumping) {
                    rigidbody2D.AddForce(_inverseOfGravity * _jumpModifier);

                    _isJumping = true;
                }
            }
        }

        // given the direction of gravity, return directions that you're allowed to move
        private ArrayList GetAllowedDirections(Vector2 gravityDirection) {
            ArrayList allowedDirections = new ArrayList();

            // gravity is in the horizonal position, thus we want to allow vertical movement
            if (Mathf.Abs(gravityDirection.x) > 5) {
                allowedDirections.Add(Vector2.up);
                allowedDirections.Add(Vector2.down);
            } else {
                allowedDirections.Add(Vector2.right);
                allowedDirections.Add(Vector2.left);
            }

            return allowedDirections;
        }
    }
}