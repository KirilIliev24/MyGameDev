using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    
    public float damage = 10f;
    float range = 100f;

    public Camera camera;

    public ParticleSystem muzzleFlash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit raycastHit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycastHit, range))
        {
            Debug.Log(raycastHit.transform.name);
            Target target = raycastHit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamege(damage);
            }
        }
    }
}
