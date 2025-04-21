using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DestroyConsumable", story: "[ConsumableInstance] is destroyed", category: "Action", id: "ddcf28d1aaf67c7a275a1361c126a66d")]
public partial class DestroyConsumableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> ConsumableInstance;
    [SerializeReference] public BlackboardVariable<SpawnConsumables> SpawnConsumables;

    protected override Status OnStart()
    {
        SpawnConsumables.Value.DestroyConsumable(ConsumableInstance.Value);
        return Status.Running;
    }
}

