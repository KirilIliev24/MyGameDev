using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    //Health
    public int health = 100;
    public HealthBar healthBar;

    //Damage
    public int damage = 10;

    //Loot
    public GameObject ammoPickup;

    //AI
    public NavMeshAgent agent;
    public LayerMask playerMask, groundMask;
    public Transform player;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //if (!agent.isOnNavMesh)
        //{
        //    transform.position = transform.position;
        //    agent.enabled = false;
        //    agent.enabled = true;
        //}
        healthBar.SetMaxHealth(100);
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if player is in range
        playerInSight = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if(!playerInSight && !playerInAttackRange)
        {
            Patroling();
        }
        if (playerInSight && !playerInAttackRange)
        {
            Chasing();
        }
        if (playerInSight && playerInAttackRange)
        {
            Attacking();
        }

    }

    private void Patroling()
    {
        if(!walkPointSet)
        {
            SearchForWalkPoint();
        }
        else 
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Reached destination
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchForWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        //Check if point is on the ground
        if(Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            walkPointSet = true;
        }
    
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        //Stop mooving
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if(!alreadyAttacked)
        {
            //Fix attack code
            //Attack code
            //Vector3 shootFrom = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
            //Rigidbody rigidbody = Instantiate(projectile, shootFrom, Quaternion.identity).GetComponent<Rigidbody>();
            //rigidbody.AddForce(transform.forward * 32, ForceMode.Acceleration);
            //rigidbody.AddForce(transform.up * 8, ForceMode.Acceleration);
            ////
            //Destroy(rigidbody, 1);
            alreadyAttacked = true;
           
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamege(int damageTaken)
    {
        health -= damageTaken;
        healthBar.SetHealth(health);
        if (health <= 0f)
        {
            //Die Animation

            //Destroy object
            Destroy(gameObject);

            SpawnLoot();
        }
    }

    private void SpawnLoot()
    {
        //spawn reload pickup
        Instantiate(ammoPickup, gameObject.transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
