using System;
using System.Globalization;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.Sentis.Layers;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PatrolOccupationArea", story: "[Agent] patrols the occupation area.", category: "Action", id: "91a45c5c312f4295a3cf3b87f195602c")]
public partial class PatrolOccupationAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    Identity identity;
    string occupation;
    GameObject spawnPoint;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            Debug.Log("No agent assigned.");
            return Status.Failure;
        }
        identity = Agent.Value.GetComponent<Identity>();
        occupation = identity.Occupation.ToString();
        Debug.Log($"im {Agent.Value.name} and im a {occupation}. i like to walk around ");
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

