using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{
    public float mouseSensitivity = 800f;
    public Transform playerBody;
    float xRotation = 0f;
    
    public void calculateRotations()
    {
        float effectiveSpeed = mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * effectiveSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * effectiveSpeed;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 15f); //rotatia pe axa este blocata pentru a nu se putea vedea in interiorul personajului

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        calculateRotations();
    }
}
