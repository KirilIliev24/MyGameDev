using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    //Health
    [SerializeField]
    public int health = 100;
    public HealthBar healthBar;

    //Damage
    [SerializeField]
    public int damage = 10;

    //Loot
    public GameObject ammoPickup;
    public GameObject healPickup;

    //AI
    public NavMeshAgent agent;
    public LayerMask playerMask, groundMask;
    public Transform player;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    [SerializeField]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public GameObject shootPoint;
    private bool ableToShootPlayer = false;

    //States
    [SerializeField]
    public float sightRange, attackRange;
    [SerializeField]
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

       

        if (!playerInSight && !playerInAttackRange)
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

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.transform.position, transform.forward, out hit, 20f))
        {
            if(hit.collider.tag == "Player")
            {
                Debug.Log($"Able to shoot player: {ableToShootPlayer}");
                ableToShootPlayer = true;
            }
        }
        else
        {
            ableToShootPlayer = false;
        }

        if (!alreadyAttacked && ableToShootPlayer)
        {
            //Attack code
            Rigidbody rigidbody = Instantiate(projectile, shootPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.forward * 32, ForceMode.Impulse);
            rigidbody.AddForce(transform.up * 4, ForceMode.Impulse);
            ////
            Destroy(rigidbody.gameObject, 1f);
            alreadyAttacked = true;
           
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
        ableToShootPlayer = false;
        Debug.Log($"Able to shoot player: {ableToShootPlayer}");
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
        int itemDrop = Random.Range(0, 100);
        if(itemDrop < 50)
        {
            //spawn reload pickup
            Instantiate(ammoPickup, gameObject.transform.position, ammoPickup.transform.rotation);
        }
        else
        {
            //spawn heal pickup
            Instantiate(healPickup, gameObject.transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
