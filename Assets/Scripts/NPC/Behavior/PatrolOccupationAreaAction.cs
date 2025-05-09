using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolOccupationArea", story: "[Agent] patrols the [PatrolArea]", category: "Action", id: "91a45c5c312f4295a3cf3b87f195602c")]
public partial class PatrolOccupationAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<PatrolArea> PatrolArea;
    [SerializeReference] public BlackboardVariable<bool> IsWorker;
    [SerializeReference] public BlackboardVariable<bool> RandomPoint;
    [SerializeReference] public BlackboardVariable<bool> WillGetInLine;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> DeadPersonOccupation;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    [SerializeReference] public BlackboardVariable<string> Occupation;
    [SerializeReference] public BlackboardVariable<float> StoppingDistance;
    [SerializeReference] public BlackboardVariable<bool> IsDead;

    Transform currentPoint;
    bool pointGiven;
    bool isSitting;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            Debug.LogWarning("No agent assigned.");
            return Status.Failure;
        }
        if (Agent.Value.GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogWarning("Agent does not have a NavMeshAgent");
            return Status.Failure;
        }
        if (Occupation.Value == "")
        {
            Debug.LogWarning("No occupation assigned.");
            return Status.Failure;
        }
        if (Agent.Value.GetComponent<NPC>().isDead)
        {
            DeadPersonOccupation.Value = Agent.Value.GetComponent<Identity>().Occupation.ToString();
            Debug.LogWarning($"{Agent.Value.name} the {DeadPersonOccupation.Value} is dead.");
            IsDead.Value = true;
            return Status.Failure;
        }
        Initialize();
        WalkToPoint();
        return Status.Running;
    }
    void Initialize()
    {
        if (Agent.Value.GetComponent<NPC>().SpawnPoint == null)
        {
            Debug.LogWarning($"{Agent.Value.name} has no SpawnPoint set.");
            return;
        }
        currentPoint = null;
        pointGiven = false;
        isSitting = false;
        NavMeshAgent.Value.stoppingDistance = 0;
        if (PatrolArea.Value != null) return;
        PatrolArea.Value = Agent.Value.GetComponent<NPC>().SpawnPoint.GetComponent<PatrolArea>();
    }

    protected override Status OnUpdate()
    { 
        if(!pointGiven) return Status.Running;
        Animator.Value.SetBool("isWalking", true);
        Animator.Value.SetFloat("walkSpeed", NavMeshAgent.Value.speed);

        if (!NavMeshAgent.Value.pathPending)
        {
            if (CurrentPoint.Value == null) return Status.Failure;
            if (NavMeshAgent.Value.remainingDistance <= 1.5f &&    
                CurrentPoint.Value.GetComponent<PatrolPoint>() != null && 
                CurrentPoint.Value.GetComponent<PatrolPoint>().isSeat && 
                !IsWorker.Value && 
                !isSitting)
            {
                Animator.Value.Play("Sit");
                isSitting = true;
            }
            if (NavMeshAgent.Value.remainingDistance <= NavMeshAgent.Value.stoppingDistance)
            { 
                if (!NavMeshAgent.Value.hasPath || NavMeshAgent.Value.velocity.sqrMagnitude == 0)
                {
                    Animator.Value.SetBool("isWalking", false);
                    pointGiven = false;
                    return Status.Success;
                } 
            }
        }
        return Status.Running;
    }

    void WalkToPoint()
    {
        if (!pointGiven)
        {
            Animator.Value?.SetBool("isWalking", true);
            Animator.Value?.SetFloat("walkSpeed", NavMeshAgent.Value.speed);
            if (RandomPoint.Value)
            {
                currentPoint = PatrolArea.Value.RandomPatrolPoint();
            }
            else if (WillGetInLine.Value && !RandomPoint.Value)
            {
                currentPoint = PatrolArea.Value.FindPlaceInQueue(NavMeshAgent.Value.gameObject);
                if (currentPoint == null)
                {
                    Debug.LogWarning($"No space in the line for {NavMeshAgent.Value.name}.");
                    return;
                }
            }
            else
            {
                currentPoint = PatrolArea.Value.FindRandomUnreservedPoint(NavMeshAgent.Value.gameObject);
            }
            CurrentPoint.Value = currentPoint;
            NavMeshAgent.Value.SetDestination(CurrentPoint.Value.position);
            pointGiven = true;
        }
    }
}

