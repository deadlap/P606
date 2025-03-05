using System;
using Unity.VisualScripting;
using UnityEngine;

public class SampleNPC : MonoBehaviour
{
    Collider collider;
    bool isPlayerInTriggerZone;
    public bool isClosestToPlayer;

    void Awake()
    {
        if(GetComponent<Collider>() == null)
            Debug.LogError($"No collider found on the NPC \"{gameObject.name}\"");
        else
            collider = GetComponent<Collider>();
    }

    void OnEnable()
    {
        PlayerInputEvent.PlayerInteract += OnPlayerInteract;
    }

    void OnDisable()
    {
        PlayerInputEvent.PlayerInteract -= OnPlayerInteract;
    }

    void OnPlayerInteract()
    {
        if (!isPlayerInTriggerZone || !isClosestToPlayer) return;
        Debug.Log($"Hello I'm {gameObject.name}.");
        //interact with the NPC
        isClosestToPlayer = false;
    }

    void Start()
    {
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTriggerZone = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTriggerZone = false;
        }
    }
}
