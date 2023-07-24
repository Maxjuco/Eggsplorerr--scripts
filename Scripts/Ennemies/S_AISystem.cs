using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_AISystem : MonoBehaviour
{
    public float radius;
    public float angle;
    public GameObject[] targets;

    public MonoBehaviour[] behaviours;
    private List<LayerMask> targetLayerMasks;
    public LayerMask obstructionMask;
    public Transform target;
    public bool canSeeTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        targetLayerMasks = new List<LayerMask>();
        foreach (GameObject target in targets)
        {
            targetLayerMasks.Add(1 << target.layer);
        }
        GetComponent<S_Patrolling>().enabled = true;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.8f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks;
        rangeChecks = Physics.OverlapSphere(transform.position, radius);

        List<Collider> rangec = new List<Collider>(rangeChecks);
        rangec.Reverse();
        foreach (Collider collide in rangec)
        {
            if (targetLayerMasks.Contains(1 << collide.gameObject.layer))
            {
                target = collide.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        canSeeTarget = true;
                    }
                    canSeeTarget = false;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else if (canSeeTarget)
            {
                canSeeTarget = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //faire la detection des éléments de targetLayerMasks
        behaviours[0].enabled = true;
        StartCoroutine(FOVRoutine());
    }
}
