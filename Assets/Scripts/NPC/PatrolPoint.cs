using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public bool hasPlate;
    public bool isSingleSeat;
    public bool isSeat;
    public bool isReserved;
    public bool isBeingServed;
    public bool hasBeenServed;
    public int patrolPointIndex;
    public GameObject occupant;
    int NPCCount;

    public GameObject plate;
    GameObject seat;
    void Awake()
    {
        if(GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
        if (hasPlate)
        {
            plate = FindClosestWithTag("Plate");
        }
        if (isSingleSeat)
        {
            seat = FindClosestWithTag("Seat");
            transform.position = seat.transform.position;
        }
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

    public GameObject FindClosestWithTag(string tag)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(currentPosition, target.transform.position);
            if (distance < minDistance)
            {
                closest = target;
                minDistance = distance;
            }
        }

        return closest;
    }
}
