using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeaveChair", story: "Agent leaves chair", category: "Action", id: "64a19ccd4ceaa14538c4c7b14849413a")]
public partial class LeaveChairAction : Action
{
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    protected override Status OnStart()
    {
        if (CurrentPoint.Value.GetComponent<PatrolPoint>() != null)
            if (CurrentPoint.Value.GetComponent<PatrolPoint>().isSeat)
                Animator.Value.Play("Leave");
        return Status.Running;
    }
}

