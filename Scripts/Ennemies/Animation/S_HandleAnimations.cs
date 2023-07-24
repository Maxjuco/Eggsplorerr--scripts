using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class S_HandleAnimations : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private float normalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        normalSpeed = navMeshAgent.speed;
    }

    // Update is called once per frame
    void Update()
    {

        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if (navMeshAgent.speed == 0f)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        if ((navMeshAgent.speed == normalSpeed) && !isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        if ((navMeshAgent.speed > normalSpeed) && !isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        
    }
}
