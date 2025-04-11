using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.VFX;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopVFX", story: "[VFX] is stopped", category: "Action", id: "642e65ae4bc5368523e13afa76c8f8ad")]
public partial class StopVfxAction : Action
{
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;

    protected override Status OnStart()
    {
        if (VFX.Value != null || VFX.Value.gameObject.activeSelf)
        {
            VFX.Value.Stop();
        }
        else
        {
            Debug.LogWarning("VFX is null");
        }
        return Status.Success;
    }
}

