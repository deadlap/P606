using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform; 
    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 newPosition = playerTransform.position;
            transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }
}
