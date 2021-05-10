using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void Awake()
    {
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Rotate(new Vector3(0, 200, 0) * Time.deltaTime);
        //gameObject.transform.Rotate(gameObject.transform.up, Time.deltaTime * 90f);
        //transform.RotateAround(transform.position, transform.up, Time.deltaTime);
        transform.rotation = Quaternion.AngleAxis(5, Vector3.up) * transform.rotation;
    }
}
