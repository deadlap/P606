using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetPatrolAreas", story: "Various Values are assigned", category: "Action", id: "a97497ae0869809563b66572778a0bb9")]
public partial class SetValuesAction : Action
{
    [SerializeReference] public BlackboardVariable<PatrolArea> Kitchen;
    [SerializeReference] public BlackboardVariable<PatrolArea> PoolArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> DiningArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> BarLineArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> BarOrderArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> BarArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> Bar;
    [SerializeReference] public BlackboardVariable<PatrolArea> KitchenOrderArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> KitchenAndDiningArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> CleanUpArea;
    [SerializeReference] public BlackboardVariable<PatrolArea> WanderArea;

    [SerializeReference] public BlackboardVariable<SpawnConsumables> SpawnConsumables;
    protected override Status OnStart()
    {
        Kitchen.Value = GameObject.Find("Kitchen").GetComponent<PatrolArea>();
        PoolArea.Value = GameObject.Find("PoolArea").GetComponent<PatrolArea>();
        DiningArea.Value = GameObject.Find("DiningArea").GetComponent<PatrolArea>();
        BarLineArea.Value = GameObject.Find("BarLineArea").GetComponent<PatrolArea>();
        BarOrderArea.Value = GameObject.Find("BarOrderArea").GetComponent<PatrolArea>();
        BarArea.Value = GameObject.Find("BarArea").GetComponent<PatrolArea>();
        Bar.Value = GameObject.Find("Bar").GetComponent<PatrolArea>();
        KitchenOrderArea.Value = GameObject.Find("KitchenOrderArea").GetComponent<PatrolArea>();
        KitchenAndDiningArea.Value = GameObject.Find("KitchenAndDiningArea").GetComponent<PatrolArea>();
        CleanUpArea.Value = GameObject.Find("CleanUpArea").GetComponent<PatrolArea>();
        WanderArea.Value = GameObject.Find("WanderArea").GetComponent<PatrolArea>();

        SpawnConsumables.Value = GameObject.Find("SpawnConsumables").GetComponent<SpawnConsumables>();

        return Status.Success;
    }
}

