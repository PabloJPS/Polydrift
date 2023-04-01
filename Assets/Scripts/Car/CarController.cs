using System.Collections;
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
        rb.mass = 10; // Set the mass of the Rigidbody component to 1000
        rb.drag = 0.5f; // Set the drag value of the Rigidbody component
        rb.angularDrag = 0.5f; // Set the angular drag value of the Rigidbody component
    }

    private void Update()
    {
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");

        // // Get the ride's forward direction in local space
        // Vector3 rideForward = transform.TransformDirection(Vector3.forward);

        // // Move the ride forward or backward based on vertical input
        // Vector3 movement = rideForward * -verticalInput * 200 * Time.deltaTime;
        // rb.MovePosition(transform.position + movement);

        // // Turn the ride based on horizontal input
        // float turnAmount = horizontalInput * 20 * Time.deltaTime;
        // Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        // rb.MoveRotation(rb.rotation * turnRotation);
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Get the ride's forward direction in local space
        Vector3 rideForward = transform.TransformDirection(Vector3.forward);

        // Calculate the acceleration of the ride based on vertical input
        float accelerationAmount = -verticalInput * acceleration * Time.deltaTime;

        // Limit the ride's speed to the maximum speed
        float currentSpeed = Vector3.Dot(rb.velocity, rideForward);
        float maxSpeedPerFrame = maxSpeed * Time.deltaTime;
        float newSpeed = Mathf.Clamp(currentSpeed + accelerationAmount, -maxSpeedPerFrame, maxSpeedPerFrame);

        // Move the ride forward or backward based on the new speed
        Vector3 movement = rideForward * newSpeed;
        rb.MovePosition(transform.position + movement);

        // Turn the ride based on horizontal input
        float turnAmount = horizontalInput * turningSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

}