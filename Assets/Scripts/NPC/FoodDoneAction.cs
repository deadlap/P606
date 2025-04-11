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
    [SerializeReference] public BlackboardVariable<bool> IsPreparingFood;
    [SerializeReference] public BlackboardVariable<int> FoodDifficulty;
    [SerializeReference] public BlackboardVariable<int> FoodGuestsWaiting;
    [SerializeReference] public BlackboardVariable<int> FoodReady;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;
    int foodProgress;
    protected override Status OnStart()
    {
        Animator.Value?.SetTrigger("resetState");
        VFX.Value.Stop();
        foodProgress++;
        Debug.Log($"Food is not done yet ({foodProgress}/{FoodDifficulty.Value}).");
        if (foodProgress == FoodDifficulty.Value)
        {
            IsPreparingFood.Value = false;
            foodProgress = 0;
            FoodGuestsWaiting.Value--;
            FoodReady.Value++;
            Debug.Log("Food is done!");
        }
        return Status.Running;
    }
}

