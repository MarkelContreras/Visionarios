using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour {

    void OnCollisionEnter(Collision collision) {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            if (collision.relativeVelocity.magnitude >= 6) enemy.Die();
        }
    }
}
