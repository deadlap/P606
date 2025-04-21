using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.Sentis;
using UnityEngine.AI;
using UnityEngine.VFX;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PrepareFood", story: "[Agent] makes food", category: "Action", id: "e3af089590678ba630d4a25dd0c48f7f")]
public partial class PrepareFoodAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;

    protected override Status OnStart()
    {
        Animator.Value.Play("PreparingFood");
        if(VFX.Value != null || VFX.Value.gameObject.activeSelf)
        {
            VFX.Value.Play();
        }
        return Status.Running;
    }
}
