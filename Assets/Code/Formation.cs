using UnityEngine;
using System.Collections;
using System;

public class Formation : SteeringBehaviour
{
    public Boid leaderBoid;
    public GameObject leader;
    private Vector3 offset;
    private Vector3 targetPos;
    public bool useDeadReconing = false;


    public void Start()
    {
        if (leader  != null)
        {
            leaderBoid = leader.GetComponentInChildren<Boid>();
            offset = transform.position - leader.transform.position;
            offset = Quaternion.Inverse(transform.rotation) * offset;
        }
    }

    public void OnDrawGizmos()
    {
        if (isActiveAndEnabled && leaderBoid != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, leaderBoid.transform.position);
        }
    }

    public override Vector3 Calculate()
    {
        if (leaderBoid != null)
        {
            targetPos = leaderBoid.TransformPoint(offset);
            targetPos.y = leaderBoid.position.y + offset.y;

            if (useDeadReconing)
            {
                float dist = Vector3.Distance(boid.position, leaderBoid.position);
                float lookAhead = (dist / boid.maxSpeed);
                targetPos = targetPos + (lookAhead * leaderBoid.velocity);
            }
            /*
            /*float pitchForce = target.y - position.y;
            pitchForce *= (1.0f - pitchForceScale);
            target.y -= pitchForce;

            offsetPursuitTargetPos = target;

            Utilities.checkNaN(target);
            */
            return boid.ArriveForce(targetPos, boid.maxSpeed / 2, 5.0f);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
