using UnityEngine;
using System.Collections;
using System;

public class Formation : SteeringBehaviour
{
    private Boid leaderBoid;
    public GameObject leader;
    private Vector3 offset;
    private Vector3 targetPos;

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
        }
    }

    public override Vector3 Calculate()
    {
        if (leaderBoid != null)
        {
            Vector3 targetPos = Vector3.zero;

            targetPos = leaderBoid.TransformPoint(offset);

            float dist = (targetPos - boid.position).magnitude;

            float lookAhead = (dist / boid.maxSpeed);

            targetPos = targetPos + (lookAhead * leaderBoid.velocity);

            /*float pitchForce = target.y - position.y;
            pitchForce *= (1.0f - pitchForceScale);
            target.y -= pitchForce;

            offsetPursuitTargetPos = target;

            Utilities.checkNaN(target);
            */
            return boid.SeekForce(targetPos);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
