﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

struct SceneAvoidanceFeelerInfo
{
    public Vector3 point;
    public Vector3 normal;
    public bool collided;
    public FeeelerType feelerType;
    public enum FeeelerType { front, side };
    
    public SceneAvoidanceFeelerInfo(Vector3 point, Vector3 normal, bool collided, FeeelerType feelerType)
    {
        this.point = point;
        this.normal = normal;
        this.collided = collided;
        this.feelerType = feelerType;
    }
}

public class Boid : MonoBehaviour
{    
    // Need these because we might be running on a thread and can't touch the transform
    [Header("Transform")]    
    public Vector3 position = Vector3.zero;
    public Vector3 forward = Vector3.forward;
    public Vector3 up = Vector3.up;
    public Vector3 right = Vector3.right;
    public Quaternion rotation = Quaternion.identity;

    private Vector3 tempUp;

    // Variables required to implement the boid
    [Header("Boid Attributes")]
    public float mass = 1.0f;
    public float maxSpeed = 20.0f;
    public float maxForce = 10.0f;
    public float weight = 1.0f;
    [Range(0.0f, 2.0f)]
    public float timeMultiplier = 1.0f;
    [Range(0.0f, 1.0f)]
    public float damping = 0.01f;
    public enum CalculationMethods { WeightedTruncatedSum, WeightedTruncatedRunningSumWithPrioritisation, PrioritisedDithering };
    public CalculationMethods calculationMethod = CalculationMethods.WeightedTruncatedRunningSumWithPrioritisation;
    public float radius = 5.0f;
    public float maxTurnDegrees = 180.0f;
    public bool applyBanking = true;
    public float straighteningTendancy = 0.2f;        
    public bool integrateForces = true;
    public float preferredTimeDelta = 0.0f;

    [HideInInspector]
    public Flock flock;

    public bool enforceNonPenetrationConstraint;

    [HideInInspector]
    public Vector3 force = Vector3.zero;

    [HideInInspector]
    public Vector3 velocity = Vector3.zero;

    [HideInInspector]
    public Vector3 acceleration;

    [Header("Gravity")]
    public bool applyGravity = false;
    public Vector3 gravity = new Vector3(0, -9, 0);
       
    [HideInInspector]
    public bool multiThreadingEnabled = false;

    [HideInInspector]
    public SteeringBehaviour[] behaviours;

    public float TimeDelta
    {
        get
        {
            float flockMultiplier = (flock == null) ? 1 : flock.timeMultiplier;
            float timeDelta = multiThreadingEnabled ? flock.threadTimeDelta : Time.deltaTime;
            return timeDelta * flockMultiplier * timeMultiplier;
        }
    }       

    void Start()
    {
        desiredPosition = transform.position;
        timeAcc = preferredTimeDelta;
        UpdateLocalFromTransform();

        behaviours = GetComponents<SteeringBehaviour>();
    }

    #region Integration


    public void UpdateLocalFromTransform()
    {
        position = transform.position;
        up = transform.up;
        right = transform.right;
        forward = transform.forward;
        rotation = transform.rotation;
    }

    public Vector3 TransformDirection(Vector3 direction)
    {
        return rotation * direction;
    }

    public Vector3 TransformPoint(Vector3 localPoint)
    {
        Vector3 p = rotation * localPoint;
        p += position;
        return p;
    }

    float timeAcc = 0;
    Vector3 desiredPosition = Vector3.zero;

