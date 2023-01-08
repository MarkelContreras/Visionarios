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
    private Vector3 walkVelocity = new Vector3(0, 0, 0);
    private Vector2 moveDir = new Vector2(0, 0);
    private HashSet<GameObject> triggerObjects = new HashSet<GameObject>();

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

    private void Update() {
        moveDir = moveAction.ReadValue<Vector2>();
    }

    public void MetalNearEnter(GameObject other) {
        triggerObjects.Add(other.gameObject);
    }

    public void MetalNearExit(GameObject other) {
        triggerObjects.Remove(other.gameObject);
    }

    public bool MetalNear() {
        return triggerObjects.Contains(GrabManager.GetMovingMetal().gameObject);
    }

    void FixedUpdate() {
        this.inGround = Physics.CheckSphere(this.floorDetector.position, this.floorDistance, this.floor);
        Vector3 v = GetVelocity() - walkVelocity;
        lastPos = transform.position;
        if (inGround) {
            v.x = 0;
            v.y = 0;
            v.z = 0;
        }
        if (pendingVelocity != Vector3.zero) {
            MoveMetalObject metal = GrabManager.GetMovingMetal();
            if (metal != null) {
                if (!MetalNear() || !metal.MetalMoves() || !(GrabManager.GetGrabForce() < 0))
                    v = pendingVelocity;
            }
            pendingVelocity = Vector3.zero;
        } else {
            
        }
        v += Physics.gravity * Time.fixedDeltaTime;
        walkVelocity = (Quaternion.Euler(0, camera.rotation.eulerAngles.y, 0) * new Vector3(moveDir.x, 0, moveDir.y)) * speed;
        this.controller.Move((v + walkVelocity) * Time.fixedDeltaTime);
    }
}
