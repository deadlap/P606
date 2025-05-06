using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Random = UnityEngine.Random;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseMood", story: "[Agent] chooses a mood from [MoodEnum]", category: "Action", id: "ed9564685687edd22c5892ac57ef67fe")]
public partial class ChooseMoodAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<MoodEnum> MoodEnum;

    protected override Status OnStart()
    {
        switch (Random.Range(0, 5))
        {
            case 0:
                MoodEnum.ObjectValue = 0;
                break;
            case 1:
                MoodEnum.ObjectValue = 1;
                break;
            case 2:
                MoodEnum.ObjectValue = 2;
                break;
            case 3:
                MoodEnum.ObjectValue = 3;
                break;
            case 4:
                MoodEnum.ObjectValue = 4;
                break;
            default:
                break;
        }
        return Status.Success;
    }
}

