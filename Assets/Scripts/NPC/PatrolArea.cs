using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolArea : MonoBehaviour
{
    public List<Transform> patrolPoints = new();
    public List<Transform> availablePoints = new();
    public List<Transform> queuePoints = new();
    public List<Transform> guestPoints = new();
    public float radius;
    GameObject pointObj;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var point = transform.GetChild(i);
            patrolPoints.Add(point);
        }
    }

    public Transform RandomPatrolPoint()
    {
        if (pointObj != null)
        {
            Destroy(pointObj);
        }
        SphereCollider patrolPointSphere;
        while (true)
        {
            if (patrolPoints[Random.Range(0, patrolPoints.Count)].TryGetComponent<SphereCollider>
                (out patrolPointSphere)) break;
        }
        radius = patrolPointSphere.radius;
        var randomPoint = Random.insideUnitCircle;
        var pointPosition = new Vector3(randomPoint.x * radius, 1, randomPoint.y * radius);
        pointPosition += patrolPointSphere.transform.position;

        pointObj = new("RandomPatrolPoint");
        pointObj.transform.position = pointPosition;
        pointObj.transform.rotation = patrolPointSphere.transform.rotation;
        return pointObj.transform;
    }

    public Transform FindRandomUnreservedPoint(GameObject occupant)
    {
        if(availablePoints != null)
            availablePoints.Clear();
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            var patrolPoint = patrolPoints[i].GetComponent<PatrolPoint>();
            if (!patrolPoint.isReserved && 
                !availablePoints.Contains(patrolPoints[i]))
            {
                availablePoints.Add(patrolPoints[i]);
            }
        }
        if (availablePoints.Count == 0) return RandomPatrolPoint();
        var randomPosition = availablePoints[Random.Range(0, availablePoints.Count)].GetComponent<PatrolPoint>();
        randomPosition.isReserved = true;
        randomPosition.isBeingServed = false;
        randomPosition.hasBeenServed = false;
        randomPosition.occupant = occupant;
        return randomPosition.transform;
    }
    public Transform FindGuestToServe(GameObject occupant, bool isBringingFood = false)
    {
        if(guestPoints != null)
            guestPoints.Clear();
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            var patrolPoint = patrolPoints[i].GetComponent<PatrolPoint>();
            if (patrolPoint.isReserved &&
                patrolPoint.occupant != null &&
                patrolPoint.isBeingServed == isBringingFood &&
                !patrolPoint.hasBeenServed &&
                guestPoints.Contains(patrolPoints[i]) == false)
            {
                guestPoints.Add(patrolPoints[i]);
            }
        }
        if(guestPoints.Count == 0) return null;
        var transform = guestPoints[Random.Range(0, guestPoints.Count)];
        transform.GetComponent<PatrolPoint>().occupant = occupant;
        return transform;
    }

    public Transform FindPlaceInQueue(GameObject occupant)
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (!queuePoints.Contains(patrolPoints[i]))
            {
                queuePoints.Add(patrolPoints[i]);
            }
            var patrolPoint = patrolPoints[i].GetComponent<PatrolPoint>();
            patrolPoint.patrolPointIndex = i;
            if (!patrolPoint.isReserved)
            {
                patrolPoint.isReserved = true;
                patrolPoint.occupant = occupant;
                return patrolPoints[i];
            }
        }
        return RandomPatrolPoint();
    }

    public Transform FindGuestInQueue(GameObject occupant)
    {
        var patrolPoint = patrolPoints[0].GetComponent<PatrolPoint>();
        if (patrolPoint.isReserved)
        {
            return patrolPoints[0];
        }
        return FindRandomUnreservedPoint(occupant);
    }
}
