using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public float maxSpeed = 5000f; // The maximum speed of the car
    public float acceleration = 300f; // The acceleration of the car
    public float turningSpeed = 100f; // The speed at which the car turns
    public float brakeStrength = 100f; // The speed at which the car turns
    public Button resetButton;
    public TextMeshProUGUI resetButtonText;
    public int raceRequiredTracks;

    private Rigidbody rb;
    private Transform[] wheelTransforms = null; //wheels transforms
    private HashSet<string> iteratedTracks = new HashSet<string>();
    private string startTrack;
    private bool raceFinished;
    private Color resetButtonColor;
    private Color resetButtonTextColor;
    private bool hadAccident; //Car 180 degrees rotated and touching the ground 
    private bool outOfMap; //Car in the ground and with four wheels on a banned tagged area

    private void Start()
    {
        UnitySystemConsoleRedirector.Redirect();

        resetButton.onClick.AddListener(()=> {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        resetButtonColor = resetButton.image.color;
        resetButtonTextColor = resetButtonText.color;

        MakeButtonInvisible(resetButton);
        MakeButtonTextInvisible(resetButtonText);

        startTrack = string.Empty;
        raceFinished = false;
        hadAccident = false;
        outOfMap = false;

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
        if (hadAccident || raceFinished || outOfMap)
            return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool brakeInput = Input.GetButton("Jump");

        bool allWheelsOnGround = true;
        //All wheels on ground
        foreach (Transform wheel in wheelTransforms) {
            if (!IsWheelOnGround(wheel, 0.5f)) {
                allWheelsOnGround = false;
                break;
            }
        }

        // Calculate the acceleration of the ride based on vertical input, only if all wheels are on the ground
        float accelerationAmount =  allWheelsOnGround ? -verticalInput * acceleration * Time.deltaTime : 0;

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

    private bool HasCarHadAccident()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        return angle >= 70f;
    }

    private void OnCollisionStay(Collision collision)
    {
        //Accident?
        if (HasCarHadAccident())
        {
            hadAccident = true;
            MakeButtonVisible(resetButton, resetButtonColor);
            MakeButtonTextVisible(resetButtonText, resetButtonTextColor);
        }

        if (collision.gameObject.CompareTag("Banned")) {
            //Out of map?
            outOfMap = true;
            MakeButtonVisible(resetButton, resetButtonColor);
            MakeButtonTextVisible(resetButtonText, resetButtonTextColor);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            //Race finished?
            var trackName = collision.gameObject.name;
            var added = iteratedTracks.Add(trackName);
            //Saving the name of the first track
            if (startTrack.Equals(string.Empty))
                startTrack = trackName;

            if (trackName.Equals(startTrack) && iteratedTracks.Count == raceRequiredTracks)
                RaceFinished();
        }
    }

    private void RaceFinished()
    {
        raceFinished = true;
        Console.WriteLine($"Race finished");
    }

    //Makes a button invisible and not enabled
    void MakeButtonInvisible(Button button)
    {
        //Making the button invisible and not interactable
        Color resetButtonColor = button.image.color;
        resetButtonColor.a = 0f;

        button.interactable = false;
        button.image.color = resetButtonColor;
    }

    void MakeButtonTextInvisible(TextMeshProUGUI buttonText)
    {
        Color textColor = buttonText.color;
        textColor.a = 0f;
        buttonText.color = textColor;
    }

    void MakeButtonVisible(Button button, Color color)
    {
        button.interactable = true;
        button.image.color = color;
    }
    void MakeButtonTextVisible(TextMeshProUGUI buttonText, Color color)
    {
        buttonText.color = color;
    }
}