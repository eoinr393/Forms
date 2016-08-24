using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class JitterWander: SteeringBehaviour
{
    [Range(0.0f, 100.0f)]
    public float wanderRadius = 10.0f;

    [Range(0.0f, 100.0f)]
    public float wanderJitter = 20.0f;

    [Range(0.0f, 100.0f)]
    public float wanderDistance = 15.0f;

    private Vector3 wanderTargetPos;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 wanderCircleCenter = Utilities.TransformPointNoScale(Vector3.forward * wanderDistance, transform);
        Gizmos.DrawWireSphere(wanderCircleCenter, wanderRadius);
        Gizmos.color = Color.green;
        Vector3 worldTarget = Utilities.TransformPointNoScale(wanderTargetPos + Vector3.forward * wanderDistance, transform);
        Gizmos.DrawLine(transform.position, worldTarget);
    }

    public void Start()
    {
        wanderTargetPos = Utilities.RandomInsideUnitSphere() * wanderRadius;
    }

    public override Vector3 Calculate()
    {
        float jitterTimeSlice = wanderJitter * boid.TimeDelta;

        Vector3 toAdd = Utilities.RandomInsideUnitSphere() * jitterTimeSlice;
        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;

        Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
        Vector3 worldTarget = boid.TransformPoint(localTarget);
        return (worldTarget - boid.position);
    }
}