    void FixedUpdate()
    {
        float smoothRate;

        if (!multiThreadingEnabled)
        {
            UpdateLocalFromTransform();
            force = CalculateForce();
        }

        timeAcc += Time.deltaTime;
            
        if (timeAcc > preferredTimeDelta)
        {                
            float timeAccMult = timeAcc * timeMultiplier;
            if (flock != null)
            {
                timeAccMult *= flock.timeMultiplier;
            }
            Vector3 newAcceleration = force / mass;
            if (timeAcc > 0.0f)
            {
                smoothRate = Utilities.Clip(9.0f * timeAccMult, 0.15f, 0.4f) / 2.0f;
                Utilities.BlendIntoAccumulator(smoothRate, newAcceleration, ref acceleration);
            }

            if (applyGravity)
            {
                acceleration += gravity;
            }
            velocity += acceleration * timeAccMult;

            if (integrateForces)
            {
                desiredPosition = desiredPosition + (velocity * timeAccMult);
            }

            // the length of this global-upward-pointing vector controls the vehicle's
            // tendency to right itself as it is rolled over from turning acceleration
            Vector3 globalUp = new Vector3(0, straighteningTendancy, 0);
            // acceleration points toward the center of local path curvature, the
            // length determines how much the vehicle will roll while turning
            Vector3 accelUp = acceleration * 0.05f;
            // combined banking, sum of UP due to turning and global UP
            Vector3 bankUp = accelUp + globalUp;
            // blend bankUp into vehicle's UP basis vector
            smoothRate = timeAccMult;// * 3.0f;
            Vector3 tempUp = transform.up;
            Utilities.BlendIntoAccumulator(smoothRate, bankUp, ref tempUp);

            float speed = velocity.magnitude;
            if (speed > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
            Utilities.checkNaN(velocity);

            if (speed > 0.01f && integrateForces)
            {
                transform.forward = Vector3.RotateTowards(transform.forward, velocity, Mathf.Deg2Rad * maxTurnDegrees * Time.deltaTime, float.MaxValue);
                //transform.forward = velocity;
            }

            if (applyBanking && integrateForces)
            {
                transform.LookAt(transform.position + transform.forward, tempUp);
            }
            velocity *= (1.0f - (damping * timeAccMult));
            timeAcc = 0.0f;
            UpdateLocalFromTransform();
        }

        if (preferredTimeDelta != 0.0f)
        {
            float timeDelta = Time.deltaTime * timeMultiplier;
            timeDelta *= (flock == null) ? 1 : flock.timeMultiplier;
            float dist = Vector3.Distance(transform.position, desiredPosition);
            float distThisFrame = dist * (timeDelta / preferredTimeDelta);
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, 50 * Time.deltaTime);
        }
        else
        {
            transform.position = desiredPosition;
        }            
    }

    private bool AccumulateForce(ref Vector3 runningTotal, Vector3 force)
    {
        float soFar = runningTotal.magnitude;

        float remaining = maxForce - soFar;
        if (remaining <= 0)
        {
            return false;
        }

        float toAdd = force.magnitude;


        if (toAdd < remaining)
        {
            runningTotal += force;
        }
        else
        {
            runningTotal += Vector3.Normalize(force) * remaining;
        }
        return true;
    }

    public Vector3 CalculateForce()
    {
        Vector3 totalForce = Vector3.zero;

        foreach (SteeringBehaviour behaviour in behaviours)
        {
            Vector3 force = behaviour.Calculate() * behaviour.weight;
            force *= weight;
            if (!AccumulateForce(ref totalForce, force))
            {
                break;
            }            
        }
        return totalForce;
    }
    #endregion

    // Shared behaviours
    public Vector3 SeekForce(Vector3 targetPos)
    {
        Vector3 desiredVelocity;

        desiredVelocity = targetPos - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        return (desiredVelocity - velocity);
    }

    public Vector3 FleeForce(Vector3 target)
    {
        Vector3 desiredVelocity;
        desiredVelocity = position - target;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Utilities.checkNaN(desiredVelocity);
        return (desiredVelocity - velocity);
    }

    public Vector3 ArriveForce(Vector3 target, float slowingDistance = 15.0f, float deceleration = 0.9f)
    {
        Vector3 desired = target - position;

        float distance = desired.magnitude;

        if (distance < 1.0f)
        {
            return Vector3.zero;
        }
        float desiredSpeed = 0;
        if (distance < slowingDistance)
        {
            desiredSpeed = (distance / slowingDistance) * maxSpeed * (1.0f - slowingDistance);
        }
        else
        {
            desiredSpeed = maxSpeed;
        }
        desired *= desiredSpeed;

        return desired - velocity;
    }
}
