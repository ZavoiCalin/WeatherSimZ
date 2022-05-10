using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public CharacterController controller; 
    public float speed = 120f; 

    public void calculateMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float effectiveMovementSpeed = speed * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * effectiveMovementSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
    }
}
