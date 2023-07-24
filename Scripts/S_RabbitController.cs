using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class S_RabbitController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;

    public Transform[] waypoints;
    [Header("Test")]
    public int m_currentWayPointIndex;
    Vector3 playerLastPosition = Vector3.zero;
    public Vector3 m_PlayerPosition;


    public float m_WaitTime;
    public float m_TimeToRotate;
    public bool m_playerInRange;
    public bool m_PlayerNear;
    public bool m_IsPatrol;
    public bool m_CaughtPlayer;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_currentWayPointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[m_currentWayPointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnvironmentView();
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }
    }

    void Chasing()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;
        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(m_PlayerPosition);
        }
        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_currentWayPointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }
    void Patroling()
    {
        if (m_PlayerNear)
        { //si on aperçoit le joueur
            if(m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        { //on reprend la patrouille
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_currentWayPointIndex].position);
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    Debug.Log("prochain point");
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;

                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }

        }
    }
    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }
    private void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }
    private void NextPoint()
    {
        m_currentWayPointIndex = (m_currentWayPointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_currentWayPointIndex].position);
    }
    void EnvironmentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_playerInRange = false;
            }
            if (m_playerInRange)
            {
                m_PlayerPosition = player.position;
            }
        }
    }
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if(Vector3.Distance(transform.position,player) <= 0.3) //quand on perd le joueur de vue
        {
            if (m_WaitTime <= 0)
            { //retour à la patrouille
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_currentWayPointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

}
