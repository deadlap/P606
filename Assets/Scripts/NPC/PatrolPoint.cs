using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public bool isReserved;
    public bool isBeingServed;
    public bool hasBeenServed;
    public int patrolPointIndex;
    int NPCCount;

    void Awake()
    {
        if(GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isReserved ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPCCount++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            isReserved = NPCCheck();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPCCount--;
            if (NPCCount <= 0)
            {
                NPCCount = 0;
                isReserved = NPCCheck();
                isBeingServed = NPCCheck();
            }
        }
    }

    bool NPCCheck()
    {
        return NPCCount > 0;
    }
}
