using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    public Animator animator;

    private float a = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("Running", true);
    }

    public void Die() {
        animator.SetBool("Dead", true);
        gameObject.layer = 7; //Ignore collisions
        LevelManager.EnemyKilled();
    }

    // Update is called once per frame
    void Update()
    {
        a += Time.deltaTime;
        if((int) a % 2 == 0) {
            animator.SetBool("Attacking", true);
        } else {
            animator.SetBool("Attacking", false);
        }
    }
}
