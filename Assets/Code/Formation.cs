using UnityEngine;
using System.Collections;
using System;

public class Formation : SteeringBehaviour
{
    public Boid leader;
    private Vector3 offset;
    private Vector3 targetPos;

    public void Start()
    {
        if (leader != null)
        {
            offset = transform.position - leader.transform.position;
            offset = Quaternion.Inverse(transform.rotation) * offset;
        }
    }

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

    public override Vector3 Calculate()
    {
        Vector3 target = Vector3.zero;

        target = leader.TransformPoint(offset);
        
        float dist = (target - boid.position).magnitude;

        float lookAhead = (dist / boid.maxSpeed);

        target = target + (lookAhead * leader.velocity);

        /*float pitchForce = target.y - position.y;
        pitchForce *= (1.0f - pitchForceScale);
        target.y -= pitchForce;

        offsetPursuitTargetPos = target;

        Utilities.checkNaN(target);
        */
        return boid.ArriveForce(target);
    }
}
