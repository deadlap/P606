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
        if (target == null) return Status.Failure;

        Vector3 desiredDirection;

        if (FacePosition.Value)
        {
            desiredDirection = target.transform.position - Agent.Value.transform.position;
            desiredDirection.Normalize();
        }
        else
        {
            desiredDirection = target.transform.forward;
            desiredDirection.Normalize();
        }

        // Rotate the direction vector by -45 degrees on Y to compensate for the world rotation
        //Quaternion worldOffset = Quaternion.Euler(0, -45, 0);
        //Vector3 adjustedDirection = worldOffset * desiredDirection;
        Quaternion targetRotation = Quaternion.LookRotation(desiredDirection);
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, targetRotation, Time.deltaTime * RotationSpeed.Value);

        // Calculate the dot product to check if the agent is facing the target direction
        dotProduct = Vector3.Dot(Agent.Value.transform.forward.normalized, desiredDirection);
        if (dotProduct > FacingAccuracy.Value)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}

