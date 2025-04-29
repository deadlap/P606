using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ToggleGameObject", story: "[GameObjectItem] is [Toggle]", category: "Action", id: "a93c5d95bf141d4540037578f802f286")]
public partial class ToggleGameObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> GameObjectItem;
    [SerializeReference] public BlackboardVariable<bool> Toggle;

    protected override Status OnStart()
    {
        GameObjectItem.Value.SetActive(Toggle.Value);
        return Status.Running;
    }
}

