using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NoiseWander: SteeringBehaviour
{
    public Boid enemy = null;
    private Vector3 wanderTargetPos = Vector3.zero;
    [Range(0.0f, 100.0f)]
    public float radius = 10.0f;

    [Range(0.0f, 100.0f)]
    public float distance = 15.0f;

    [Range(0.001f, 1.0f)]
    public float noisiness = 0.5f;

    private float noise = 0.0f;
    
    public override Vector3 Calculate()
    {
        float n = Mathf.PerlinNoise(noise, 0);
        float theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);
        wanderTargetPos.x = Mathf.Sin(theta);
        wanderTargetPos.z = -Mathf.Cos(theta);

        n = Mathf.PerlinNoise(noise, 0);
        theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);

        wanderTargetPos.y = 0;
        wanderTargetPos *= radius;
        Vector3 localTarget = wanderTargetPos + (Vector3.forward * distance);
        Vector3 worldTarget = boid.TransformPoint(localTarget);

        noise += noisiness * boid.TimeDelta;
        Vector3 desired = worldTarget - boid.position;
        desired.Normalize();
        desired *= boid.maxSpeed;
        //return Vector3.zero;
        return desired - boid.velocity;
    }
}