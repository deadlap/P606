using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BeginPrepareFood", story: "[Agent] begins to prepare food", category: "Action", id: "f8e80e6f7e231a5a7b2eeed92e70a11a")]
public partial class BeginPrepareFoodAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<int> FoodGuestsWaiting;
    [SerializeReference] public BlackboardVariable<int> FoodDifficulty;
    [SerializeReference] public BlackboardVariable<bool> IsWorking;

    protected override Status OnStart()
    {
        if(Agent.Value == null)
        {
            Debug.LogWarning("No agent assigned.");
            return Status.Failure;
        }
        if (IsWorking) return Status.Failure;
        IsWorking.Value = true;
        FoodDifficulty.Value = UnityEngine.Random.Range(3, 7);
        return Status.Running;
    }
}

