using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    
    public float damage = 10f;
    float range = 100f;
    public float fireRate = 10f;
    private float nextTimeToFire = 0f;

    private const int maxAmmo = 100;
    public int ammoLeft = 100;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera camera;

    public ParticleSystem muzzleFlash;

    private void Start()
    {
        ammoLeft = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

       

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            if (ammoLeft <= 0)
            {
                //display no ammo
                return;
            }
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        //muzzleFlash.Play();
        ammoLeft--;
        Debug.Log($"Ammo left:{ammoLeft}");
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


    //call this method from player
    private void Reload(int ammo)
    {
        isReloading = true;
        //yield return new WaitForSeconds(1f);
        if (ammoLeft + ammo >= maxAmmo)
        {
            ammoLeft = maxAmmo;
        }
        else
        {
            ammoLeft = ammoLeft + ammo;
        }
        isReloading = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ReloadAmmo"))
        {
            Reload(25);
            Debug.Log("Reload");
            Debug.Log($"Ammo after reload:{ammoLeft}");
            Destroy(other.gameObject);
        }
    }
}
