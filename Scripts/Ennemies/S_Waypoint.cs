using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Waypoint : MonoBehaviour
{
    public float waitingTime;
    public Transform waypoint;

    private void Start()
    {
        waypoint = GetComponent<Transform>();
    }
}
