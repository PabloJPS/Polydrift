using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5000f; // The maximum speed of the car
    public float acceleration = 300f; // The acceleration of the car
    public float turningSpeed = 100f; // The speed at which the car turns
    public float brakeStrength = 100f; // The speed at which the car turns

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool brakeInput = Input.GetButton("Jump");

        // Get the ride's forward direction in local space
        Vector3 rideForward = transform.TransformDirection(Vector3.forward);

        // Calculate the acceleration of the ride based on vertical input
        float accelerationAmount = -verticalInput * acceleration * Time.deltaTime;

        // Limit the ride's speed to the maximum speed
        float currentSpeed = Vector3.Dot(rb.velocity, rideForward);
        float maxSpeedPerFrame = maxSpeed * Time.deltaTime;
        float newSpeed = Mathf.Clamp(currentSpeed + accelerationAmount, -maxSpeedPerFrame, maxSpeedPerFrame);

        // Calculate the force needed to achieve the desired speed
        float forceMagnitude = (newSpeed - currentSpeed) / Time.deltaTime * rb.mass;

        // Apply the force to the ride's Rigidbody component
        Vector3 movementForce = rideForward * forceMagnitude;
        rb.AddForce(movementForce);

        // Apply a brake force if the brake input is detected
        if (brakeInput)
        {
            Vector3 brakeForce = -rb.velocity.normalized * brakeStrength * rb.mass;
            rb.AddForce(brakeForce);
        }

        // Turn the ride based on horizontal input
        float turnAmount = horizontalInput * turningSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}