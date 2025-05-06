using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Vector3 movement;
    bool canPlayerAct = true;
    [SerializeField] float movementSpeed = 5f;
    CharacterController characterController;
    PlayerInput playerInput;
    List<GameObject> interactables = new();
    [SerializeField] GameObject interactButtonPrefab;
    GameObject currentInteractButton;
    Image interactButtonFillImage;
    [HideInInspector] public GameObject currentInteractable = null;
    GameObject closestInteractable = null;
    [SerializeField] float interactFillTime = 0.5f;
    Vector3 interactButtonOffset = new Vector3(0, 0.8f, 0);
    Animator animator;
    [HideInInspector] public AudioSource audioSource;
    Vector2 moveDirection;
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (GetComponent<CharacterController>() == null)
            characterController = gameObject.AddComponent<CharacterController>();
        else
            characterController = GetComponent<CharacterController>();
        
        if(GetComponent<PlayerInput>() == null)
            playerInput = gameObject.AddComponent<PlayerInput>();
        else
            playerInput = GetComponent<PlayerInput>();
        if(GetComponentInChildren<Animator>() == null)
            Debug.LogWarning("PlayerController: No Animator component found on the player object.");
        else
            animator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
        // Double movespeed in the editor
        movementSpeed *= 2f;
#endif
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Interact"].performed += OnInteract;
        PlayerInputEvent.EnterDialog += () => canPlayerAct = false;
        PlayerInputEvent.ExitDialog += () => canPlayerAct = true;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
        PlayerInputEvent.EnterDialog -= () => canPlayerAct = false;
        PlayerInputEvent.ExitDialog -= () => canPlayerAct = true;
    }
    void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        
        Vector2 direction = new(moveDirection.x, moveDirection.y);
        movement = new(direction.x, 0, direction.y);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if(!(GameStats.INSTANCE.IntroPlayed || IsDeadNPC())) return; // Check if the intro has been played
        if(GameStats.INSTANCE.IntroPlayed && IsDeadNPC()) return;
        if (closestInteractable == null) return;
        if (!canPlayerAct) return;
        StartCoroutine(Interact(context));
    }

    public void FreezePlayer(bool freeze)
    {
        canPlayerAct = !freeze;
    }

    void Move()
    {
        if (!canPlayerAct) return;
        if (characterController)
            characterController.Move(movement * movementSpeed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("walkSpeed", movementSpeed);

        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    void Rotate()
    {
        if (!canPlayerAct) return;
        Vector3 rotationDirection = new Vector3(moveDirection.x, 0f, moveDirection.y).normalized;
        if (rotationDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * movementSpeed);
        }
    }

    IEnumerator Interact(InputAction.CallbackContext context)
    {
        currentInteractable = closestInteractable;
        if(currentInteractButton == null) yield break;
        var start = interactButtonFillImage.fillAmount;
        var end = 1f;
        float elapsedTime = 0;
        // while the player holds the interact button, the fill amount of the interact button increases.
        while (context.ReadValueAsButton() && elapsedTime < end)
        {
            yield return null;
            elapsedTime += Time.deltaTime / interactFillTime;
            if (currentInteractButton != null)
                interactButtonFillImage.fillAmount = Mathf.Lerp(start, end, elapsedTime);
        }
        // if the player releases the interact button before the fill amount reaches 1, the interaction is cancelled.
        if (!context.ReadValueAsButton())
        {
            if (currentInteractButton != null) 
                interactButtonFillImage.fillAmount = 0;
            yield break;
        }
        // if the player holds the interact button until the fill amount reaches 1, the interaction is successful.
        if (currentInteractButton != null)
            interactButtonFillImage.fillAmount = 0;
        PlayerInputEvent.OnPlayerInteract();
        animator.SetBool("isWalking", false);
    }
    void Update()
    {
        Move();
        Rotate();
        if (interactables.Count > 0)
        {
            IdentifyClosestInteractable();
            CreateInteractButton();
            SetInteractButtonPosition();
        }
        if (interactables.Count <= 0)
            DestroyInteractButton();
        if(IsDeadNPC() && GameStats.INSTANCE.IntroPlayed)
            DestroyInteractButton();
    }

    void IdentifyClosestInteractable()
    {
        if(!canPlayerAct) return;
        float minDistance = float.MaxValue;
        if (interactables.Count == 0)
        {
            closestInteractable = null;
        }
        else
        {
            for (int i = 0; i < interactables.Count; i++)
            {
                if (interactables[i] == null) {
                    interactables.RemoveAt(i);
                    i--;
                    continue;
                };
                float distance = Vector3.Distance(transform.position, interactables[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestInteractable = interactables[i];
                }
            }
        }
    }

    void CreateInteractButton()
    {
        if (!canPlayerAct) return;
        if(!(GameStats.INSTANCE.IntroPlayed || IsDeadNPC())) return;
        if(GameStats.INSTANCE.IntroPlayed && IsDeadNPC()) return;
        if (closestInteractable == null) return;
        if(currentInteractButton != null) return;
        currentInteractButton = Instantiate(interactButtonPrefab, closestInteractable.transform.position, Quaternion.identity);
        interactButtonFillImage = currentInteractButton.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();
    }

    void SetInteractButtonPosition()
    {
        if (closestInteractable == null) return;
        if (currentInteractButton != null)
        {
            if(closestInteractable.transform.parent.tag == "NPC")
            {
                currentInteractButton.transform.SetParent(closestInteractable.transform.parent.GetChild(0).GetChild(0));
            }
            else
            {
                currentInteractButton.transform.SetParent(closestInteractable.transform);
            }
        currentInteractButton.transform.localPosition = interactButtonOffset;
        }
    }

    void DestroyInteractButton()
    {
        if (currentInteractButton != null)
            Destroy(currentInteractButton);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            interactables.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            interactables.Remove(other.gameObject);
        }
    }
    bool IsDeadNPC() {
        if (closestInteractable == null) return false;
        if (closestInteractable.transform.parent == null) return false;
        return closestInteractable.transform.parent == GameStats.INSTANCE.Victim.transform;
    }
}
