using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class JitterWander: SteeringBehaviour
{
    public float wanderRadius = 10.0f;
    public float wanderJitter = 20.0f;
    public float wanderDistance = 15.0f;

    private Vector3 wanderTargetPos;


    public override Vector3 Calculate()
    {
        float jitterTimeSlice = wanderJitter * boid.TimeDelta;

        Vector3 toAdd = Utilities.RandomInsideUnitSphere() * jitterTimeSlice;
        wanderTargetPos += toAdd;
        wanderTargetPos.Normalize();
        wanderTargetPos *= wanderRadius;

        Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
        Vector3 worldTarget = boid.TransformPoint(localTarget);
        return (worldTarget - position);
    }
}
