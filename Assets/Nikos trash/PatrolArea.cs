using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolArea : MonoBehaviour
{
    public List<Transform> patrolPoints;
    public float radius;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform point = transform.GetChild(i);
            patrolPoints.Add(point);
        }
    }
    
    public Vector3 RandomPatrolPoint()
    {
        var randomPatrolPoint = patrolPoints[Random.Range(0, patrolPoints.Count)];
        radius = randomPatrolPoint.GetComponent<SphereCollider>().radius;
        var randomPoint = Random.insideUnitCircle;
        var pointPosition = new Vector3(randomPoint.x * radius, 1, randomPoint.y * radius);
        pointPosition += randomPatrolPoint.transform.position;
        return pointPosition;
    }
}
