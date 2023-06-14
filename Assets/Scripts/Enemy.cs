using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public GameObject target;
    public NavMeshAgent nav;
    public Animator animator;
    public Transform[] wonderingDestinations;
    public Transform[] fleeDestinations;

    public float fov = 90f;
    public float viewDistance = 10f;
    public float attackDistance = 1f;
    public float walkingSpeed = 2f;
    public float chasingSpeed = 4f;
    
    private Vector3 destination;
    private bool hasDestination = false;
    private bool isFleeing = false;
    public bool hasRotation = false;
    private int wonderCount = 0;
    private bool canMove = true;
    private bool chasing = false;
    private bool isDead = false;
    private bool attackable = false;
    private bool attacking = false;
    private bool justAttacked = false;
    private List<Vector3> wonder = new List<Vector3>();

    public void Die() {
        animator.SetBool("Dead", true);
        gameObject.layer = 7; //Ignore collisions
        isDead = true;
        nav.destination = transform.position;
        LevelManager.EnemyKilled();
    }

    private bool InDistanceAndView(float distance) {
        if (Vector3.Distance(transform.position, target.transform.position) <= distance) {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(target.transform.position - transform.position, transform.forward) <= fov) {
                RaycastHit hit;
                Physics.Raycast(transform.position, target.transform.position - transform.position, out hit);
                if (hit.transform != null && hit.transform.gameObject.Equals(target)) return true;
            }
        }
        return false;
    }

    private void LastCheckAttack() {
        if (InDistanceAndView(attackDistance)) LevelManager.Die();
    }

    private float ScoreToFleeingPoint(Vector3 fleeingPosition, Vector3 playerPosition) {
        Vector3 posDirection = Vector3.Normalize(fleeingPosition - transform.position);
        Vector3 playerToPosition = transform.position - playerPosition;
        Vector3 playerToClosestPosInLine = playerToPosition - Vector3.Dot(playerToPosition, posDirection) * posDirection;
        return playerToClosestPosInLine.magnitude;
    }

    private Vector3 SetNextFleeingPoint() {
        float score = -1f;
        Vector3 bestPos = Vector3.zero;
        foreach(Transform t in fleeDestinations) {
            Vector3 posToT = t.position - transform.position;
            if (posToT.magnitude < 1f){
                continue;
            }
            float currentScore = ScoreToFleeingPoint(t.position, target.transform.position);
            if(currentScore > score) {
                score = currentScore;
                bestPos = t.position;
            }
        }
        return bestPos;
    }

    void Awake() {
        if (wonderingDestinations.Length == 0) {
            GameObject obj = new GameObject(gameObject.name + "_wonder");
            obj.transform.position = transform.position;
            wonderingDestinations = new Transform[] {obj.transform};
        }
        if (wonderingDestinations.Length == 1) {
            hasDestination = true;
            destination = wonderingDestinations[0].position;
        }
    }

    void Update() {
        if (isDead) return;
        hasRotation = true;
        attackable = InDistanceAndView(attackDistance);
        justAttacked = false;
        if (attacking) {
            attacking = false;
            justAttacked = true;
        }
        if (!hasDestination) {
            if (isFleeing) {
                destination  = SetNextFleeingPoint();
                hasDestination = true;
                nav.stoppingDistance = 0;
                nav.speed = chasingSpeed;
            } 
            if (wonderingDestinations.Length > 1 || chasing) {
                if (chasing) wonderCount = -1;
                wonderCount = (wonderCount + 1) % wonderingDestinations.Length;
                destination = wonderingDestinations[wonderCount].position;
                hasDestination = true;
                chasing = false;
                nav.stoppingDistance = 0f;
                nav.speed = walkingSpeed;
            } else {
                Vector3 d = wonderingDestinations[0].forward;
                d.y = 0;
                float angle = Vector3.Angle(d, transform.forward);
                hasRotation = angle != 0;
                if (hasRotation) {
                    Quaternion rot = Quaternion.LookRotation(d);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rot, (nav.angularSpeed * Time.deltaTime) / angle);
                }
            }
        } else {
            if (canMove) {
                nav.destination = destination;
            } else {
                nav.destination = InDistanceAndView(attackDistance - 1f) ? transform.position : target.transform.position;
            }
            if (!nav.pathPending && canMove) {
                if (nav.remainingDistance <= nav.stoppingDistance) {
                    if (!nav.hasPath || nav.velocity.sqrMagnitude == 0f) {
                        hasDestination = false;
                    }
                }
            }
        }
        if (InDistanceAndView(viewDistance)) {
            hasDestination = true;
            if (fleeDestinations.Length >= 2) {
                isFleeing = true;
                destination = SetNextFleeingPoint();
                nav.stoppingDistance = 0;
            } else {
                chasing = true;
                destination = target.transform.position;
                nav.stoppingDistance = attackDistance - 1;
            }
            nav.speed = chasingSpeed;
        }
        if (attackable && !isFleeing) {
            hasDestination = true;
            chasing = true;
            canMove = false;
            if (!justAttacked) attacking = true;
            Invoke(nameof(LastCheckAttack), 0.5f);
        } else {
            canMove = true;
        }
        animator.SetBool("Walking", hasDestination || hasRotation);
        animator.SetBool("Running", chasing || isFleeing);
        animator.SetBool("Attacking", attacking);
    }
}
