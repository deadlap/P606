using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public bool isSeat;
    public bool isReserved;
    public bool isBeingServed;
    public bool hasBeenServed;
    public int patrolPointIndex;
    public GameObject occupant;
    int NPCCount;

    void Awake()
    {
        if(GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isReserved ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == occupant)
        {
            NPCCount++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == occupant)
        {
            isReserved = NPCCheck();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == occupant)
        {
            NPCCount--;
            if (NPCCount <= 0)
            {
                NPCCount = 0;
                isReserved = NPCCheck();
                isBeingServed = NPCCheck();
                hasBeenServed = NPCCheck();
                occupant = null;
            }
        }
    }

    bool NPCCheck()
    {
        return NPCCount > 0;
    }
}
