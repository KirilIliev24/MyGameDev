using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    
    public float damage = 10f;
    float range = 100f;
    public float fireRate = 15f;
    public float ammoLeft = 100f;
    public float maxAmmo = 100f;
    public Camera camera;

    private float nextTimeToFire = 0f;

    public ParticleSystem muzzleFlash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        //muzzleFlash.Play();
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
