using UnityEngine;
using System.Collections;

namespace BGE.Forms
{
    public class ViveController : MonoBehaviour {

        public SteamVR_TrackedObject leftTrackedObject;
        public SteamVR_TrackedObject rightTrackedObject;
        private Rigidbody rigidBody;

        public GameObject leftEngine;
        public GameObject rightEngine;

        public GameObject head;
        public float maxSpeed = 250.0f;
        public float power = 1000.0f;

        public Boid boid; // Am I controlling a boid?

        private SteamVR_Controller.Device leftController
        {
            get
            {
                return SteamVR_Controller.Input((int)leftTrackedObject.index);
            }
        }

        private SteamVR_Controller.Device rightController
        {
            get
            {
                return SteamVR_Controller.Input((int)rightTrackedObject.index);
            }
        }

        // Use this for initialization
        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            desiredYaw = transform.rotation;
        }

        void DetatchFromBoid()
        {
            if (boid != null)
            {
                boid.GetComponent<Harmonic>().active = false;
                boid.GetComponent<Harmonic>().enabled = false;
                boid.GetComponent<PlayerSteering>().enabled = false;
                boid.damping = 0.5f;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.parent = null;
                boid = null;
            }
        }


        Quaternion desiredYaw;

        // Update is called once per frame
        void FixedUpdate()
        {
            float leftTrig = 0.0f;
            float rightTrig = 0.0f;

            if (leftTrackedObject != null && leftTrackedObject.isActiveAndEnabled)
            {
                // The trigger button
                leftTrig = leftController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1).x;            
            }
        
            if (rightTrackedObject != null && rightTrackedObject.isActiveAndEnabled)
            {
                // The trigger button
                rightTrig = rightController.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis1).x;
            }
        
            if (leftTrig > 0.2f)
            {
                DetatchFromBoid();
                rigidBody.AddForceAtPosition(leftTrackedObject.transform.forward * power * leftTrig, leftTrackedObject.transform.position);
            }
            else
            {

            }
            leftEngine.GetComponent<JetFire>().fire = leftTrig;        
            rightEngine.GetComponent<JetFire>().fire = rightTrig;
            if (rightTrig > 0.2f)
            {
                DetatchFromBoid();
                rigidBody.AddForceAtPosition(rightTrackedObject.transform.forward * power * rightTrig, rightTrackedObject.transform.position);
            }
            else
            {
            }

            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxSpeed);

        
            //rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, 10f);

            /*
            if (leftTrig > 0.2f && rightTrig > 0.2f)
            {
                rigidBody.AddForce(head.transform.forward * power * 10);
            }
            else if (leftTrig > 0.2f)
            {
                desiredYaw *= Quaternion.AngleAxis(leftTrig * 0.5f, Vector3.up);
            }
            else if (rightTrig > 0.2f)
            {
                desiredYaw *= Quaternion.AngleAxis(rightTrig * 0.5f, -Vector3.up);
            }

            float currentYaw = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredYaw, Time.deltaTime);
            */
        }
    }
}