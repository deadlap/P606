using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class NPCActionInterrupter : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    BehaviorGraphAgent behaviorGraphAgent;
    float originalSpeed;
    float originalAcceleration;
    float originalYRotation;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        originalSpeed = navMeshAgent.speed;
        originalAcceleration = navMeshAgent.acceleration;
    }
    
    void OnEnable()
    {
        PlayerInputEvent.EnterDialog += Interrupt;
        PlayerInputEvent.ExitDialog += Resume;
    }

    void OnDisable()
    {
        PlayerInputEvent.EnterDialog -= Interrupt;
        PlayerInputEvent.ExitDialog -= Resume;
    }

    void Interrupt()
    {
        if (PlayerController.instance.currentInteractable.transform.parent == null) return;
        if (PlayerController.instance.currentInteractable.transform.parent.gameObject != gameObject) return;
        originalYRotation = transform.rotation.eulerAngles.y;
        behaviorGraphAgent.enabled = false;
        animator.SetBool("isWalking", false);
        transform.LookAt(PlayerController.instance.transform);
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = float.MaxValue; // Makes the NPC stop immediately.
    }

    void Resume()
    {
        if (PlayerController.instance.currentInteractable.transform.parent == null) return;
        if (PlayerController.instance.currentInteractable.transform.parent.gameObject != gameObject) return;
        behaviorGraphAgent.enabled = true;
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.acceleration = originalAcceleration;
        transform.rotation = Quaternion.Euler(0, originalYRotation, 0);
    }
}
