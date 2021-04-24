﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int health = 50;
    public GameObject ammoPickup;
    public GameObject healPickup;
    public HealthBar healthBar;

    void Start()
    {
        healthBar.SetMaxHealth(100);
    }

    private void SpawnLoot()
    {
        int itemDrop = Random.Range(0, 100);
        if (itemDrop < 50)
        {
            //spawn reload pickup
            Instantiate(ammoPickup, gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            //spawn heal pickup
            Instantiate(healPickup, gameObject.transform.position, Quaternion.identity);
        }
    }

    public void TakeDamege(int damageTaken)
    {
        health -= damageTaken;
        healthBar.SetHealth(health);
        if (health <= 0f)
        {
            Destroy(gameObject);
            SpawnLoot();
        }
    }
}
