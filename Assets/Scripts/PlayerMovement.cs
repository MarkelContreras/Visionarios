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

    private UnityEngine.InputSystem.InputAction moveAction;
    private bool inGround;
    private Vector3 lastPos;
    private Vector3 pendingVelocity = new Vector3(0, 0, 0);
    private Vector3 walkVelocity = new Vector3(0, 0, 0);
    private Vector2 moveDir = new Vector2(0, 0);
    private float vYCap = 0;

    private void Awake() {
        inputActionAsset.Enable();
        moveAction = inputActionAsset.FindAction("Move");
        lastPos = transform.position;
    }

    public void AddVelocity(Vector3 velocity) {
        pendingVelocity += velocity;
    }

    public Vector3 GetVelocity() {
        return (transform.position - lastPos) / Time.fixedDeltaTime;
    }

    public void Warp(Transform t) {
        controller.enabled = false;
        controller.transform.position = t.position;
        controller.transform.rotation = t.rotation;
        controller.enabled = true;
        lastPos = t.position;
    }

    private void Update() {
        moveDir = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        this.inGround = Physics.CheckSphere(this.floorDetector.position, this.floorDistance, this.floor);
        Vector3 v = GetVelocity() - walkVelocity;
        lastPos = transform.position;
        if (inGround || (GrabManager.GetMovingMetal() != null && !GrabManager.GetMovingMetal().MetalMoves())) {
            v.x = 0;
            v.y = 0;
            v.z = 0;
        }
        if (pendingVelocity != Vector3.zero) {
                v = pendingVelocity;
            pendingVelocity = Vector3.zero;
        } else if (GrabManager.GetMovingMetal() == null || GrabManager.GetMovingMetal().MetalMoves()) {
            v += Physics.gravity * Time.fixedDeltaTime;
        }
        if ((GrabManager.GetMovingMetal() != null && !GrabManager.GetMovingMetal().MetalMoves()) || inGround || v.y < vYCap) {
            vYCap = v.y;
        } else {
            v.y = vYCap;
        }
        walkVelocity = (Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0) * new Vector3(moveDir.x, 0, moveDir.y)) * speed;
        this.controller.Move((v + walkVelocity) * Time.fixedDeltaTime);
    }
}
