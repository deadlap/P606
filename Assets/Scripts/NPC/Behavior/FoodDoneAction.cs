using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.VFX;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FoodDone", story: "[Agent] done making food", category: "Action", id: "f6b3a788acf980065c2a717f2da95ce1")]
public partial class FoodDoneAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<bool> IsWorking;
    [SerializeReference] public BlackboardVariable<int> FoodDifficulty;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;
    [SerializeReference] public BlackboardVariable<int> FoodProgress;
    [SerializeReference] public BlackboardVariable<int> FoodGuestCount;
    [SerializeReference] public BlackboardVariable<int> FoodReady;
    protected override Status OnStart()
    {
        Animator.Value?.SetTrigger("resetState");
        FoodProgress.Value++;
        Debug.Log($"Food is not done yet ({FoodProgress.Value}/{FoodDifficulty.Value}).");
        if (FoodProgress.Value == FoodDifficulty.Value)
        {
            IsWorking.Value = false;
            FoodProgress.Value = 0;
            FoodReady.Value++;
            FoodGuestCount.Value--;
            Debug.Log("Food is done!");
        }
        return Status.Running;
    }
}

