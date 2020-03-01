using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Uses physics based displacement of TopDownController,
    /// with the added effect of pointing player towards the mouse's world location.
    /// </summary>
    public class TDC_FaceMouse : TopDownController {

        [Tooltip("You must adjust this correctly for the direction your sprite faces by default, otherwise mouse facing won't work.")]
        [SerializeField] private FacingDirection _startingDirection = FacingDirection.RIGHT;

        protected new void Start() {
            base.Start();
        }

        // store initial location of the camera so we can snap it back once you've left the look ahead realm
        public void LookAtMouse(InputAction.CallbackContext context) {
            Vector2 directionTowardsMouse = GetMousePositionInWorldSpace();
            float angle = Mathf.Atan2(directionTowardsMouse.y, directionTowardsMouse.x) * Mathf.Rad2Deg;

            // taking initial direction into account, now rotate towards the mouse!
            transform.rotation = Quaternion.AngleAxis((angle + (float)_startingDirection) % 360, Vector3.forward);
        }

        protected Vector2 GetMousePositionInWorldSpace() {
            // get the mouse's world location, as seen by the camera
            Vector2 mouseLocation = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // calculate the direction, and the angle at which to face
            return mouseLocation - (Vector2)transform.position;
        }
    }
}