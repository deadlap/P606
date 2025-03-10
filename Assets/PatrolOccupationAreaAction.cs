using System;
using System.Collections;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolOccupationArea", story: "[Agent] patrols the occupation area.", category: "Action", id: "91a45c5c312f4295a3cf3b87f195602c")]
public partial class PatrolOccupationAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    Identity identity;
    string occupation;
    GameObject patrolArea;
    bool hasInitialized;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            Debug.Log("No agent assigned.");
            return Status.Failure;
        }
        Initialize();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    void Initialize()
    {
        if(hasInitialized) return;
        if(Agent.Value.GetComponent<Identity>() == null) return;
        identity = Agent.Value.GetComponent<Identity>();
        occupation = identity.Occupation.ToString();
        if(Agent.Value.GetComponent<NPC>().spawnPoint == null) return; 
        patrolArea = Agent.Value.GetComponent<NPC>().spawnPoint;
        Debug.Log($"im {Agent.Value.name} and im a {occupation}. i like to walk around {patrolArea.name}");
        hasInitialized = true;
    }
}

