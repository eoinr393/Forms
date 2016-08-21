/*

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using BGE.Geom;

// Hello world

    

namespace BGE
{

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
    
        // Need these because we are running on a thread and can't touch the transform
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
        public float forceMultiplier = 1.0f;
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

        [Header("Seek")]
        public bool seekEnabled = false;
        public Vector3 seekTargetPos = Vector3.zero;
        public float seekWeight = 1.0f;
        public bool seekPlayer = false;

        [Header("Flee")]
        public bool fleeEnabled;
        public float fleeWeight = 1.0f;
        public GameObject fleeTarget = null;
        public float fleeRange = 100.0f;
        private Vector3 fleeTargetPosition = Vector3.zero;
        [HideInInspector]
        private Vector3 fleeForce;
        private Vector2 fleeGizmoPos;

        [Header("Arrive")]
        public bool arriveEnabled = false;
        public Vector3 arriveTargetPos = Vector3.zero;
        public float arriveWeight = 1.0f;
        public float arriveSlowingDistance = 15.0f;
        [Range(0.0f, 1.0f)]
        public float arriveDeceleration = 0.9f;

        public enum WanderMethod { Jitter, Noise };
        [Header("Wander")]
        public bool wanderEnabled;
        public WanderMethod wanderMethod;
        public float wanderRadius = 10.0f;
        public float wanderJitter = 20.0f;
        public float wanderDistance = 15.0f;
        public float wanderWeight = 1.0f;
        public float wanderNoiseDeltaX = 0.5f;
        private float wanderNoiseX;
        private float wanderNoiseY;


        [Header("Separation")]
        public bool separationEnabled = false;
        public float separationWeight = 1.0f;

        [Header("Alignment")]
        public bool alignmentEnabled = false;
        public float alignmentWeight = 1.0f;

        [Header("Cohesion")]
        public bool cohesionEnabled = false;
        public float cohesionWeight = 1.0f;

        [Header("Obstacle Avoidance")]
        public bool obstacleAvoidanceEnabled = false;
        public float minBoxLength = 50.0f;
        public float obstacleAvoidanceWeight = 1.0f;

        [Header("Plane Avoidance")]
        public bool planeAvoidanceEnabled = false;
        public float planeAvoidanceWeight = 1.0f;
        public float planeY = 0;

        [Header("Follow Path")]
        public bool followPathEnabled = false;
        public float followPathWeight = 1.0f;
        public bool ignoreHeight = false;
        public Path path;

        [Header("Pursuit")]
        public bool pursuitEnabled = false;
        public GameObject pursuitTarget = null;
        public float pursuitWeight = 1.0f;
        private Vector3 pursuitTargetPos;

        [Header("Evade")]
        public bool evadeEnabled = false;
        public GameObject evadeTarget = null;
        public float evadeWeight = 1.0f;


        [Header("Offset Pursuit")]
        public bool offsetPursuitEnabled = false;
        public GameObject offsetPursuitTarget = null;
        public float offsetPursuitWeight = 1.0f;
        [Range(0.0f, 1.0f)]
        public float pitchForceScale = 1.0f;
        [HideInInspector]
        public Vector3 offset;
        private Vector3 offsetPursuitTargetPos = Vector3.zero;

        [Header("Sphere Constrain")]
        public bool sphereConstrainEnabled = false;
        public bool centreOnPosition = true;
        public Vector3 sphereCentre = Vector3.zero;
        public float sphereRadius = 1000.0f;
        public float sphereConstrainWeight = 1.0f;

        [Header("Random Walk")]
        public bool randomWalkEnabled = false;
        [HideInInspector]
        public Vector3 randomWalkCenter = Vector3.zero;
        public float randomWalkWaitMaxSeconds = 5.0f;
        private float randomWalkWait = 0.0f;
        public float randomWalkRadius = 1000.0f;
        public bool randomWalkKeepY = true;
        public float randomWalkWeight = 1.0f;
        private Vector3 randomWalkForce = Vector3.zero;

        [Header("Scene Avoidance")]
        public bool sceneAvoidanceEnabled = false;
        public float sceneAvoidanceWeight = 1.0f;
        public float sceneAvoidanceScalar = 4.0f;
        public float sceneAvoidanceForwardFeelerDepth = 30;
        public float sceneAvoidanceSideFeelerDepth = 15;
        SceneAvoidanceFeelerInfo[] sceneAvoidanceFeelers = new SceneAvoidanceFeelerInfo[5];
        public float sceneAvoidanceFrontFeelerDither = 0.5f;
        public float sceneAvoidanceSideFeelerDither = 0.05f;
        public float sceneAvoidanceFeelerRadius = 2.0f;
        public enum SceneAvoidanceForceType { normal, incident, braking };
        public SceneAvoidanceForceType sceneAvoidanceForceType = SceneAvoidanceForceType.normal;

        [Header("Wiggle")]
        public float wiggleSpeed = 30;
        [HideInInspector]
        public float rampedWiggleSpeed = 0;
        [HideInInspector]
        public float rampedWiggleAmplitude = 0;
        public float wiggleAmplitude = 50;
        public bool wiggleEnabled = false;
        public float wiggleWeigth = 1.0f;
        public WiggleAxis wiggleDirection = WiggleAxis.Horizontal;
        public enum WiggleAxis { Horizontal, Vertical };
        private Vector3 wiggleWorldTarget = Vector3.zero;
        public float wiggleTheta;

        [HideInInspector]
        public Vector3 force = Vector3.zero;

        [HideInInspector]
        public Vector3 velocity = Vector3.zero;

        [HideInInspector]
        public Vector3 acceleration;

        List<Boid> tagged = new List<Boid>();
        List<Vector3> PlaneAvoidanceFeelers = new List<Vector3>();

        private Vector3 wanderTargetPos;
        private Vector3 randomWalkTarget;

        
        [Header("Gravity")]
        public bool applyGravity = false;
        public Vector3 gravity = new Vector3(0, -9, 0);

        Collider myCollider;

        public void OnDrawGizmos()
        {
            if (seekEnabled)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, seekTargetPos);
            }
            if (arriveEnabled)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(transform.position, arriveTargetPos);
            }
            if (pursuitEnabled)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, pursuitTargetPos);
            }
            if (offsetPursuitEnabled)
            {
                if (Application.isPlaying)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(transform.position, offsetPursuitTargetPos);
                }
                
            }

            if (wanderEnabled)
            {
                Gizmos.color = Color.blue;
                Vector3 wanderCircleCenter = transform.TransformPoint(Vector3.forward * wanderDistance);
                Gizmos.DrawWireSphere(wanderCircleCenter, wanderRadius);
                Gizmos.color = Color.green;
                Vector3 worldTarget = transform.TransformPoint(wanderTargetPos + Vector3.forward * wanderDistance);
                Gizmos.DrawLine(transform.position, worldTarget);
            }

            if (wiggleEnabled)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(position, wiggleWorldTarget);
                LineDrawer.DrawTarget(wiggleWorldTarget, Color.red);
                Vector3 worldCenter = transform.TransformPoint(Vector3.forward * wanderDistance);
                Gizmos.color = Color.gray;
                Gizmos.DrawWireSphere(worldCenter, wanderRadius);
            }

            if (sceneAvoidanceEnabled)
            {
                foreach (SceneAvoidanceFeelerInfo feeler in sceneAvoidanceFeelers)
                {
                    if (feeler.collided)
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawLine(transform.position, feeler.point);
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawLine(feeler.point, feeler.point + (feeler.normal * 5));
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(feeler.point, feeler.point + (force / 100.0f));
                    }
                }
            }

            if (fleeEnabled)
            {
                if (fleeForce.sqrMagnitude > 0.0f)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, fleeGizmoPos);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, transform.position + fleeForce);
                }
            }

            if (sphereConstrainEnabled)
            {
                if (!Application.isPlaying)
                {
                    sphereCentre = transform.position;
                }
                Gizmos.color = Color.green;
                if (centreOnPosition)
                {
                    
                    Gizmos.DrawWireSphere(sphereCentre, sphereRadius);
                }
                else
                {
                    Gizmos.DrawWireSphere((flock != null) ? flock.flockCenter : sphereCentre, sphereRadius);
                }
            }
        }


        [HideInInspector]
        public bool multiThreadingEnabled = false;

        private float ThreadTimeDelta()
        {
            float flockMultiplier = (flock == null) ? 1 : flock.timeMultiplier;
            float timeDelta = multiThreadingEnabled ? flock.threadTimeDelta : Time.deltaTime;
            return timeDelta * flockMultiplier * timeMultiplier;
        }
        
        public Boid()
        {
            TurnOffAll();            
        }

        void Start()
        {
            //Application.targetFrameRate = 20;
            randomWalkCenter = position;
            randomWalkTarget = position;
            //if (randomWalkKeepY)
            //{
            //    randomWalkTarget.y = position.y;
            //}
            if (offsetPursuitTarget != null)
            {
                offset = transform.position - offsetPursuitTarget.transform.position;
                offset = Quaternion.Inverse(transform.rotation) * offset;
            }

            wanderNoiseX = Utilities.RandomRange(0, 10000);
            wanderNoiseY = Utilities.RandomRange(0, 10000);

            randomWalkWait = Utilities.RandomRange(0, randomWalkWaitMaxSeconds);

            if (centreOnPosition)
            {
                sphereCentre = transform.position;
            }

            myCollider = GetComponentInChildren<Collider>();

            if (path == null)
            {
                path = GetComponent<Path>();
            }

            wiggleTheta = UnityEngine.Random.Range(0, Mathf.PI * 2.0f);

            desiredPosition = transform.position;
            timeAcc = preferredTimeDelta;
        }

        #region Flags

        public void TurnOffAll()
        {
            seekEnabled = false;
            fleeEnabled = false;
            arriveEnabled = false;
            wanderEnabled = false;
            cohesionEnabled = false;
            separationEnabled = false;
            alignmentEnabled = false;
            obstacleAvoidanceEnabled = false;
            planeAvoidanceEnabled = false;
            followPathEnabled = false;
            pursuitEnabled = false;
            evadeEnabled = false;
            offsetPursuitEnabled = false;
            sphereConstrainEnabled = false;
            randomWalkEnabled = false;
            wiggleEnabled = false;
        }


        #endregion

        #region Utilities        
        
        // These feelers are normals       

        #endregion
        #region Integration

        private bool accumulateForce(ref Vector3 runningTotal, Vector3 force)
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



        private void EnforceNonPenetrationConstraint()
        {
            foreach (Boid boid in tagged)
            {
                if (boid == this)
                {
                    continue;
                }
                Vector3 toOther = boid.position - position;
                float distance = toOther.magnitude;
                float overlap = radius + boid.radius - distance;
                if (overlap >= 0)
                {
                    boid.position = (boid.position + (toOther / distance) *
                     overlap);
                }
            }
        }

        Vector3 oldSeparation, oldAlignment, oldCohesion;

        public void UpdateLocalFromTransform()
        {
            position = transform.position;
            up = transform.up;
            right = transform.right;
            forward = transform.forward;
            rotation = transform.rotation;

            fleeTargetPosition = (fleeTarget == null) ? Vector3.zero : fleeTarget.transform.position;
        }

        private Vector3 Calculate()
        {
            Vector3 force = Vector3.zero;
            Vector3 steeringForce = Vector3.zero;
            
            if (obstacleAvoidanceEnabled)
            {
                force = ObstacleAvoidance() * obstacleAvoidanceWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (sceneAvoidanceEnabled)
            {
                force = SceneAvoidance() * sceneAvoidanceWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            Utilities.checkNaN(force);
            if (planeAvoidanceEnabled)
            {
                force = PlaneAvoidance() * planeAvoidanceWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }
            
            if (sphereConstrainEnabled)
            {
                force = SphereConstrain(sphereRadius) * sphereConstrainWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            // ALways do wiggle        
            if (wiggleEnabled)
            {
                force = Wiggle() * wiggleWeigth;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }



            if (evadeEnabled)
            {
                force = Evade() * evadeWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (fleeEnabled)
            {
                if (fleeTargetPosition != Vector3.zero)
                {
                    force = Flee(fleeTargetPosition, fleeRange) * fleeWeight;
                    force *= forceMultiplier;
                    fleeForce = force;
                }

                if (flock != null)
                {
                    force = Vector3.zero;
                    foreach (Vector3 enemyPosition in flock.enemyPositions)
                    {
                        force += Flee(enemyPosition, fleeRange) * fleeWeight;
                        force *= forceMultiplier;
                        
                    }
                    fleeForce = force;
                }
                
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (separationEnabled || cohesionEnabled || alignmentEnabled)
            {
                if (flock != null)
                {
                    TagNeighboursSimple(flock.neighbourDistance);
                    
                }
            }

            if (separationEnabled && (tagged.Count > 0))
            {
                force = Separation() * separationWeight;
                force *= forceMultiplier;

                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }

            }

            if (alignmentEnabled && (tagged.Count > 0))
            {
                force = Alignment() * alignmentWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (cohesionEnabled && (tagged.Count > 0))
            {
                force = Cohesion() * cohesionWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (seekEnabled)
            {
                force = Seek(seekTargetPos) * seekWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }


            if (arriveEnabled)
            {
                force = Arrive(arriveTargetPos) * arriveWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (wanderEnabled)
            {
                if (wanderMethod == WanderMethod.Jitter)
                {
                    force = Wander() * wanderWeight;
                }
                else
                {
                    force = NoiseWander() * wanderWeight;
                }
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }


            if (pursuitEnabled)
            {
                force = Pursue() * pursuitWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (offsetPursuitEnabled)
            {
                force = OffsetPursuit(offset) * offsetPursuitWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }
            
            if (followPathEnabled)
            {
                force = FollowPath() * followPathWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }

            if (randomWalkEnabled)
            {
                force = RandomWalk() * randomWalkWeight;
                force *= forceMultiplier;
                if (!accumulateForce(ref steeringForce, force))
                {
                    return steeringForce;
                }
            }                
            return steeringForce;
         
        }

        Vector3 Wiggle()
        {

            float n = Mathf.Sin(wiggleTheta);

            rampedWiggleAmplitude = Mathf.Lerp(rampedWiggleAmplitude, wiggleAmplitude, Time.deltaTime * 2.0f);

            float t = Utilities.Map(n, -1.0f, 1.0f, -rampedWiggleAmplitude, rampedWiggleAmplitude);
            float theta = Mathf.Sin(Utilities.DegreesToRads(t));

            if (wiggleDirection == WiggleAxis.Horizontal)
            {
                wanderTargetPos.x = Mathf.Sin(theta);
                wanderTargetPos.z = Mathf.Cos(theta);
                wanderTargetPos.y = 0;
            }
            else
            {
                wanderTargetPos.y = Mathf.Sin(theta);
                wanderTargetPos.z = Mathf.Cos(theta);
                wanderTargetPos.x = 0;
            }

            wanderTargetPos *= wanderRadius;
            Vector3 yawRoll = transform.rotation.eulerAngles;
            yawRoll.x = 0;
            Vector3 localTarget = wanderTargetPos + (Vector3.forward * wanderDistance);
            wiggleWorldTarget = TransformPoint(localTarget);

            Vector3 worldTargetOnY = transform.position + Quaternion.Euler(yawRoll) * localTarget;
            rampedWiggleSpeed = Mathf.Lerp(rampedWiggleSpeed, wiggleSpeed, Time.deltaTime * 2.0f);
            wiggleTheta += ThreadTimeDelta() * rampedWiggleSpeed * Mathf.Deg2Rad;
            //wiggleTheta += ThreadTimeDelta() * wiggleAngularSpeedDegrees * Mathf.Deg2Rad;
            if (wiggleTheta > Utilities.TWO_PI)
            {
                wiggleTheta = Utilities.TWO_PI - wiggleTheta;
            }

            return Seek(worldTargetOnY);
        }

        Vector3 CalculateSceneAvoidanceForce(SceneAvoidanceFeelerInfo info)
        {
            Vector3 force = Vector3.zero;

            Vector3 fromTarget = fromTarget = position - info.point;
            float dist = Vector3.Distance(position, info.point);

            switch (sceneAvoidanceForceType)
            {
                case SceneAvoidanceForceType.normal:
                    force = info.normal * (sceneAvoidanceForwardFeelerDepth * sceneAvoidanceScalar / dist);
                    break;
                case SceneAvoidanceForceType.incident:                    
                    fromTarget.Normalize();
                    force -= Vector3.Reflect(fromTarget, info.normal) * (sceneAvoidanceForwardFeelerDepth / dist);
                    break;
                case SceneAvoidanceForceType.braking:
                    force += fromTarget * (sceneAvoidanceForwardFeelerDepth / dist);
                    break;
            }
            return force;
        }        

        Vector3 TransformDirection(Vector3 direction)
        {
            return rotation * direction;
        }

        void UpdateSceneAvoidanceFrontFeeler()
        {
            RaycastHit info;
            float forwardFeelerDepth = sceneAvoidanceForwardFeelerDepth + ((velocity.magnitude / maxSpeed) * sceneAvoidanceForwardFeelerDepth);

            // Forward feeler
            //int layerMask = 1 << 9;
            bool collided = Physics.SphereCast(transform.position, sceneAvoidanceFeelerRadius, TransformDirection(Vector3.forward), out info, forwardFeelerDepth);
            sceneAvoidanceFeelers[0] = new SceneAvoidanceFeelerInfo(info.point, info.normal
                , collided && info.collider != myCollider, SceneAvoidanceFeelerInfo.FeeelerType.front);
        }
        

        void UpdateSceneAvoidanceSideFeelers()
        {
            Vector3 feelerDirection;
            RaycastHit info;
            bool collided;

            float sideFeelerDepth = sceneAvoidanceSideFeelerDepth + ((velocity.magnitude / maxSpeed) * sceneAvoidanceSideFeelerDepth);

            
            // Left feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.up) * feelerDirection;
            collided = Physics.SphereCast(transform.position, sceneAvoidanceFeelerRadius, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            sceneAvoidanceFeelers[1] = new SceneAvoidanceFeelerInfo(info.point, info.normal, 
                collided && info.collider != myCollider, SceneAvoidanceFeelerInfo.FeeelerType.side);

            // Right feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.up) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            sceneAvoidanceFeelers[2] = new SceneAvoidanceFeelerInfo(info.point, info.normal
                , collided && info.collider != myCollider, SceneAvoidanceFeelerInfo.FeeelerType.side);

            // Up feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(45, Vector3.right) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            sceneAvoidanceFeelers[3] = new SceneAvoidanceFeelerInfo(info.point, info.normal
                , collided && info.collider != myCollider, SceneAvoidanceFeelerInfo.FeeelerType.side);

            // Down feeler
            feelerDirection = Vector3.forward;
            feelerDirection = Quaternion.AngleAxis(-45, Vector3.right) * feelerDirection;
            collided = Physics.SphereCast(transform.position, 2, transform.TransformDirection(feelerDirection), out info, sideFeelerDepth);
            sceneAvoidanceFeelers[4] = new SceneAvoidanceFeelerInfo(info.point, info.normal
                , collided && info.collider != myCollider, SceneAvoidanceFeelerInfo.FeeelerType.side);
        }

        Vector3 SceneAvoidance()
        {
            Vector3 force = Vector3.zero;

            for (int i = 0; i < sceneAvoidanceFeelers.Length; i++)
            {                                                
                SceneAvoidanceFeelerInfo info = sceneAvoidanceFeelers[i];
                if (info.collided)
                {
                    force += CalculateSceneAvoidanceForce(info);
                }
            }            

            return force;
        }

        float timeAcc = 0;
        Vector3 desiredPosition = Vector3.zero;
        void Update()
        {
            float smoothRate;

            if (!multiThreadingEnabled)
            {
                UpdateLocalFromTransform();
                CalculateForces();
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

            // Update the front feeler each frame
            if (sceneAvoidanceEnabled && (UnityEngine.Random.Range(0.0f, 1.0f) < sceneAvoidanceFrontFeelerDither))
            {
                UpdateSceneAvoidanceFrontFeeler();
            }

            if (sceneAvoidanceEnabled && (UnityEngine.Random.Range(0.0f, 1.0f) < sceneAvoidanceSideFeelerDither))
            {
                //Update the side feelers
                UpdateSceneAvoidanceSideFeelers();
            }
        }
        
        public void CalculateForces()
        {
            force = Calculate();
            
            //if (calculateThisFrame && enforceNonPenetrationConstraint)
            //{
            //    EnforceNonPenetrationConstraint();
            //}            
        }

        #endregion

        #region Behaviours

        Vector3 Seek(Vector3 targetPos)
        {
            Vector3 desiredVelocity;

            desiredVelocity = targetPos - position;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;            
            return (desiredVelocity - velocity);
        }

    Vector3 Evade()
    {
        float dist = (evadeTarget.GetComponent<Boid>().position - position).magnitude;
        float lookAhead = maxSpeed;

        Vector3 targetPos = evadeTarget.GetComponent<Boid>().position + (lookAhead * evadeTarget.GetComponent<Boid>().velocity);
        return Flee(targetPos);
    }

        

        Vector3 OffsetPursuit(Vector3 offset)
        {
            Vector3 target = Vector3.zero;

            target = offsetPursuitTarget.GetComponent<Boid>().TransformPoint(offset);


            float dist = (target - position).magnitude;

            float lookAhead = (dist / maxSpeed);

            target = target + (lookAhead * offsetPursuitTarget.GetComponent<Boid>().velocity);

            float pitchForce = target.y - position.y;
            pitchForce *= (1.0f - pitchForceScale);
            target.y -= pitchForce;

            offsetPursuitTargetPos = target;

            Utilities.checkNaN(target);
            return Seek(target);
        }

        Vector3 Pursue()
        {
            Vector3 toTarget = pursuitTarget.GetComponent<Boid>().position - position;
            float dist = toTarget.magnitude;
            float time = dist / maxSpeed;

            pursuitTargetPos = pursuitTarget.GetComponent<Boid>().position + (time * pursuitTarget.GetComponent<Boid>().velocity);
            
            return Seek(pursuitTargetPos);
        }

        Vector3 Flee(Vector3 targetPos, float fleeRange)
        {
            Vector3 desiredVelocity;
            desiredVelocity = position - targetPos;
            if (desiredVelocity.magnitude > fleeRange)
            {
                return Vector3.zero;
            }
            fleeGizmoPos = targetPos;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            Utilities.checkNaN(desiredVelocity);
            return (desiredVelocity - velocity);
        }

        Vector3 Flee(Vector3 targetPos)
        {
            Vector3 desiredVelocity;
            desiredVelocity = position - targetPos;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            Utilities.checkNaN(desiredVelocity);
            return (desiredVelocity - velocity);
        }

        Vector3 RandomWalk()
        {
            float dist = (position - randomWalkTarget).magnitude;
            if (dist < 30)
            {
                StartCoroutine("RandomWalkWait");
                float sphereRadius = (flock != null) ? flock.radius : randomWalkRadius;
                Vector3 r = Utilities.RandomInsideUnitSphere();
                r.y = Mathf.Abs(r.y);
                randomWalkTarget = randomWalkTarget = randomWalkCenter + r * randomWalkRadius;
            }
            if (randomWalkKeepY)
            {
                randomWalkTarget.y = position.y;
            }
            return Seek(randomWalkTarget);
        }

        IEnumerator RandomWalkWait()
        {
            randomWalkEnabled = false;
            yield return new WaitForSeconds(randomWalkWait);
            randomWalkEnabled = true;
            randomWalkWait = Utilities.RandomRange(0, randomWalkWaitMaxSeconds);
        }

        Vector3 TransformPoint(Vector3 localPoint)
        {
            // return TransformPoint(localPoint);
            Vector3 p = rotation * localPoint;
            p += position;
            return p;
        }

        Vector3 Wander()
        {
            float jitterTimeSlice = wanderJitter * ThreadTimeDelta();

            Vector3 toAdd = Utilities.RandomInsideUnitSphere() * jitterTimeSlice;
            wanderTargetPos += toAdd;
            wanderTargetPos.Normalize();
            wanderTargetPos *= wanderRadius;

            Vector3 localTarget = wanderTargetPos + Vector3.forward * wanderDistance;
            Vector3 worldTarget = TransformPoint(localTarget);
            return (worldTarget - position);
        }

        Vector3 NoiseWander()
        {
            float n = Mathf.PerlinNoise(wanderNoiseX, 0);
            float theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);
            wanderTargetPos.x = Mathf.Sin(theta);
            wanderTargetPos.z = -Mathf.Cos(theta);

            n = Mathf.PerlinNoise(wanderNoiseX, 0);
            theta = Utilities.Map(n, 0.0f, 1.0f, 0, Mathf.PI * 2.0f);

            wanderTargetPos.y = 0;
            wanderTargetPos *= wanderRadius;
            Vector3 localTarget = wanderTargetPos + (Vector3.forward * wanderDistance);
            Vector3 worldTarget = TransformPoint(localTarget);

            wanderNoiseX += wanderNoiseDeltaX * ThreadTimeDelta();
            Vector3 desired = worldTarget - position;
            desired.Normalize();
            desired *= maxSpeed;
            //return Vector3.zero;
            return desired - velocity;
        }

        public Vector3 PlaneAvoidance()
        {
            makeFeelers();

            Plane worldPlane = new Plane(new Vector3(0, 1, 0), -planeY);
            Vector3 force = Vector3.zero;
            foreach (Vector3 feeler in PlaneAvoidanceFeelers)
            {
                if (!worldPlane.GetSide(feeler))
                {
                    float distance = Math.Abs(worldPlane.GetDistanceToPoint(feeler));
                    force += worldPlane.normal * distance;
                }
            }

            if (force.magnitude > 0.0)
            {
                DrawFeelers();
            }
            return force;
        }

        public void DrawFeelers()
        {
            foreach (Vector3 feeler in PlaneAvoidanceFeelers)
            {
                LineDrawer.DrawLine(position, feeler, Color.green);
            }
        }

        public Vector3 Arrive(Vector3 target)
        {
            Vector3 desired = target - position;

            float distance = desired.magnitude;
            //toTarget.Normalize();
            
            if (distance < 1.0f)
            {
                return Vector3.zero;
            }
            float desiredSpeed = 0;
            if (distance < arriveSlowingDistance)
            {
                desiredSpeed = (distance / arriveSlowingDistance) * maxSpeed * (1.0f - arriveDeceleration);
            }
            else
            {
                desiredSpeed = maxSpeed;
            }
            desired *= desiredSpeed;

            return desired - velocity;
        }

        private Vector3 FollowPath()
        {
            float epsilon = 20.0f;
            float dist;
            Vector3 nextWayPoint = path.NextWaypoint();

            if (ignoreHeight)
            {
                nextWayPoint.y = position.y;
            }

            dist = (position - path.NextWaypoint()).magnitude;

            if (dist < epsilon)
            {
                path.AdvanceToNext();
            }
            if ((!path.looped) && path.IsLast())
            {
                return Arrive(path.NextWaypoint());
            }
            else
            {
                return Seek(path.NextWaypoint());
            }
        }

        public Vector3 SphereConstrain(float radius)
        {
            Vector3 toTarget = position -
                ((flock != null) ? flock.flockCenter : sphereCentre);
            float sphereRadius = (flock != null) ? flock.radius : radius;
            Vector3 steeringForce = Vector3.zero;
            if (toTarget.magnitude > sphereRadius)
            {
                steeringForce = Vector3.Normalize(toTarget) * (sphereRadius - toTarget.magnitude);
            }
            return steeringForce;
        }

        #endregion

        #region Flocking
        private int TagNeighboursSimple(float inRange)
        {
            tagged.Clear();

            float inRangeSq = inRange * inRange;
            foreach (Boid boid in flock.boids)
            {                
                if (boid != this)
                {
                    if ((position - boid.position).sqrMagnitude < inRangeSq)
                    {
                        tagged.Add(boid);
                    }
                }
            }
            return tagged.Count;
        }
        
        public Vector3 Separation()
        {
            Vector3 steeringForce = Vector3.zero;
            foreach (Boid boid in tagged)
            {
                if (boid != this)
                {
                    Vector3 toEntity = position - boid.position;
                    steeringForce += (Vector3.Normalize(toEntity) / toEntity.magnitude);
                }
            }

            return steeringForce;
        }

        public Vector3 Cohesion()
        {
            Vector3 steeringForce = Vector3.zero;
            Vector3 centreOfMass = Vector3.zero;
            int taggedCount = 0;
            foreach (Boid boid in tagged)
            {
                if (boid != this)
                {
                    centreOfMass += boid.position;
                    taggedCount++;
                }
            }
            if (taggedCount > 0)
            {
                centreOfMass /= (float)taggedCount;

                if (centreOfMass.sqrMagnitude == 0)
                {
                    steeringForce = Vector3.zero;
                }
                else
                {
                    steeringForce = Vector3.Normalize(Seek(centreOfMass));
                }
            }
            Utilities.checkNaN(steeringForce);
            return steeringForce;
        }

        public Vector3 Alignment()
        {
            Vector3 steeringForce = Vector3.zero;
            int taggedCount = 0;
            foreach (Boid boid in tagged)
            {
                if (boid != this)
                {
                    steeringForce += boid.forward;
                    taggedCount++;
                }
            }

            if (taggedCount > 0)
            {
                steeringForce /= (float)taggedCount;
                steeringForce -= forward;
            }
            return steeringForce;

        }
        #endregion Flocking        
    }
}

*/