using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalNearPlayer : MonoBehaviour {
    public PlayerMovement player;

    void OnTriggerEnter(Collider other) {
        // player.MetalNearEnter(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        // player.MetalNearExit(other.gameObject);
    }
}
