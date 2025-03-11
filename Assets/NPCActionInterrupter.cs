using UnityEngine;
using UnityEngine.AI;

public class NPCActionInterrupter : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    float originalSpeed;
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
        navMeshAgent.speed = 0;
    }

    void Resume()
    {
        if (PlayerController.instance.closestNPC != gameObject) return;
        navMeshAgent.speed = originalSpeed;
    }
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        originalSpeed = navMeshAgent.speed;
    }
}
