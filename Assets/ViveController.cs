using UnityEngine;
using System.Collections;

public class ViveController : MonoBehaviour {

    public SteamVR_TrackedObject leftTrackedObject;
    public SteamVR_TrackedObject rightTrackedObject;
    private Rigidbody rigidBody;

    public GameObject leftEngine;
    public GameObject rightEngine;

    public GameObject head;

    public float power = 1000.0f;

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
            leftEngine.SetActive(true);
            rigidBody.AddForceAtPosition(leftTrackedObject.transform.forward * power * leftTrig, leftTrackedObject.transform.position);
        }
        else
        {
            leftEngine.SetActive(false);
        }

        if (rightTrig > 0.2f)
        {
            rightEngine.SetActive(true);
            rigidBody.AddForceAtPosition(rightTrackedObject.transform.forward * power * rightTrig, rightTrackedObject.transform.position);
        }
        else
        {
            rightEngine.SetActive(false);
        }

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
