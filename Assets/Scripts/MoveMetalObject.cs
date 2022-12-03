using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMetalObject : MonoBehaviour {

    //public CharacterController controller;
    //public float drag = 0.7f;


    private Rigidbody rb;
    private PlayerMovement grabber = null;
    private Vector3 grabingSpeed;
    private bool justGrabbed = false;
    //private bool touchingSurface = true;
    //private Vector3 lastPos;
    //private Vector3 pendingVelocity = new Vector3(0, 0, 0);

    // public void AddVelocity(Vector3 velocity) {
    //     pendingVelocity += velocity;
    // }
    //
    // public Vector3 GetVelocity() {
    //     return (transform.position - lastPos) / Time.fixedDeltaTime;
    // }

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab(PlayerMovement cont) {
        grabber = cont;
        justGrabbed = true;
        rb.useGravity = false;
    }

    public void UpdateGrab(Vector3 speed) {
        grabingSpeed = speed;
    }

    public void Release() {
        grabber = null;
        rb.useGravity = true;
    }

    // void OnCollisionEnter(Collision collision) {
    //     touchingSurface = true;
    // }
    //
    // void OnCollisionExit(Collision collision) {
    //     touchingSurface = false;
    // }

    void FixedUpdate() {
        // Vector3 v = GetVelocity();
        // lastPos = transform.position;
        // if (grabber == null) {
        //     v += Physics.gravity;
        // } else {
        //     grabber.AddVelocity(grabbingSpeed - v);
        // }
        // v += pendingVelocity;
        // if (touchingSurface) {
        //     v.x -= v.x * drag;
        //     v.y -= v.y * drag;
        //     v.z -= v.z * drag;
        // }
        // pendingVelocity = new Vector3(0, 0, 0);
        // this.controller.Move(v * Time.fixedDeltaTime);
        // )

        if (grabber != null && !justGrabbed) {
            if (!justGrabbed) {
                grabber.AddVelocity(grabingSpeed - GetComponent<Rigidbody>().velocity);
            }
            justGrabbed = false;
            rb.velocity = grabingSpeed;
        }
    }

}
