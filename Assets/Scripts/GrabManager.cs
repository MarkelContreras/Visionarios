using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabManager : MonoBehaviour {

    private static MoveMetalObject target = null;
    private static Vector3 minBackForceMult = Vector3.zero;
    private static float mult = 0.0f;
    private static MoveMetalObject obj = null;
    private static bool shoudlSpawnCoin = true;

    public InputActionAsset inputActionAsset;
    public PlayerMovement playerController;
    public Vector3 minBackForce = Vector3.zero;
    public float maxRange = 10.0f;
    public float minRange = 0.3f;
    public float speed = 1.0f;
    public CoinSpawner coinSpawner;

    private InputAction metalAction;
    private bool coinSpawned = false;

    public static void Target(MoveMetalObject obj) {
        target = obj;
    }

    public static void Untarget() {
        target = null;
    }

    public static void TargetCoinSpawner() {
        shoudlSpawnCoin = true;
    }

    public static void UntargetCoinSpawner() {
        shoudlSpawnCoin = false;
    }

    public static Vector3 GetMinBackForce() {
        return minBackForceMult;
    }

    public static MoveMetalObject GetMovingMetal() {
        return obj;
    }

    public static float GetGrabForce() {
        return mult;
    }

    private void Awake() {
        inputActionAsset.Enable();
        metalAction = inputActionAsset.FindAction("Metal");
    }

    private void Update() {
        mult = metalAction.ReadValue<float>();
        minBackForceMult = minBackForce * Mathf.Abs(mult);
        if (mult != 0 ) {
            if (obj == null) {
                if (shoudlSpawnCoin) {
                    if (mult < 0 && !coinSpawned) {
                        coinSpawned = true;
                        obj = coinSpawner.SpawnCoin();
                    }
                } else if (target != null) {
                    Vector3 dir = target.transform.position - transform.position;
                    if (dir.magnitude <= maxRange) {
                        obj = target;
                        obj.Grab(playerController);
                    }
                }
            }
            
        } else if (obj != null) {
            obj.Release();
            obj = null;
        } else {
            coinSpawned = false;
        }
    }

    void FixedUpdate() {
        if (obj != null) {
            Vector3 dir = obj.transform.position - transform.position;
            float dist = dir.magnitude;
            if (dist >= maxRange && mult > 0) {
                obj.UpdateGrab(Vector3.zero);
            } else {
                obj.UpdateGrab(dir.normalized * mult * speed);
            }
        }
    }
}
