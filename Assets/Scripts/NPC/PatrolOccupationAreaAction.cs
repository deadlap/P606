using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolOccupationArea", story: "[Agent] patrols with the [PatrolArea]", category: "Action", id: "91a45c5c312f4295a3cf3b87f195602c")]
public partial class PatrolOccupationAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<PatrolArea> PatrolArea;
    [SerializeReference] public BlackboardVariable<bool> RandomPoint;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    [SerializeReference] public BlackboardVariable<string> Occupation;

    Transform currentPoint;
    Vector3 currentPointPosition;
    bool pointGiven;

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
        if (Occupation == "")
        {
            Debug.LogWarning("No occupation assigned.");
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
        PatrolArea.Value = Agent.Value.GetComponent<NPC>().SpawnPoint.GetComponent<PatrolArea>();
    }

    protected override Status OnUpdate()
    {
        if (Vector2.Distance(currentPointPosition, Agent.Value.transform.position) < 0.5f)
        {
            Animator.Value?.SetBool("isWalking", false);
            CurrentPoint.Value = currentPoint;
            pointGiven = false;
            return Status.Success;
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
                currentPointPosition = PatrolArea.Value.RandomPatrolPoint();
            }
            else
            {
                currentPoint = PatrolArea.Value.RandomPosition();
                currentPointPosition = new(currentPoint.position.x, 1, currentPoint.position.z);
            }
            NavMeshAgent.Value.SetDestination(currentPointPosition);
            pointGiven = true;
        }
    }
}

