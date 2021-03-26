using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    public GameObject ammoPickup;

    public void TakeDamege(float damageTaken)
    {
        health -= damageTaken;
        if (health <= 0f)
        {
            Destroy(gameObject);
            //spawn reload pickup
            Instantiate(ammoPickup, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
