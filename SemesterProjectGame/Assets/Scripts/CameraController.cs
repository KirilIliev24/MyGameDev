using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform player;
    float xRotation = 0f;

    [SerializeField]
    float sensitivity = 150f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // if its += then rotation is flipped
        xRotation -= mouseY;

        //clamp from -90 to 90
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //quarternions are responsible for rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX );
    }
}
