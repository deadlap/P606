using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Unity.Sentis;
using UnityEngine.AI;
using UnityEngine.VFX;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PrepareFood", story: "[Agent] makes food", category: "Action", id: "e3af089590678ba630d4a25dd0c48f7f")]
public partial class PrepareFoodAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<Transform> CurrentPoint;
    [SerializeReference] public BlackboardVariable<VisualEffect> VFX;
    float RotationSpeed = 5f; 
    float facingAccuracy = 0.95f;

    protected override Status OnStart()
    {
        Animator.Value.Play("PreparingFood");
        VFX.Value.Play();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return FacePoint(CurrentPoint.Value.gameObject);
    }
    
    Status FacePoint(GameObject target)
    {
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, target.transform.rotation, Time.deltaTime * RotationSpeed);
        float dotProduct = Vector3.Dot(Agent.Value.transform.forward, target.transform.forward);
        if (dotProduct > facingAccuracy)
        {
            return Status.Success;
        }
        return Status.Running;
    }
}
