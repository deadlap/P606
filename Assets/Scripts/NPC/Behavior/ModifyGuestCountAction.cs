using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ModifyGuestCount", story: "Modify [GuestCount]", category: "Action", id: "4c6c66c0f6c2d6c949c803bc08d22b7e")]
public partial class ModifyGuestCountAction : Action
{
    [SerializeReference] public BlackboardVariable<int> GuestCount;
    [SerializeReference] public BlackboardVariable<bool> Additive;


    protected override Status OnStart()
    {
        if(Additive.Value)
        {
            GuestCount.Value++;
        }
        else
        {
            GuestCount.Value--;
            if(GuestCount.Value < 0)
                GuestCount.Value = 0;
        }
        return Status.Success;
    }
}

