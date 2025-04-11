using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.Sentis.Layers;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "OccupationChecker", story: "[Agent] is assigned its [Occupation]", category: "Action", id: "bc98d80f6773202c17c5a88d273a5c4b")]
public partial class SetOccupationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> Occupation;
    [SerializeReference] public BlackboardVariable<OccupationEnum> OccupationEnum;
    [SerializeReference] public BlackboardVariable<Identity> Identity;
    [SerializeReference] public BlackboardVariable<PatrolArea> OwnCabin;


    protected override Status OnStart()
    {
        if (Agent.Value.GetComponent<Identity>() == null)
        {
            Debug.LogWarning($"{Agent.Value.name} has no Identity set.");
            return Status.Failure;
        }
        Identity.Value = Agent.Value.GetComponent<Identity>();
        Occupation.Value = Identity.Value.Occupation.ToString();
        switch (Occupation.Value.ToLower())
        {
            case "chef":
                OccupationEnum.ObjectValue = 0;
                break;
            case "waiter":
                OccupationEnum.ObjectValue = 1;
                break;
            case "bartender":
                OccupationEnum.ObjectValue = 2;
                break;
            case "janitor":
                OccupationEnum.ObjectValue = 3;
                break;
            default:
                OccupationEnum.ObjectValue = 4;
                break;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Identity.Value.GetComponent<NPC>().SpawnPoint.GetComponent<PatrolArea>())
        {
            OwnCabin.Value = Identity.Value.GetComponent<NPC>().SpawnPoint.GetComponent<PatrolArea>();
            return Status.Success;
        }
        return Status.Running;
    }
}

