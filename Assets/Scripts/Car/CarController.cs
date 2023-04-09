using UnityEngine;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5000f; // The maximum speed of the car
    public float acceleration = 300f; // The acceleration of the car
    public float turningSpeed = 100f; // The speed at which the car turns
    public float brakeStrength = 100f; // The speed at which the car turns

    private Rigidbody rb;
    private Transform[] wheelTransforms = null; //wheels transforms

    private void Start()
    {
        wheelTransforms = new Transform[4];
        // Get the children of a GameObject
        Transform parentTransform = gameObject.transform;
        int childCount = parentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            wheelTransforms[i] = childTransform;
        }
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

        // Check if any of the wheels are not on the ground
        bool isOnGround = true;
        foreach (Transform wheelTransform in wheelTransforms)
        {
            if (!IsWheelOnGround(wheelTransform, 2))
            {
                isOnGround = false;
                break;
            }
        }

        // Calculate the acceleration of the ride based on vertical input, only if all wheels are on the ground
        float accelerationAmount = isOnGround ? -verticalInput * acceleration * Time.deltaTime : 0f;

        // Get the ride's forward direction in local space
        Vector3 rideForward = transform.TransformDirection(Vector3.forward);

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

    private bool IsWheelOnGround(Transform wheelTransform, float maxDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(wheelTransform.position, -wheelTransform.up, out hit, maxDistance))
            // Check if the hit object has a tag indicating it is the ground
            if (hit.collider.CompareTag("Ground"))
                return true;

        return false;
    }


}