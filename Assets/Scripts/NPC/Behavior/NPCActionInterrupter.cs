using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class NPCActionInterrupter : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Animator animator;
    BehaviorGraphAgent behaviorGraphAgent;
    float originalSpeed;
    float originalAcceleration;
    float originalYRotation;
    [SerializeField] VisualEffect actionVFX;
    [SerializeField] Transform foodSpawnPosition;
    GameObject workItem;

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
        if(actionVFX != null)
        {
            actionVFX.Stop();
        }
        if (foodSpawnPosition.childCount > 0)
        {
            workItem = foodSpawnPosition.GetChild(0).gameObject;
            workItem.SetActive(false);
        }
        originalYRotation = transform.rotation.eulerAngles.y;
        behaviorGraphAgent.enabled = false;
        animator.SetBool("isBeingTalkedTo", true);
        transform.LookAt(PlayerController.instance.transform);
        navMeshAgent.speed = 0;
        navMeshAgent.acceleration = float.MaxValue; // Makes the NPC stop immediately.
    }

    void Resume()
    {
        if (PlayerController.instance.currentInteractable == null) return;
        if (PlayerController.instance.currentInteractable.transform.parent == null) return;
        if (PlayerController.instance.currentInteractable.transform.parent.gameObject != gameObject) return;
        if (actionVFX != null)
        {
            actionVFX.Play();
        }
        if (workItem != null)
        {
            workItem.SetActive(true);
            workItem = null;
        }
        behaviorGraphAgent.enabled = true;
        animator.SetBool("isBeingTalkedTo", false);
        navMeshAgent.speed = originalSpeed;
        navMeshAgent.acceleration = originalAcceleration;
        transform.rotation = Quaternion.Euler(0, originalYRotation, 0);
    }
}
