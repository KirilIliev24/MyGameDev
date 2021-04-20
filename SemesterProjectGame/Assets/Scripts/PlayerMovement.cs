using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCollision;

    public GameObject gunObject;
    
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 10f;

    private Vector3 velocity;
    private float gravity = -18f;
    bool isGrounded;
    public float jump = 0.5f;

    public int currentHealth;
    private int maxHealth = 200;

    public HealthBar healthBar;


    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //return true if the player is on the ground
        isGrounded = Physics.CheckSphere(groundCollision.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (Input.GetButton("Jump") && isGrounded)
        {
            //formula for calculating the velocity for jumping a desired hight
            velocity.y = Mathf.Sqrt(jump * -2 * gravity);
        }

        if(Input.GetMouseButtonDown(1))
        {
            TakeDamege(20);
        }

        // aplying delta time again because of the formula (g * t^2)/2
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamege(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if(currentHealth <= 0)
        {
            //do some failed screen
        }
    }    
}
