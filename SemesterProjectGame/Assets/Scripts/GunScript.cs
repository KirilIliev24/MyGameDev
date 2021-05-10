using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunScript : MonoBehaviour
{
    
    public int damage = 10;
    float range = 1000f;
    public float fireRate = 8f;
    private float nextTimeToFire = 0f;

    private const int maxAmmo = 100;
    public int ammoLeft = 100;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public TextMeshProUGUI ammoText;

    public Camera camera;

    public ParticleSystem muzzleFlash;
    private AudioSource audioSource;
    public AudioClip singleGunShot;

    private void Start()
    {
        ammoLeft = maxAmmo;
        audioSource = GetComponent<AudioSource>();
        ammoText.text = $"AMMO : {ammoLeft}";
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = $"AMMO : {ammoLeft}";
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
        muzzleFlash.Play();
        audioSource.PlayOneShot(singleGunShot, 1f);
        ammoLeft--;
        ammoText.text = $"AMMO : {ammoLeft}";
        //Debug.Log($"Ammo left:{ammoLeft}");
        RaycastHit raycastHit;

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out raycastHit, range))
        {
            //Debug.Log(raycastHit.transform.name);
            if(raycastHit.collider.tag == "Enemy")
            {
                var enemy = raycastHit.transform.GetComponent<EnemyScript>();
                enemy.TakeDamege(damage);
            }
            if (raycastHit.collider.tag == "LootCrate")
            {
                var enemy = raycastHit.transform.GetComponent<Target>();
                enemy.TakeDamege(damage);
            }
        }
    }


    private void Reload(int ammo)
    {
        isReloading = true;
       
        if (ammoLeft + ammo >= maxAmmo)
        {
            ammoLeft = maxAmmo;
        }
        else
        {
            ammoLeft += ammo;
        }
        isReloading = false;
        //yield return new WaitForSeconds(reloadTime);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ReloadAmmo"))
        {
            Reload(25);
            //Debug.Log("Reload");
            //Debug.Log($"Ammo after reload:{ammoLeft}");
            Destroy(other.gameObject);
        }
    }
}
