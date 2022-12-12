using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour {

    private static PlayerMovement controller;
    private static MoveMetalObject grabbedObject = null;

    public GameObject playerGrabber;
    public PlayerMovement playerController;
    public float maxRange = 10.0f;

    public static void Grab(MoveMetalObject obj) {
        if (grabbedObject == null) {
            grabbedObject = obj;
            obj.Grab(controller);
        }
    }

    public static void Release() {
        if (grabbedObject != null) {
            grabbedObject.Release();
        }
    }

    void Awake() {
        controller = this.playerController;
    }

    void FixedUpdate() {
        if (grabbedObject != null) {
            Vector3 dir = grabbedObject.transform.position - transform.position;
            float dist = dir.magnitude;
        }
    }
}
