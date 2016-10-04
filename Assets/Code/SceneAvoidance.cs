using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

struct FeelerInfo
{
    public Vector3 point;
    public Vector3 normal;
    public bool collided;
    public FeeelerType feelerType;
    public enum FeeelerType { front, side };
    
    public FeelerInfo(Vector3 point, Vector3 normal, bool collided, FeeelerType feelerType)
    {
        this.point = point;
        this.normal = normal;
        this.collided = collided;
        this.feelerType = feelerType;
    }
}

public class SceneAvoidance: SteeringBehaviour
{
    public float scale = 4.0f;
    public float forwardFeelerDepth = 30;
    public float sideFeelerDepth = 15;
    FeelerInfo[] feelers = new FeelerInfo[5];

    public float frontFeelerUpdatesPerSecond = 10.0f;
    public float sideFeelerUpdatesPerSecond = 5.0f;

    public float feelerRadius = 2.0f;
    public enum ForceType { normal, incident, braking };
    public ForceType forceType = ForceType.normal;

    public void Start()
    {
        StartCoroutine(UpdateFrontFeelers());
        StartCoroutine(UpdateSideFeelers());
    }

    public void OnDrawGizmos()
    {
        foreach (FeelerInfo feeler in feelers)
        {
            
            if (feeler.collided)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, feeler.point);

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(feeler.point, feeler.point + (feeler.normal * 5));
                Gizmos.color = Color.red;
                Gizmos.DrawLine(feeler.point, feeler.point + (boid.force / 100.0f));
            }
        }
    }

    public override Vector3 Calculate()
    {
        Vector3 force = Vector3.zero;

        for (int i = 0; i < feelers.Length; i++)
        {
            FeelerInfo info = feelers[i];
            if (info.collided)
            {
                force += CalculateSceneAvoidanceForce(info);
            }
        }
        return force;
    }

    System.Collections.IEnumerator UpdateFrontFeelers()
    {
        while (true)
        {
            RaycastHit info;
            float forwardFeelerDepth = this.forwardFeelerDepth + ((boid.velocity.magnitude / boid.maxSpeed) * this.forwardFeelerDepth);

            // Forward feeler
            //int layerMask = 1 << 9;
            bool collided = Physics.SphereCast(transform.position, feelerRadius, boid.TransformDirection(Vector3.forward), out info, forwardFeelerDepth);
            feelers[0] = new FeelerInfo(info.point, info.normal
                , collided, FeelerInfo.FeeelerType.front);
            yield return new WaitForSeconds(1.0f / frontFeelerUpdatesPerSecond);
        }
    }

    Vector3 CalculateSceneAvoidanceForce(FeelerInfo info)
    {
        Vector3 force = Vector3.zero;

        Vector3 fromTarget = fromTarget = boid.position - info.point;
        float dist = Vector3.Distance(boid.position, info.point);

        switch (forceType)
        {
            case ForceType.normal:
                force = info.normal * (forwardFeelerDepth * scale / dist);
                break;
            case ForceType.incident:
                fromTarget.Normalize();
                force -= Vector3.Reflect(fromTarget, info.normal) * (forwardFeelerDepth / dist);
                break;
            case ForceType.braking:
                force += fromTarget * (forwardFeelerDepth / dist);
                break;
        }
        return force;
    }

    System.Collections.IEnumerator UpdateSideFeelers()
    {
        while (true)
        {
            Vector3 feelerDirection;
            RaycastHit info;
            bool collided;

            float sideFeelerDepth = this.sideFeelerDepth + ((boid.velocity.magnitude / boid.maxSpeed) * this.sideFeelerDepth);


            // Left feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.up) * feelerDirection;
            collided = Physics.SphereCast(transform.position, feelerRadius, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            feelers[1] = new FeelerInfo(info.point, info.normal,
                collided, FeelerInfo.FeeelerType.side);

            // Right feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.up) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            feelers[2] = new FeelerInfo(info.point, info.normal
                , collided, FeelerInfo.FeeelerType.side);

            // Up feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(45, Vector3.right) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            feelers[3] = new FeelerInfo(info.point, info.normal
                , collided, FeelerInfo.FeeelerType.side);

            // Down feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.right) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            feelers[4] = new FeelerInfo(info.point, info.normal
                , collided, FeelerInfo.FeeelerType.side);
            yield return new WaitForSeconds(1.0f / sideFeelerUpdatesPerSecond);
        }
    }
    
}