using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnPersonalItem", story: "[Item] is spawned at [Position]", category: "Action", id: "13dc82bb8108971b7fcf4586250384b4")]
public partial class SpawnPersonalItemAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Position;
    [SerializeReference] public BlackboardVariable<GameObject> Item;
    [SerializeReference] public BlackboardVariable<SpawnConsumables> SpawnConsumables;
    [SerializeReference] public BlackboardVariable<string> Occupation;

    protected override Status OnStart()
    {
        if(Occupation.Value == "Bartender")
        {
            Item.Value = SpawnConsumables.Value.SpawnShaker(Position.Value);
        }
        if(Occupation.Value == "Janitor")
        {
            Item.Value = SpawnConsumables.Value.SpawnBroom(Position.Value);
        }
        return Status.Success;
    }
}

