using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveMetalObject : MonoBehaviour {

    private Rigidbody rb;
    private PlayerMovement grabber = null;
    private Vector3 grabingSpeed;
    private bool near = false;
    private Vector3 lastGrabbingSpeed = Vector3.zero;
    private int oldLayer;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        oldLayer = gameObject.layer;
    }

    public void HoverEnter() {
        GrabManager.Target(this);
    }

    public void HoverExit() {
        GrabManager.Untarget();
    }

    public void Grab(PlayerMovement cont) {
        grabber = cont;
        oldLayer = gameObject.layer;
        if (rb != null) {
            lastGrabbingSpeed = rb.velocity;
            gameObject.layer = 7; //Metal ignore
            rb.useGravity = false;
        } else {
            lastGrabbingSpeed = Vector3.zero;
        }
    }

    public void UpdateGrab(Vector3 speed, bool n) {
        grabingSpeed = speed;
        near = n;
    }

    public void Release() {
        grabber = null;
        gameObject.layer = oldLayer;
        if (rb != null) {
            rb.useGravity = true;
            if (near) rb.velocity = Vector3.zero;
        }
        near = false;
    }

    public bool MetalMoves() {
        return rb != null;
    }

    void FixedUpdate() {
        if (grabber != null) {
            Vector3 backForce = -lastGrabbingSpeed;
            if (rb != null) backForce += rb.velocity;
            Vector3 minBackForce = GrabManager.GetMinBackForce();
            if (Mathf.Abs(backForce.x) < minBackForce.x) backForce.x = 0;
            if (Mathf.Abs(backForce.y) < minBackForce.z) backForce.y = 0;
            if (Mathf.Abs(backForce.z) < minBackForce.y) backForce.z = 0;
            grabber.AddVelocity(backForce);
            lastGrabbingSpeed = grabingSpeed;
            if (rb != null) {
                rb.velocity = grabingSpeed;
                if (near) rb.angularVelocity = Vector3.zero;
            }
        }
    }

}
