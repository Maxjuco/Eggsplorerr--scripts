using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_Patrolling : MonoBehaviour
{
    private NavMeshAgent agent;
    public S_Waypoint[] waypoints;
    public float speed;
    private int waypointIndex = 0;
    private float currentWaitingTime;
    S_Waypoint currentWaypoint;

    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        currentWaitingTime = waypoints[waypointIndex].waitingTime;
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
        agent.speed = speed;

    }

    void UpdateDestination()
    {
        currentWaypoint = waypoints[waypointIndex];
        currentWaitingTime = currentWaypoint.waitingTime;
        target = currentWaypoint.waypoint.position;
        agent.SetDestination(target);      
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWaitingTime -= Time.deltaTime;
            if (currentWaitingTime <= 0)
            {
                waypointIndex = (waypointIndex + 1) % waypoints.Length;
                UpdateDestination();
            }
        }
    }
}
