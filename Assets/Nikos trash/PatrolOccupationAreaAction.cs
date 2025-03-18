using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolOccupationArea", story: "[Agent] patrols the occupation area.", category: "Action", id: "91a45c5c312f4295a3cf3b87f195602c")]
public partial class PatrolOccupationAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    [SerializeReference] public BlackboardVariable<float> speed;

    NavMeshAgent navMeshAgent;
    Identity identity;
    string occupation;
    bool hasInitialized;
    
    PatrolArea patrolArea;
    Vector3 currentPoint;
    bool pointGiven;
    bool pointReached;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            Debug.Log("No agent assigned.");
            return Status.Failure;
        }

        if (Agent.Value.GetComponent<NavMeshAgent>() == null)
        {
            Debug.Log("Agent does not have a NavMeshAgent");
            return Status.Failure;
        }
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speed;
        Initialize();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        WalkToPoint();
        if (Vector2.Distance(currentPoint, Agent.Value.transform.position) < 0.5f)
        {
            Debug.Log($"{Agent.Value.name} reached point");
            pointGiven = false;
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
    
    void Initialize()
    {
        if(hasInitialized) return;
        if (Agent.Value.GetComponent<Identity>() == null)
        {
            Debug.Log($"{Agent.Value.name} has no Identity set.");
            return;
        }
        identity = Agent.Value.GetComponent<Identity>();
        occupation = identity.Occupation.ToString();
        if (Agent.Value.GetComponent<NPC>().SpawnPoint == null)
        {
            Debug.Log($"{Agent.Value.name} has no SpawnPoint set.");
            return;
        }
        patrolArea = Agent.Value.GetComponent<NPC>().SpawnPoint.GetComponent<PatrolArea>();
        Debug.Log($"im {Agent.Value.name} and im a {occupation}. i like to walk around {patrolArea.name}");
        hasInitialized = true;
    }

    void WalkToPoint()
    {
        if (!pointGiven)
        {
            currentPoint = patrolArea.RandomPatrolPoint();
            navMeshAgent.SetDestination(currentPoint);
            pointGiven = true;
        }
    }
}

