using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private Waypoint waypoints;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float distance = 0.1f;
    private Transform current;

    // Start is called before the first frame update
    void Start()
    {
        current = waypoints.GetNextWaypoint(current);
        transform.position = current.position;

        current = waypoints.GetNextWaypoint(current);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, current.position, moveSpeed);
        if (Vector3.Distance(transform.position,current.position) < distance) 
        {
            current = waypoints.GetNextWaypoint(current);

            // Calculate the direction to the next waypoint
            Vector3 direction = current.position - transform.position;
            direction.y = 0f; // Ignore the y-axis to prevent flipping

            // Flip the direction if needed
            if (Vector3.Dot(transform.forward, direction) < 0)
            {
                direction *= -1;
            }

            // Rotate towards the next waypoint
            transform.LookAt(transform.position + direction);
        }
    }
}
