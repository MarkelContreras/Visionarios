using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour {
    
    public GameObject character;

    void OnTriggerEnter(Collider other) {
        if (other.gameObject == character) {
            LevelManager.Die();
        }
    }

}
