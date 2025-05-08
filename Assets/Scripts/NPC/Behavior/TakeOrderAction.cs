using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
// using Unity.PlasticSCM.Editor.WebApi;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TakeOrder", story: "[Agent] takes order from guest in [PatrolArea]", category: "Action", id: "72a83364afe46bb68080ea54e83a6119")]
public partial class TakeOrderAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<PatrolArea> PatrolArea;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    [SerializeReference] public BlackboardVariable<float> StoppingDistance;
    [SerializeReference] public BlackboardVariable<bool> BringsFood;
    [SerializeReference] public BlackboardVariable<bool> ServesQueue;
    Transform currentPoint;

    protected override Status OnStart()
    {
        currentPoint = null;
        Animator.Value?.SetBool("isWalking", true);
        Animator.Value?.SetFloat("walkSpeed", NavMeshAgent.Value.speed);    
        if(BringsFood.Value)
        {
            currentPoint = PatrolArea.Value.FindGuestToServe(NavMeshAgent.Value.gameObject, true);
            if (currentPoint == null)
            {
                Debug.LogWarning("No guest to serve.");
                return Status.Failure;
            }
            NavMeshAgent.Value.SetDestination(currentPoint.position);
        }
        
        if (!BringsFood.Value && !ServesQueue.Value)
        {
            currentPoint = PatrolArea.Value.FindGuestToServe(NavMeshAgent.Value.gameObject);
            if(currentPoint == null)
            {
                Debug.LogWarning("No guest to serve.");
                return Status.Failure;
            }
            NavMeshAgent.Value.SetDestination(currentPoint.position);
        }

        if (!BringsFood.Value && ServesQueue.Value)
        {
            currentPoint = PatrolArea.Value.FindGuestInQueue(NavMeshAgent.Value.gameObject);
            if (currentPoint.GetComponent<PatrolPoint>().isBeingServed)
            {
            }
        }

        if (!BringsFood.Value && ServesQueue.Value)
        {
            currentPoint = PatrolArea.Value.FindGuestInQueue(NavMeshAgent.Value.gameObject);
        }


        NavMeshAgent.Value.stoppingDistance = StoppingDistance.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
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

                if (BringsFood.Value)
                {
                    currentPoint.GetComponent<PatrolPoint>().hasBeenServed = true;
                }

                if (!BringsFood.Value && !ServesQueue.Value)
                {
                    if (!currentPoint.GetComponent<PatrolPoint>())
                    {
                        return Status.Failure;
                    }
                    currentPoint.GetComponent<PatrolPoint>().isBeingServed = true;
                }

                if (!BringsFood.Value && ServesQueue.Value)
                {
                    if (currentPoint.GetComponent<PatrolPoint>().isBeingServed)
                    {
                        currentPoint.GetComponent<PatrolPoint>().hasBeenServed = true;
                    }
                }

                if (!BringsFood.Value && ServesQueue.Value)
                {
                    currentPoint.GetComponent<PatrolPoint>().isBeingServed = true;
                }
                if(!ServesQueue.Value)
                    CurrentPoint.Value = currentPoint;
                return Status.Success;
            }
        }
        return Status.Running;
    }
}

