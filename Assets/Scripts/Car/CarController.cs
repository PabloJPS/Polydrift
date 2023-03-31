using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CarController : MonoBehaviour
{
    public float maxSpeed = 30f; // The maximum speed of the car
    public float acceleration = 10f; // The acceleration of the car
    public float turningSpeed = 100f; // The speed at which the car turns
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000; // Set the mass of the Rigidbody component to 1000
        rb.drag = 0.5f; // Set the drag value of the Rigidbody component
        rb.angularDrag = 0.5f; // Set the angular drag value of the Rigidbody component
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Get input from the player for turning
        float moveVertical = Input.GetAxis("Vertical"); // Get input from the player for accelerating/reversing

        Vector3 movement = new Vector3(0.0f, 0.0f, moveVertical); // Combine input into a movement vector
        rb.AddRelativeForce(movement * acceleration); // Apply the movement vector as a force to the Rigidbody

        Vector3 rotation = new Vector3(0.0f, moveHorizontal * turningSpeed, 0.0f); // Create a rotation vector based on input for turning
        rb.AddTorque(rotation); // Apply the rotation vector as torque to the Rigidbody

        // Limit the maximum speed of the car
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}