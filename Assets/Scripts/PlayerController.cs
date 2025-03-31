using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Vector3 movement;
    bool canPlayerAct = true;
    [SerializeField] float speed = 5f;
    CharacterController characterController;
    PlayerInput playerInput;
    List<GameObject> npcs = new List<GameObject>();
    [SerializeField] GameObject interactButtonPrefab;
    GameObject currentInteractButton;
    Image interactButtonFillImage;
    [HideInInspector] public GameObject closestNPC = null;
    [SerializeField] float interactFillTime = 0.5f;
    Animator animator;

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
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Interact"].performed += OnInteract;
        PlayerInputEvent.FreezePlayer += () => canPlayerAct = false;
        PlayerInputEvent.UnFreezePlayer += () => canPlayerAct = true;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
        PlayerInputEvent.FreezePlayer -= () => canPlayerAct = false;
        PlayerInputEvent.UnFreezePlayer -= () => canPlayerAct = true;
    }
    void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        
        Vector2 direction = new(moveDirection.x, moveDirection.y);
        movement = new(direction.x, 0, direction.y);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        if (closestNPC == null) return;
        if (!canPlayerAct) return;
        StartCoroutine(Interact(context));
    }

    void Move()
    {
        if (!canPlayerAct) return;
        if (characterController)
            characterController.Move(movement * speed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            animator.SetBool("isWalking", true);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }
    }

    IEnumerator Interact(InputAction.CallbackContext context)
    {
        if(interactButtonFillImage == null) yield break;
        var start = interactButtonFillImage.fillAmount;
        var end = 1f;
        float elapsedTime = 0;
        // while the player holds the interact button, the fill amount of the interact button increases.
        while (context.ReadValueAsButton() && elapsedTime < end)
        {
            if (interactButtonFillImage == null) yield break;
            yield return null;
            elapsedTime += Time.deltaTime / interactFillTime;
            interactButtonFillImage.fillAmount = Mathf.Lerp(start, end, elapsedTime);
        }
        // if the player releases the interact button before the fill amount reaches 1, the interaction is cancelled.
        if (!context.ReadValueAsButton())
        {
            interactButtonFillImage.fillAmount = 0;
            yield break;
        }
        // if the player holds the interact button until the fill amount reaches 1, the interaction is successful.
        start = end;
        interactButtonFillImage.fillAmount = 0;
        PlayerInputEvent.OnPlayerInteract();
        Debug.Log($"Player interacted with {closestNPC.name}. Hello!");
    }
    void Update()
    {
        Move();
        Rotate();
        if (npcs.Count > 0)
        {
            IdentifyClosestNPC();
            CreateInteractButton();
            SetInteractButtonPosition();
        }
        if (npcs.Count == 0)
            DestroyInteractButton();
    }

    void IdentifyClosestNPC()
    {
        float minDistance = float.MaxValue;
        if (npcs.Count == 0)
        {
            closestNPC = null;
        }
        else
        {
            for (int i = 0; i < npcs.Count; i++)
            {
                float distance = Vector3.Distance(transform.position, npcs[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestNPC = npcs[i];
                }
            }
            // Debug.Log($"{closestNPC.name} is the closest NPC to the player.");
        }
    }

    void CreateInteractButton()
    {
        if (closestNPC == null) return;
        if(currentInteractButton != null) return;
        currentInteractButton = Instantiate(interactButtonPrefab, closestNPC.transform.position, Quaternion.identity);
        interactButtonFillImage = currentInteractButton.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>();
        print(interactButtonFillImage.name); 
    }

    void SetInteractButtonPosition()
    {
        if (closestNPC == null) return;
        if (currentInteractButton != null)
        {
            currentInteractButton.transform.SetParent(closestNPC.transform);
            currentInteractButton.transform.localPosition = new Vector3(0, 2, 0);
        }
    }

    void DestroyInteractButton()
    {
        if (currentInteractButton != null)
            Destroy(currentInteractButton);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcs.Add(other.gameObject);
            Debug.Log($"Player entered the NPC {other.gameObject.name}'s trigger zone");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcs.Remove(other.gameObject);
            Debug.Log($"Player exited the NPC {other.gameObject.name}'s trigger zone");
        }
    }
}
