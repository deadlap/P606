using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.VFX;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PerformsWork", story: "[Agent] performs work", category: "Action", id: "e52e9103177aa4d575f18722a7e80708")]
public partial class PerformsWorkAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;
    [SerializeReference] public BlackboardVariable<string> Occupation;
    [SerializeReference] public BlackboardVariable<bool> IsWorking;

    protected override Status OnStart()
    {
        IsWorking.Value = true;
        switch (Occupation.Value)
        {
            case "Chef":
                VFX.Value.gameObject.transform.localPosition = new(0, 2.338f, 1.59f);
                Animator.Value.Play("PreparingFood");
                break;
            case "Waiter":
                VFX.Value.gameObject.transform.localPosition = new(0, 2.338f, 1.59f);
                break;
            case "Bartender":
                VFX.Value.gameObject.transform.localPosition = new(0, 2.338f, 1.59f);
                break;
            case "Janitor":
                VFX.Value.gameObject.transform.localPosition = new(0, 1.08f, 1.59f);
                break;
            default:
                break;

        }
        if (VFX.Value != null || VFX.Value.gameObject.activeSelf)
        {
            VFX.Value.Play();
        }
        return Status.Running;
    }

    void EnableVFX()
    {
        
    }
}

