using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example of a script that gets it's facing direction from the velocity defined by it's rigid body.
/// 
/// NOTE: does work with 3D but does not read the 'Z' values for now.
/// </summary>
public class RBAnimationController : AbstractAnimatorController {

    [Tooltip("If true, uses a 2D RigidBody component for direction, if false uses a 3D RigidBody.")]
    [SerializeField] private bool _is2D = true;

    private Rigidbody2D _rigidBody2D;
    private Rigidbody _rigidBody;

    protected new void Start() {
        base.Start();

        if (_is2D) {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        } else {
            _rigidBody = GetComponent<Rigidbody>();
        }
    }

    public override Vector2 GetDirection() {
        if (_is2D) {
            return _rigidBody2D.velocity;
        } else {
            return _rigidBody.velocity;
        }
    }
}
