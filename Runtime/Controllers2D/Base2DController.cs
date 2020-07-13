using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D{
    /// <summary>
    /// Logic to get the movement direction from Unity's new InputSystem.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Base2DController : MonoBehaviour {

        [Tooltip("Multiplied against your current direction to speed up your movement!")]
        [SerializeField] protected float moveForceMultiplier = 20f;
        protected Rigidbody2D rigidbody2D;

        // the current direction we wish to move, initialized to 0
        protected Vector2 direction = Vector2.zero;

        protected void Start() {
            rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Using Unity's new InputSystem, read input and store it as the current direction.
        /// Currently only works with 2D.
        /// </summary>
        public void SetDirection(InputAction.CallbackContext context) {
            direction = context.ReadValue<Vector2>();
        }
        
        /// <summary>
        /// Gets the current direction of movement.
        ///
        /// Useful if you need to perform extra logic to determine what direction you're moving.
        /// </summary>
        /// <returns></returns>
	    public Vector2 GetDirection(){
	        return direction;
	    }
    }
}