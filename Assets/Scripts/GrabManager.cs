using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabManager : MonoBehaviour {

    private static MoveMetalObject target = null;
    private static Vector3 minBackForceMult = Vector3.zero;
    private static float mult = 0.0f;
    private static MoveMetalObject obj = null;
    private static bool shoudlSpawnCoin = false;

    public InputActionAsset inputActionAsset;
    public PlayerMovement playerController;
    public Vector3 minBackForce = Vector3.zero;
    public float maxRange = 10.0f;
    public float minRange = 0.05f;
    public float speed = 1.0f;
    public MetalNearPlayer near;
    public CoinSpawner coinSpawner;
    public XRRayInteractor xrInteractor;

    private GameObject grabPoint;
    private UnityEngine.InputSystem.InputAction metalAction;
    private bool coinSpawned = false;
    private int objOldLayer;

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

    public static MoveMetalObject GetTargetMetal() {
        return target;
    }

    public static float GetGrabForce() {
        return mult;
    }

    private void Awake() {
        Vector3 currentPos = transform.position;
        transform.parent = xrInteractor.rayOriginTransform;
        transform.position = currentPos;
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
                        obj.Grab(playerController);
                        grabPoint = new GameObject("GrabPoint");
                        grabPoint.transform.position = obj.gameObject.transform.position;
                        grabPoint.transform.parent = obj.gameObject.transform;
                        objOldLayer = obj.gameObject.layer;
                        obj.gameObject.layer = 7;
                    }
                } else if (target != null) {
                    Vector3 dir = target.transform.position - transform.position;
                    if (dir.magnitude <= maxRange && (!target.MetalMoves() || !near.IsNearTarget() || mult > 0)) {
                        RaycastHit hit;
                        xrInteractor.TryGetCurrent3DRaycastHit(out hit);
                        obj = target;
                        obj.Grab(playerController);
                        grabPoint = new GameObject("GrabPoint");
                        grabPoint.transform.position = hit.point;
                        grabPoint.transform.parent = obj.gameObject.transform;
                        if (obj.MetalMoves()) {
                            objOldLayer = obj.gameObject.layer;
                            obj.gameObject.layer = 7;
                        }
                    }
                }
            }
        } else if (obj != null) {
            if (obj.MetalMoves()) near.RestoreLayer(obj.gameObject, objOldLayer);
            Destroy(grabPoint);
            obj.Release();
            grabPoint = null;
            obj = null;
        } else {
            coinSpawned = false;
        }
    }

    void FixedUpdate() {
        if (obj != null) {
            Vector3 dir = grabPoint.transform.position - transform.position;
            float step = mult * speed;
            float dist = dir.magnitude;
            if (dist >= maxRange && mult > 0) {
                obj.UpdateGrab(Vector3.zero, false);
            } else if (mult < 0 && ((obj.MetalMoves() && dist <= Mathf.Abs((step) * Time.fixedDeltaTime)) || (!obj.MetalMoves() && near.IsNearGrabbed()))) {
                obj.UpdateGrab(Vector3.zero, true);
            } else {
                obj.UpdateGrab(dir.normalized * step, false);
            }
        }
    }
}
