using UnityEngine;
using UnityEngine.AI;

public class NPCActionInterrupter : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    float originalSpeed;
    float orginalAcceleration;
    void OnEnable()
    {
        PlayerInputEvent.FreezePlayer += Interrupt;
        PlayerInputEvent.UnFreezePlayer += Resume;
    }

    void OnDisable()
    {
        PlayerInputEvent.FreezePlayer -= Interrupt;
        PlayerInputEvent.UnFreezePlayer -= Resume;
    }

    void Interrupt()
    {
        if (PlayerController.instance.closestNPC != gameObject) return;
        transform.LookAt(PlayerController.instance.transform);
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = float.MaxValue; // Makes the NPC stop immediately.
    }

    void Resume()
    {
        if (PlayerController.instance.closestNPC != gameObject) return;
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.acceleration = orginalAcceleration;
    }
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalSpeed = navMeshAgent.speed;
    }
}
