using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles somewhat standardized communication between your current 'moving direction'
/// and the Animator Controller attached to your object. NOTE: this probably can be used
/// for 2D and 3D together, perhaps adding more to this for the 'Z' direction.
/// 
/// NOTE: you can change the values that your Animator Controller uses for some flexibility.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class AbstractAnimatorController : MonoBehaviour {
    [Tooltip("Velocities in either x/y less than this value, are truncated to 0 for the Animator Controller.")]
    [SerializeField] private float _velocityThreshold = 1f;

    // these will be directly input into the Animator based on your implementation of 'GetDirection'
    [Tooltip("Animation Controller parameter for 'X' input direction.")]
    [SerializeField] private string _xDirectionVar = "MoveX";
    [Tooltip("Animation Controller parameter for 'Y' input direction.")]
    [SerializeField] private string _yDirectionVar = "MoveY";

    // these represent your last moved location, to keep you facing in that direction
    [Tooltip("Animation Controller parmameter name, used to track last moved direction on 'X'.")]
    [SerializeField] private string _xLastVar = "LastMoveX";
    [Tooltip("Animation Controller parmameter name, used to track last moved direction on 'Y'.")]
    [SerializeField] private string _yLastVar = "LastMoveY";

    private Animator _animator;

    protected void Start() {
        _animator = GetComponent<Animator>();
    }

    protected void FixedUpdate() {
        Vector2 currentDirection = AdjustForVelocityThreshold(GetDirection(), _velocityThreshold);

        SetAnimatorParameters(currentDirection);

        // adds some expandability to this script! Sets any additional parameters you want.
        SetAdditionalParameters();
    }

    /// <summary>
    /// The source of direction for the Animation Controller.
    /// TODO: consider making this an Action and using whatever action you set instead of using inheritance.
    /// </summary>
    public abstract Vector2 GetDirection();

    /// <summary>
    /// Override this method if you want to set additional parameters using this controller structure.
    /// </summary>
    public virtual void SetAdditionalParameters() {

    }

    /// <summary>
    /// If x is lower than threshold, truncate to 0.
    /// If y is lower than threshold, truncate to 0.
    /// </summary>
    private Vector2 AdjustForVelocityThreshold(Vector2 direction, float threshold) {
        // we want to tell the animator to stop 'moving' once you're past this velocity threshold
        if (Mathf.Abs(direction.x) <= _velocityThreshold) {
            direction.x = 0;
        }

        if (Mathf.Abs(direction.y) <= _velocityThreshold) {
            direction.y = 0;
        }

        return direction;
    }

    /// <summary>
    /// Sets the parameters for the direction you're currently going, and the direction
    /// you had just come from. This allows for movement/idle animations to be setup
    /// relatively easy.
    /// </summary>
    private void SetAnimatorParameters(Vector2 direction) {
        Vector2 normalized = direction.normalized;

        // this tells the animator which direction you're moving in so it can choose the right animation
        _animator.SetFloat(_xDirectionVar, direction.normalized.x);
        _animator.SetFloat(_yDirectionVar, direction.normalized.y);

        // is there any input at all? Then we don't want to wipe the last data
        if (normalized == Vector2.zero) {
            return;
        }

        // always store the last location you've moved in
        _animator.SetFloat(_xLastVar, normalized.x);
        _animator.SetFloat(_yLastVar, normalized.y);
    }
}