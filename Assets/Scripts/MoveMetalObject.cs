using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMetalObject : MonoBehaviour {

    private Rigidbody rb;
    private PlayerMovement grabber = null;
    private Vector3 grabingSpeed;
    private bool justGrabbed = false;

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

    void FixedUpdate() {
        if (grabber != null) {
            if (!justGrabbed) {
                grabber.AddVelocity(grabingSpeed - rb.velocity);
            }
            justGrabbed = false;
            rb.velocity = grabingSpeed;
        }
    }

}
