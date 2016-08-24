using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Wiggle: SteeringBehaviour
{
    [Range(0.0f, 360.0f)]
    public float speed = 30;
    public float amplitude = 50;
    public Axis direction = Axis.Horizontal;
    public enum Axis { Horizontal, Vertical };

    [HideInInspector]
    public float wiggleTheta = 0.0f;
    private Vector3 target = Vector3.zero;

    [HideInInspector]
    public float rampedWiggleSpeed = 0;
    [HideInInspector]
    public float rampedWiggleAmplitude = 0;
    [Range(0.0f, 100.0f)]
    public float radius = 50.0f;

    [Range(0.0f, 100.0f)]
    public float distance = 5.0f;

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 170);
        Vector3 wanderCircleCenter = Utilities.TransformPointNoScale(Vector3.forward * distance, transform);
        Gizmos.DrawWireSphere(wanderCircleCenter, radius);
        Gizmos.color = new Color(0, 170, 0);
        Vector3 worldTarget = Utilities.TransformPointNoScale(target + Vector3.forward * distance, transform);
        Gizmos.DrawLine(transform.position, worldTarget);
    }

    public override Vector3 Calculate()
    {
        float n = Mathf.Sin(wiggleTheta);

        rampedWiggleAmplitude = Mathf.Lerp(rampedWiggleAmplitude, amplitude, 1);

        float t = Utilities.Map(n, -1.0f, 1.0f, -rampedWiggleAmplitude, rampedWiggleAmplitude);
        float theta = Mathf.Sin(Utilities.DegreesToRads(t));

        if (direction == Axis.Horizontal)
        {
            target.x = Mathf.Sin(theta);
            target.z = Mathf.Cos(theta);
            target.y = 0;
        }
        else
        {
            target.y = Mathf.Sin(theta);
            target.z = Mathf.Cos(theta);
            target.x = 0;
        }

        target *= radius;
        Vector3 yawRoll = transform.rotation.eulerAngles;
        yawRoll.x = 0;
        Vector3 localTarget = target + (Vector3.forward * distance);
        Vector3 worldTarget = boid.TransformPoint(localTarget);

        Vector3 worldTargetOnY = transform.position + Quaternion.Euler(yawRoll) * localTarget;
        rampedWiggleSpeed = Mathf.Lerp(rampedWiggleSpeed, speed, Time.deltaTime * 2.0f);
        wiggleTheta += boid.TimeDelta * rampedWiggleSpeed * Mathf.Deg2Rad;
        //wiggleTheta += ThreadTimeDelta() * wiggleAngularSpeedDegrees * Mathf.Deg2Rad;
        if (wiggleTheta > Utilities.TWO_PI)
        {
            wiggleTheta = Utilities.TWO_PI - wiggleTheta;
        }

        return boid.SeekForce(worldTargetOnY);
    }
}