using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveMetalObject : MonoBehaviour {

    private Rigidbody rb;
    private PlayerMovement grabber = null;
    private Vector3 grabingSpeed;
    private bool justGrabbed = false;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void HoverEnter() {
        GrabManager.Target(this);
    }

    public void HoverExit() {
        GrabManager.Untarget();
    }

    public void Grab(PlayerMovement cont) {
        grabber = cont;
        justGrabbed = true;
        if (rb != null)     rb.useGravity = false;
    }

    public void UpdateGrab(Vector3 speed) {
        grabingSpeed = speed;
    }

    public void Release() {
        grabber = null;
        if (rb != null) rb.useGravity = true;
    }

    void FixedUpdate() {
        if (grabber != null) {
            if (!justGrabbed) {
                if (rb != null)
                    grabber.AddVelocity(-(grabingSpeed - rb.velocity));
                else
                    grabber.AddVelocity(-grabingSpeed);
            }
            justGrabbed = false;
            if (rb != null) rb.velocity = grabingSpeed;
        }
    }

}
