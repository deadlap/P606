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
                OccupationEnum.ObjectValue = Identity.Value.Occupation;
                break;
            case "waiter":
                OccupationEnum.ObjectValue = Identity.Value.Occupation;
                break;
            case "bartender":
                OccupationEnum.ObjectValue = Identity.Value.Occupation;
                break;
            case "janitor":
                OccupationEnum.ObjectValue = Identity.Value.Occupation;
                break;
            default:
                OccupationEnum.ObjectValue = OccupationEnum.ObjectValue;
                break;
        }
        return Status.Success;
    }
}

