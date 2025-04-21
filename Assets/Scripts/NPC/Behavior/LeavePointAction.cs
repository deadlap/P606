using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "LeavePoint", story: "Agent leaves [CurrentPoint]", category: "Action", id: "ca16380114a71b3a8bfe6f12fb2670af")]
public partial class LeavePointAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;

    protected override Status OnStart()
    {
        CurrentPoint.Value.GetComponent<PatrolPoint>().isBeingServed = false;
        CurrentPoint.Value.GetComponent<PatrolPoint>().hasBeenServed = false;
        return Status.Success;
    }
}

