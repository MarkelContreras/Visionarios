using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalNearPlayer : MonoBehaviour {

    private HashSet<GameObject> triggerObjects = new HashSet<GameObject>();
    private Dictionary<GameObject, int> layersToRestore = new Dictionary<GameObject, int>();

    void OnTriggerEnter(Collider other) {
        Debug.Log("enter");
        triggerObjects.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        Debug.Log("exit");
        triggerObjects.Remove(other.gameObject);
        if (layersToRestore.ContainsKey(other.gameObject)) {
            other.gameObject.layer = layersToRestore[other.gameObject];
            layersToRestore.Remove(other.gameObject);
        }
    }

    public bool IsNearTarget() {
        return triggerObjects.Contains(GrabManager.GetTargetMetal().gameObject);
    }

    public bool IsNearGrabbed() {
        Debug.Log(triggerObjects.Contains(GrabManager.GetMovingMetal().gameObject));
        return triggerObjects.Contains(GrabManager.GetMovingMetal().gameObject);
    }

    public void RestoreLayer(GameObject obj, int oldLayer) {
        if (triggerObjects.Contains(obj)) {
            layersToRestore.Add(obj, oldLayer);
        } else {
            obj.layer = oldLayer;
        }
    }
}
