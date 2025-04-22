using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeaveTable", story: "Agent waits until they are served", category: "Action", id: "424db43c1292cb186f3077850416951d")]
public partial class WaitForServiceAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<PatrolArea> PatrolArea;
    [SerializeReference] public BlackboardVariable<PatrolOccupationAreaAction> PatrolOccupationArea;
    PatrolPoint patrolPoint;
    Transform currentPoint;
    bool pointGiven;

    protected override Status OnStart()
    {
        if(CurrentPoint.Value == null)
        {
            Debug.LogWarning("No current point assigned.");
            return Status.Failure;
        }
        pointGiven = false;
        patrolPoint = CurrentPoint.Value.GetComponent<PatrolPoint>();
        return Status.Running;
    }
    protected override Status OnUpdate()
    {
        if (!patrolPoint.hasBeenServed)
        {
            if(PatrolArea.Value != null)
            {
                if(patrolPoint.patrolPointIndex - 1 >= 0)
                {
                    if (!PatrolArea.Value.queuePoints[patrolPoint.patrolPointIndex - 1].GetComponent<PatrolPoint>().isReserved)
                    {
                        currentPoint = PatrolArea.Value.FindPlaceInQueue(NavMeshAgent.Value.gameObject);
                        CurrentPoint.Value = currentPoint;
                        patrolPoint = CurrentPoint.Value.GetComponent<PatrolPoint>();
                        NavMeshAgent.Value.SetDestination(patrolPoint.transform.position);
                        pointGiven = true;
                    }
                }
            }
            if (pointGiven)
            {
                if (NavMeshAgent.Value.pathPending)
                {
                    Animator.Value?.SetBool("isWalking", true);
                    Animator.Value?.SetFloat("walkSpeed", NavMeshAgent.Value.speed);
                }
                else if (NavMeshAgent.Value.remainingDistance <= NavMeshAgent.Value.stoppingDistance)
                {
                    if (!NavMeshAgent.Value.hasPath || NavMeshAgent.Value.velocity.sqrMagnitude <= 0)
                    {
                        Animator.Value?.SetBool("isWalking", false);
                        pointGiven = false;
                    }
                }
            }
            return Status.Running;
        }
        else return Status.Success;
    }
}

