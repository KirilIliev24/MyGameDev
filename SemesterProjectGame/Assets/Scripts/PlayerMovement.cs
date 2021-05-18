using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCollision;

    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask wallMask;

    public float speed = 10f;

    private Vector3 velocity;
    private float gravity = -18f;
    bool isGrounded;
    bool isOnWall;
    public float jump = 0.5f;

    public int currentHealth;
    private int maxHealth = 200;

    public HealthBar healthBar;

    private int meleeDamege = 5;

    //Animator
    public Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //return true if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCollision.position, groundDistance, groundMask);
        isOnWall = Physics.CheckSphere(groundCollision.position, groundDistance, wallMask);

        if (isGrounded || isOnWall)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }
       

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.fixedDeltaTime);

        velocity.y += gravity * Time.fixedDeltaTime;

        if (Input.GetButton("Jump"))
        {
            if(isGrounded || isOnWall)
            {
                //formula for calculating the velocity for jumping a desired hight
                velocity.y = Mathf.Sqrt(jump * -2 * gravity);
            }
        }

        RaycastHit hit;

        
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("PunchObject");
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2.5f))
            {
                //Debug.DrawRay(transform.position, transform.forward, Color.green);
                if (hit.collider.CompareTag("LootCrate"))
                {
                    Debug.Log($"Destroy crate");

                    var lootCrate = hit.transform.GetComponent<Target>();
                    lootCrate.TakeDamege(meleeDamege);
                }
                if (hit.collider.CompareTag("Enemy"))
                {
                    var enemy = hit.transform.GetComponent<EnemyScript>();
                    enemy.TakeDamege(meleeDamege);
                }
            }
        }
        // aplying delta time again because of the formula (g * t^2)/2
        controller.Move(velocity * Time.fixedDeltaTime);
    }

    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            YouDied();
        }
    }

    private void Heal(int amount)
    {
        if (currentHealth + amount > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
        healthBar.SetHealth(currentHealth);
    }

    private void YouDied()
    {
        SceneManager.LoadScene("EndGameScene");
        PlayerPrefs.SetString("EndGameString", "You DIED");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Heal"))
        {
            Heal(25);
            //Debug.Log("Healed");
            //Debug.Log($"Health after heal:{currentHealth}");
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            TakeDamege(20);
            //Debug.Log("Player took damage");
            //Debug.Log($"Health after damage:{currentHealth}");
            Destroy(collision.gameObject);
        }
    }
}
