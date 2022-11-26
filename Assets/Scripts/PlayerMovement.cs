using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public InputActionReference moveAction;
    public CharacterController controller;
    public float speed = 1f;
    public Transform floorDetector;
    public float floorDistance = 0.1f;
    public LayerMask floor;

    private bool inGround;
    private Vector3 lastPos;
    private Vector3 pendingVelocity = new Vector3(0, 0, 0);
    private Vector2 moveDir = new Vector2(0, 0);

    public void AddVelocity(Vector3 velocity) {
        pendingVelocity += velocity;
    }

    public Vector3 GetVelocity() {
        return (transform.position - lastPos) / Time.fixedDeltaTime;
    }

    void Start() {
        moveAction.action.started += moveActionStarted;
        //moveAction.action.stopped += moveActionStopped;
    }

    void moveActionStarted(InputAction.CallbackContext context) {
        moveDir = context.ReadValue<Vector2>();
    }

    void moveActionStopped(InputAction.CallbackContext context) {
        moveDir = Vector2.zero;
    }

    void FixedUpdate() {
        this.inGround = Physics.CheckSphere(this.floorDetector.position, this.floorDistance, this.floor);
        Vector3 v = GetVelocity();
        lastPos = transform.position;
        v += Physics.gravity * Time.fixedDeltaTime;
        v += pendingVelocity;
        if (inGround) {
            v.x = 0;
            v.y = 0;
            v.z = 0;
        }
        v.x = moveDir.x * speed;
        v.z = moveDir.y * speed;
        pendingVelocity = new Vector3(0, 0, 0);
        this.controller.Move(v * Time.fixedDeltaTime);
    }
}
