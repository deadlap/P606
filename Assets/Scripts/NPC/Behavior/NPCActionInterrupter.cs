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
        if (PlayerController.instance.interactNPC != gameObject) return;
        behaviorGraphAgent.enabled = false;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("resetState");
        transform.LookAt(PlayerController.instance.transform);
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = float.MaxValue; // Makes the NPC stop immediately.
    }

    void Resume()
    {
        if (PlayerController.instance.interactNPC != gameObject) return;
        behaviorGraphAgent.enabled = true;
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.acceleration = originalAcceleration;
    }
}
