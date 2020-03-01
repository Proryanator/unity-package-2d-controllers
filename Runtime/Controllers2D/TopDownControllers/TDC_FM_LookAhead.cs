using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Uses abilities of the TDC_FaceMouse script to face sprite towards mouse,
    /// with the ability to look ahead too.
    /// </summary>
    public class TDC_FM_LookAhead : TDC_FaceMouse {

        [Tooltip("Enables lookahead feature, where camera goes in the direction of the mouse. NOTE: camera tracking on the player must be enabled for this to work with movement.")]
        [SerializeField] private bool _enableMouseLookAhead = false;

        [Tooltip("The radius around the player that the mouse must pass before the camera begins to 'look ahead'.")]
        [SerializeField] private float _lookAheadStartRadius = 10f;

        // TODO: maybe these can be based on screen size or world space one day!
        [Tooltip("Radius that, past this point, the camera will no longer move in that direction.")]
        [SerializeField] private float _lookAheadMaxRadius = 20f;

        [Tooltip("0-1f, affects how far the camera will move in the direction of the mouse. 1f will move to the point of your mouse, less will be some percentage.")]
        [SerializeField] private float _cameraLookPercentage = 1f;

        private Vector3 _cameraInitialPosition;

        protected new void Start() {
            base.Start();

            _cameraInitialPosition = Camera.main.transform.position;
        }

        public void LookAheadIfEnabled(InputAction.CallbackContext context) {
            if (!_enableMouseLookAhead) {
                return;
            }

            Vector2 directionTowardsMouse = GetMousePositionInWorldSpace();
            float distanceFromPlayer = directionTowardsMouse.magnitude;

            // if the distance of the mouse from the player is past a certain radius, we'll adjust the camera
            if (Mathf.Abs(distanceFromPlayer) > _lookAheadStartRadius) {
                directionTowardsMouse = KeepCameraInMaxRadius(directionTowardsMouse, _lookAheadMaxRadius);

                // apply percentage to actually move the camera
                directionTowardsMouse *= _cameraLookPercentage;
                Vector3 newCameraPosition = new Vector3(directionTowardsMouse.x, directionTowardsMouse.y, _cameraInitialPosition.z);
                Camera.main.transform.position = newCameraPosition;
            }
        }

        // TODO: work on logic to be able to limit how far the camera is from the player
        private Vector2 KeepCameraInMaxRadius(Vector2 directionTowardsMouse, float maxRadius) {
            // cap the farthest the mouse will make the camera move!
            /*if (Mathf.Abs(distanceFromPlayer) > _lookAheadMaxRadius) {
                // make sure we maintain the same sign/direction of the magnitude when we do this, it matters!
                distanceFromPlayer = _lookAheadMaxRadius * Mathf.Sign(distanceFromPlayer);

                // update the Vector2 to be the limited distance, but same angle
                directionTowardsMouse = GetVector2From(Vector2.Angle(transform.position, directionTowardsMouse), distanceFromPlayer);
            }*/

            return directionTowardsMouse;
        }

        // TODO: this would add nice effects to the camera later if necessary, for now simply snaps towards the mouse
        private void SlowlyMoveCamera() {

        }

        // these will help to visualize the radius around the player, and where your start/stop ends
    }
}