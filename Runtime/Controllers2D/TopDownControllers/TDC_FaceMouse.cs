using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Proryanator.Controllers2D {
    /// <summary>
    /// Uses physics based displacement of TopDownController,
    /// with the added effect of pointing player towards the mouse's world location.
    /// 
    /// NOTE: if you're getting weird behaviour where you're colliding with objects and
    /// not facing the mouse, most likely you haven't restricted rotation on the 'z'.
    /// </summary>
    public class TDC_FaceMouse : TopDownController {

        [Tooltip("You must adjust this correctly for the direction your sprite faces by default, otherwise mouse facing won't work.")]
	    [SerializeField] private FacingDirection _startingDirection = FacingDirection.RIGHT;

	    [Tooltip("If true, will make the player always face the mouse, even if the mouse hasn't moved. False will only face the player when the mouse movement changes.")]
	    [SerializeField] private bool _onlyFaceWithMouseInput = true;

	    // stores the currently facing direction of the sprite, will initialize to the direction it starts as
	    private Vector2 _facingDirection;

	    // caches the main camera so we don't have to make this call every time we look around
	    private Camera _mainCamera;
	    
	    private void Awake(){
		    _mainCamera = Camera.main;
	    }

	    protected new void Start() {
	        base.Start();
	        
	        // initialize the facing direction based on the starting facing direction
	        _facingDirection = ConvertFacingDirectionToVector(_startingDirection);
	    }

	    /// <summary>
	    /// Gives you the current facing direction of the sprite.
	    /// </summary>
	    public Vector2 GetFacingDirection(){
		    return _facingDirection;
	    }
	    
	    /// <summary>
	    /// Should you want to only move the mouse when you move it, you'd want to set this up as a callback
	    /// using Unity's new Input System.
	    /// </summary>
	    public void LookAtMouse(InputAction.CallbackContext context){
		    CalculateAndLookAtMouse();
	    }


	    protected new void FixedUpdate() {
	        base.FixedUpdate();

	        // if you want to face the mouse no matter what, this will happen!
	        if (_onlyFaceWithMouseInput){
		        CalculateAndLookAtMouse();
	        }
	    }

	    protected Vector2 GetMousePositionInWorldSpace() {
	        // get the mouse's world location, as seen by the camera
	        Vector2 mouseLocation = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

	        // calculate the direction, and the angle at which to face
	        return mouseLocation - (Vector2)transform.position;
	    }
	    
	    /// <summary>
	    /// Wrapper method to do all calculations in 1 go.
	    /// </summary>
	    private void CalculateAndLookAtMouse(){
		    RotateTowardsMouse(GetMousePositionInWorldSpace());    
	    }
	    
	    /// <summary>
	    /// Rotates the current object towards the mouse.
	    /// </summary>
	    private void RotateTowardsMouse(Vector2 directionTowardsMouse) {
		    float angle = Mathf.Atan2(directionTowardsMouse.y, directionTowardsMouse.x) * Mathf.Rad2Deg;

		    // taking initial direction into account, now rotate towards the mouse!
		    transform.rotation = GetRotationForStartingDirection(angle, _startingDirection);
	        
		    // this method will remember the direction you're facing
		    _facingDirection = directionTowardsMouse;
	    }

	    protected Quaternion GetRotationForStartingDirection(float angle, FacingDirection direction) {
	        return Quaternion.AngleAxis((angle + (float)_startingDirection) % 360, Vector3.forward);
	    }

	    private Vector2 ConvertFacingDirectionToVector(FacingDirection direction){
	        switch (direction){
		        case FacingDirection.UP:
			        return Vector2.up;
		        case FacingDirection.DOWN:
			        return Vector2.down;
			    case FacingDirection.LEFT:
				    return Vector2.left;
			    case FacingDirection.RIGHT:
				    return Vector2.right;
	        }

	        return Vector2.zero;
	    }
    }
}