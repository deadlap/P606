using UnityEngine;
using UnityEngine.InputSystem;

public class ICannotBelieveThereWasAnotherBug : MonoBehaviour
{
    [SerializeField] GameObject chair;
    [SerializeField] GameObject player;
    private void Awake()
    {
        if (!Ending.goodEndingAchieved) return;
        chair.SetActive(false);
        player.transform.position = new Vector3(-19.5f, -0.9f, -22.5f);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerInput>().enabled = true;
    }
}
