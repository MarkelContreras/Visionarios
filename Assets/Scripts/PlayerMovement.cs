using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public CharacterController controller;
    public float speed = 1f;
    public Transform floorDetector;
    public float floorDistance = 0.1f;
    public LayerMask floor;

    private bool inGround;
    private Vector3 lastPos;
    private Vector3 pendingVelocity = new Vector3(0, 0, 0);

    public void AddVelocity(Vector3 velocity) {
        pendingVelocity += velocity;
    }

    public Vector3 GetVelocity()) {
        return (transform,position - lastPos) / Time.fixedDeltaTime;
    }

    void Start() {
        
    }

    void FixedUpdate() {
        this.inGround = Physics.CheckSphere(this.floorDetector.position, this.floorDistance, this.floor);
        flaot x = 0; //get x from input
        float y = 0; //get y from input
        Vector3 v = getVelocity();
        lastPos = transform.position;
        v += Physics.gravity;
        v += pendingVelocity;
        if (inGround) {
            v.x = 0;
            v.y = 0;
            v.z = 0;
        }
        v.x = x * speed;
        v.y = y * speed;
        pendingVelocity = new Vector3(0, 0, 0);
        this.controller.Move(v * Time.fixedDeltaTime);
    }
}
