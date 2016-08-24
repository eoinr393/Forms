using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class NoiseWander: SteeringBehaviour
{
    private Vector3 target = Vector3.zero;
    [Range(0.0f, 100.0f)]
    public float radius = 50.0f;

    [Range(0.0f, 100.0f)]
    public float distance = 5.0f;

    [Range(0.001f, 1.0f)]
    public float noisiness = 0.2f;

    private float noise = 0.0f;

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 wanderCircleCenter = Utilities.TransformPointNoScale(Vector3.forward * distance, transform);
        Gizmos.DrawWireSphere(wanderCircleCenter, radius);
        Gizmos.color = Color.green;
        Vector3 worldTarget = Utilities.TransformPointNoScale(target + Vector3.forward * distance, transform);
        Gizmos.DrawLine(transform.position, worldTarget);
    }

    public override Vector3 Calculate()
    {
        float n = Mathf.PerlinNoise(noise, 0);
        float theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);
        target.x = Mathf.Sin(theta);
        target.z = -Mathf.Cos(theta);

        n = Mathf.PerlinNoise(noise, 0);
        theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);

        target.y = 0;
        target *= radius;
        Vector3 localTarget = target + (Vector3.forward * distance);
        Vector3 worldTarget = boid.TransformPoint(localTarget);

        noise += noisiness * boid.TimeDelta;
        Vector3 desired = worldTarget - boid.position;
        desired.Normalize();
        desired *= boid.maxSpeed;
        //return Vector3.zero;
        return desired - boid.velocity;
    }
}