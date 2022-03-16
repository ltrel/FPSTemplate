using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    public float lookSensitivity = 1f;
    public float moveSpeed = 10f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;

    private float xRotation = 0f;
    private float ySpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // Transform the direction into local coordinates
        Vector3 moveDirection = transform.TransformDirection(new Vector3(x, 0, z));

        if(controller.isGrounded)
        {
            // Keep slightly below zero to ensure we actually hit the ground
            ySpeed = -1f;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                ySpeed = jumpSpeed;
            }
        }
        else
        {
            // Gravity needs to be multiplied by deltaTime on its own because
            // it's an acceleration
            ySpeed -= gravity * Time.deltaTime;
        }

        // Apply speed to movement and add vertical velocity
        Vector3 velocity = (moveDirection * moveSpeed) + (Vector3.up * ySpeed);
        // Factor in deltaTime and move
        controller.Move(velocity * Time.deltaTime);
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity; 
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        // Vertical rotation, clamped between -90 and 90 degrees, only affects
        // camera transform
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation, 
        transform.Rotate(Vector3.up * mouseX);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotateCamera();
    }
}
