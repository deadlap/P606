using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FaceSameDirection", story: "[Agent] faces same direction as [Target]", category: "Action", id: "474ad4c562aded7bdde6031164c23ad4")]
public partial class FaceSameDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<bool> FacePosition;
    [SerializeReference] public BlackboardVariable<float> RotationSpeed;
    [SerializeReference] public BlackboardVariable<float> FacingAccuracy;
    float dotProduct;
    protected override Status OnUpdate()
    {
        return FacePoint(Target.Value.gameObject);
    }

    Status FacePoint(GameObject target)
    {
        if (target == null)
        {
            return Status.Failure;
        }
        if (FacePosition.Value)
        {
            var direction = target.transform.position - Agent.Value.transform.position;
            direction.y = 0; // Ignore vertical direction
            var targetRotation = Quaternion.LookRotation(direction);
            Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, targetRotation, Time.deltaTime * RotationSpeed.Value);
            dotProduct = Vector3.Dot(Agent.Value.transform.forward, direction.normalized);
        }
        else
        {
            Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, target.transform.rotation, Time.deltaTime * RotationSpeed.Value);
            dotProduct = Vector3.Dot(Agent.Value.transform.forward.normalized, target.transform.forward.normalized);
        }
        if (dotProduct > FacingAccuracy.Value)
        {
            return Status.Success;
        }
        return Status.Running;
    }
}

