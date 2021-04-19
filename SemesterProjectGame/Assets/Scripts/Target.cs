using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 100;
    public GameObject ammoPickup;
    public HealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(100);
    }

    public void TakeDamege(int damageTaken)
    {
        health -= damageTaken;
        healthBar.SetHealth(health);
        if (health <= 0f)
        {
            Destroy(gameObject);
            //spawn reload pickup
            Instantiate(ammoPickup, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
