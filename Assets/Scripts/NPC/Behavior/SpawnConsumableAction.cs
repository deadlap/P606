using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnConsumable", story: "[Consumeable] is instantiated at [Agent]", category: "Action", id: "7ec9bc9cb149de17b6327df44e1fe2d6")]
public partial class SpawnConsumableAction : Action
{
    [SerializeReference] public BlackboardVariable<SpawnConsumables> SpawnConsumables;
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<GameObject> FoodInstance = null;
    [SerializeReference] public BlackboardVariable<GameObject> DrinkInstance = null;
    [SerializeReference] public BlackboardVariable<string> Consumeable;
    [SerializeReference] public BlackboardVariable<bool> AsHat;

    protected override Status OnStart()
    {
        if (Consumeable.Value == "Food")
        {
            if(!AsHat.Value)
                SetPlatePosition();
            FoodInstance.Value = SpawnConsumables.Value.SpawnFood(Agent.Value, AsHat.Value);
        }
        if (Consumeable.Value == "Drink")
        {
            DrinkInstance.Value = SpawnConsumables.Value.SpawnDrink(Agent.Value);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    void SetPlatePosition()
    {
        Agent.Value.transform.position = CurrentPoint.Value.GetComponent<PatrolPoint>().plate.transform.position;
    }
}

