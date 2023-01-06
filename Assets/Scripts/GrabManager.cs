using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabManager : MonoBehaviour {

    private static MoveMetalObject target = null;
    private static Vector3 minBackForceMult = Vector3.zero;

    public InputActionAsset inputActionAsset;
    public PlayerMovement playerController;
    public Vector3 minBackForce = Vector3.zero;
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

    public static Vector3 GetMinBackForce() {
        return minBackForceMult;
    }

    private void Awake() {
        inputActionAsset.Enable();
        metalAction = inputActionAsset.FindAction("Metal");
    }

    private void Update() {
        mult = metalAction.ReadValue<float>();
        minBackForceMult = minBackForce * Mathf.Abs(mult);
        if (mult != 0 ) {
            if (target != null && obj == null) {
                Vector3 dir = target.transform.position - transform.position;
                if (dir.magnitude <= maxRange) {
                    obj = target;
                    obj.Grab(playerController);
                }
            }
        } else if (obj != null) {
            obj.Release();
            obj = null;
        }
    }

    void FixedUpdate() {
        if (obj != null) {
            Vector3 dir = obj.transform.position - transform.position;
            float dist = dir.magnitude;
            // Vector3 v = dir.normalized * mult * speed;
            // Vector3 comp = obj.transform.position + v;
            // float newDist = (comp - transform.position).magnitude;
            // if (newDist > maxRange) {
            //     Debug.Log("Far");
            //     comp *= (maxRange / newDist);
            //     v = comp - dir;
            // } else if (newDist < minRange) {
            //     Debug.Log("Near");
            //     comp *= (minRange / newDist);
            //     v = comp - dir;
            // }
            // obj.UpdateGrab(v);
            if (dist > maxRange) {
                obj.Release();
                obj = null;
            } else if (dist <= minRange && mult < 0) {
                obj.UpdateGrab(Vector3.zero);
            } else {
                obj.UpdateGrab(dir.normalized * mult * speed);
            }
        }
    }
}
