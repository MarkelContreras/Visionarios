using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabManager : MonoBehaviour {

    private static MoveMetalObject target = null;

    public InputActionAsset inputActionAsset;
    public PlayerMovement playerController;
    public float maxRange = 10.0f;
    public float minRange = 0.3f;
    public float speed = 1.0f;

    private MoveMetalObject obj = null;
    private InputAction metalAction;
    private float mult = 0.0f;

    public static void Target(MoveMetalObject obj) {
        target = obj;
    }

    public static void Untarget() {
        target = null;
    }

    private void Awake() {
        inputActionAsset.Enable();
        metalAction = inputActionAsset.FindAction("Metal");
    }

    private void Update() {
        mult = metalAction.ReadValue<float>();
        if (mult != 0 ) {
            if (target != null && obj == null) {
                Vector3 dir = target.transform.position - transform.position;
                Debug.Log(dir.magnitude);
                if (dir.magnitude <= maxRange) {
                    obj = target;
                    obj.Grab(playerController);
                }
            }
        } else if (obj != null) {
            Debug.Log("UnGrab");
            obj.Release();
            obj = null;
        }
    }

    void FixedUpdate() {
        if (obj != null) {
            Vector3 dir = obj.transform.position - transform.position;
            float dist = dir.magnitude;
            Vector3 v = dir.normalized * mult * speed;
            Vector3 comp = obj.transform.position + v;
            float newDist = comp.magnitude;
            if (newDist > maxRange) {
                comp *= (maxRange / newDist);
                v = comp - dir;
            } else if (newDist < minRange) {
                comp *= (minRange / newDist);
                v = comp - dir;
            }
            obj.UpdateGrab(v);
        }
    }
}
