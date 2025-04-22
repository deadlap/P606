using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MixDrink", story: "[Shaker] is spawned at [Position]", category: "Action", id: "13dc82bb8108971b7fcf4586250384b4")]
public partial class MixDrinkAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Position;
    [SerializeReference] public BlackboardVariable<GameObject> Shaker;
    [SerializeReference] public BlackboardVariable<SpawnConsumables> SpawnConsumables;

    protected override Status OnStart()
    {
        Shaker.Value = SpawnConsumables.Value.SpawnShaker(Position.Value);
        return Status.Success;
    }
}

