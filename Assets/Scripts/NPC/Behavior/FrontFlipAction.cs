using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FrontFlip", story: "[Agent] performs front flip", category: "Action", id: "69d5e8faad7cf7207f53c02cd9ab6f12")]
public partial class FrontFlipAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<int> TargetValue;

    protected override Status OnStart()
    {
        int randomValue = Random.Range(1, TargetValue.Value);
        if (randomValue != (TargetValue.Value - 1)) return Status.Failure; // - 1 since Random.Range is maxExclusive
        Animator.Value.SetTrigger("FrontFlip");
        return Status.Running;
    }
}

