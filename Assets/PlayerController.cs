using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Vector3 movement;
    bool canPlayerMove = true;
    [SerializeField] float speed = 5f;
    CharacterController characterController;
    PlayerInput playerInput;
    List<GameObject> npcs = new List<GameObject>();
    [HideInInspector] public GameObject closestNPC = null;

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
    }

    void OnEnable()
    {
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Interact"].performed += OnInteract;
        PlayerInputEvent.FreezePlayer += () => canPlayerMove = false;
        PlayerInputEvent.UnFreezePlayer += () => canPlayerMove = true;
    }

    void OnDisable()
    {
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled -= OnMove;
        playerInput.actions["Interact"].performed -= OnInteract;
        PlayerInputEvent.FreezePlayer -= () => canPlayerMove = false;
        PlayerInputEvent.UnFreezePlayer -= () => canPlayerMove = true;
    }
    void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        movement = new Vector3(direction.x, 0, direction.y);
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        IdentifyClosestNPC();
        if (closestNPC == null) return;
        Debug.Log($"Player interacted with {closestNPC.name}. Hello!");
        PlayerInputEvent.OnPlayerInteract();
    }

    void Move()
    {
        if (!canPlayerMove) return;
        if (characterController)
            characterController.Move(movement * speed * Time.deltaTime);
    }
    void Update()
    {
        Move();
    }

    void IdentifyClosestNPC()
    {
        float minDistance = 100;
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
            //closestNPC.GetComponent<SampleNPC>().isClosestToPlayer = true;
            Debug.Log($"{closestNPC.name} is the closest NPC to the player.");
        }
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
