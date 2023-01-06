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

    public bool MetalMoves() {
        return rb != null;
    }

    void FixedUpdate() {
        if (grabber != null) {
            if (!justGrabbed) {
                Vector3 backForce = -grabingSpeed;
                if (rb != null) backForce += rb.velocity;
                Vector3 minBackForce = GrabManager.GetMinBackForce();
                if (Mathf.Abs(backForce.x) < minBackForce.x) backForce.x = 0;
                if (Mathf.Abs(backForce.y) < minBackForce.z) backForce.y = 0;
                if (Mathf.Abs(backForce.z) < minBackForce.y) backForce.z = 0;
                grabber.AddVelocity(backForce);
            }
            justGrabbed = false;
            if (rb != null) rb.velocity = grabingSpeed;
        }
    }

}
