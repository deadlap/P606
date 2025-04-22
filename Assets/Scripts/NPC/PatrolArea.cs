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
        var randomPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        radius = randomPatrolPoint.GetComponent<SphereCollider>().radius;
        var randomPoint = Random.insideUnitCircle;
        var pointPosition = new Vector3(randomPoint.x * radius, 1, randomPoint.y * radius);
        pointPosition += randomPatrolPoint.transform.position;

        pointObj = new("RandomPatrolPoint");
        pointObj.transform.position = pointPosition;
        return pointObj.transform;
    }

    public Transform FindRandomUnreservedPoint()
    {
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
        var randomPosition = availablePoints[Random.Range(0, availablePoints.Count)];
        randomPosition.GetComponent<PatrolPoint>().isReserved = true;
        randomPosition.GetComponent<PatrolPoint>().isBeingServed = false;
        randomPosition.GetComponent<PatrolPoint>().hasBeenServed = false;
        return randomPosition;
    }
    public Transform FindGuestToServe(bool isBringingFood = false)
    {
        guestPoints.Clear();
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            var patrolPoint = patrolPoints[i].GetComponent<PatrolPoint>();
            if (patrolPoint.isReserved &&
                patrolPoint.isBeingServed == isBringingFood &&
                !patrolPoint.hasBeenServed &&
                guestPoints.Contains(patrolPoints[i]) == false)
            {
                guestPoints.Add(patrolPoints[i]);
            }
        }
        if(guestPoints.Count == 0) return FindRandomUnreservedPoint();
        var guestPosition = guestPoints[Random.Range(0, guestPoints.Count)];
        return guestPosition;
    }

    public Transform FindPlaceInQueue()
    {
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            queuePoints.Add(patrolPoints[i]);
            var patrolPoint = patrolPoints[i].GetComponent<PatrolPoint>();
            patrolPoint.patrolPointIndex = i;
            if (!patrolPoint.isReserved)
            {
                patrolPoint.isReserved = true;
                return patrolPoints[i];
            }
        }
        return RandomPatrolPoint();
    }

    public Transform FindGuestInQueue()
    {
        var patrolPoint = patrolPoints[0].GetComponent<PatrolPoint>();
        if (patrolPoint.isReserved)
        {
            return patrolPoints[0];
        }
        return FindRandomUnreservedPoint();
    }
}
