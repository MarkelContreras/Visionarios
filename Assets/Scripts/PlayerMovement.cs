using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

    public InputActionAsset inputActionAsset;
    public CharacterController controller;
    public Transform camera;
    public float speed = 1f;
    public Transform floorDetector;
    public float floorDistance = 0.1f;
    public LayerMask floor;

    private InputAction moveAction;
    private bool inGround;
    private Vector3 lastPos;
    private Vector3 pendingVelocity = new Vector3(0, 0, 0);
    private Vector2 moveDir = new Vector2(0, 0);

    private void Awake() {
        inputActionAsset.Enable();
        moveAction = inputActionAsset.FindAction("Move");
        lastPos = transform.position;
    }

    public void AddVelocity(Vector3 velocity) {
        pendingVelocity += velocity;
        Debug.Log(pendingVelocity);
    }

    public Vector3 GetVelocity() {
        return (transform.position - lastPos) / Time.fixedDeltaTime;
    }

    private void Update() {
        moveDir = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        this.inGround = Physics.CheckSphere(this.floorDetector.position, this.floorDistance, this.floor);
        Vector3 v = GetVelocity();
        lastPos = transform.position;
        v += Physics.gravity * Time.fixedDeltaTime;
        if (inGround) {
            v.x = 0;
            v.y = 0;
            v.z = 0;
        }
        v += pendingVelocity;
        v += (Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0) * new Vector3(moveDir.x, 0, moveDir.y)) * speed;
        pendingVelocity = new Vector3(0, 0, 0);
        this.controller.Move(v * Time.fixedDeltaTime);
    }
}
