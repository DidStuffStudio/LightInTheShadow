
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private bool checkingPath;
   // public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    //public float timeBetweenAttacks;
    //bool alreadyAttacked;
    //public GameObject projectile;

    //States
    public float sightRange;//, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public bool canKill = true;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange) Patroling();
        if (playerInSightRange) ChasePlayer();
        //if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // //Calculate random point in range
         float randomZ = Random.Range(-walkPointRange, walkPointRange);
         float randomX = Random.Range(-walkPointRange, walkPointRange);
        //
        var randomPos = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //
        //
        // if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        //     walkPointSet = true;

        //Vector3 randomDirection = Random.insideUnitSphere * walkPointRange;
        //randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, walkPointRange, 1))
        {
            walkPoint = hit.position;
            walkPointSet = true;
            StartCoroutine(TimeToNextPath());
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    
    IEnumerator TimeToNextPath()
    {
        yield return new WaitForSeconds(2.0f);
        if (!agent.hasPath)
            SearchWalkPoint();
    }

    // private void AttackPlayer()
    // {
    //     //Make sure enemy doesn't move
    //     agent.SetDestination(transform.position);
    //
    //     transform.LookAt(player);
    //
    //     if (!alreadyAttacked)
    //     {
    //         ///Attack code here
    //         Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
    //         rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
    //         rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    //         ///End of attack code
    //
    //         alreadyAttacked = true;
    //         Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //     }
    // }
    // private void ResetAttack()
    // {
    //     alreadyAttacked = false;
    // }

    // public void TakeDamage(int damage)
    // {
    //     health -= damage;
    //
    //     if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    // }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!canKill) return;
        if (other.gameObject.layer == 10)
        {
           player.transform.position = player.GetComponent<playerController>().respawnLocation;
        }
    }
}
