using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject objective;

    [SerializeField] private float chaseRadius = 5f;
    [SerializeField] private float attackRadius = 0.5f;

    [SerializeField] private float attackTime = 0.5f;

    bool attacking = false;

    private void Awake()
    {
        if (navMeshAgent == null)
        {
            navMeshAgent= GetComponent<NavMeshAgent>();
        }
    }

    private void Update()
    {
        if (objective != null)
        {
            var distance = Vector3.Distance(transform.position, objective.transform.position);
            RaycastHit hit;
            Physics.Raycast(transform.position, objective.transform.position - transform.position, out hit);
            Debug.DrawRay(transform.position, objective.transform.position - transform.position);
            Debug.Log(hit.transform.gameObject);
            if (distance <= chaseRadius && (hit.transform != null) && (hit.transform.gameObject.Equals(objective)))

            {
                if (distance > attackRadius)
                {
                    navMeshAgent.destination = objective.transform.position;
                } 
                
                else
                {
                    navMeshAgent.destination = transform.position;

                    if (!attacking)
                    {
                        attacking = true;
                        Invoke(nameof(Attack), attackTime);
                    }
                }
            }
           else
            {
                navMeshAgent.destination = transform.position;
            }

            Debug.Log(distance);
        }
    }

    private void Attack()
    {
        if (Vector3.Distance(transform.position, objective.transform.position) <= attackRadius)
        {
            Destroy(objective);
            Invoke(nameof(ReloadScene), 1f);
        }
        attacking = false;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
