using UnityEngine;
using System.Collections;
using System;

public class PlayerSteering : SteeringBehaviour {
    public float power = 500.0f;

    public float upForce;
    public float rightForce;

    private ViveController viveController;
    private Vector3 viveForce
    public void Start()
    {
        viveController = FindObjectOfType<ViveController>();
    }

    public override void Update()
    {
        base.Update();
        upForce = - Input.GetAxis("Vertical");
        rightForce = Input.GetAxis("Horizontal");

        // Control the boid
        if (viveController != null)
        {
            if (viveController.leftTrackedObject != null && viveController.rightTrackedObject != null)
            {
                Quaternion average = Quaternion.Slerp(viveController.leftTrackedObject.transform.localRotation
    , viveController.rightTrackedObject.transform.localRotation, 0.5f);

                /*        
                float viveRightForce = viveController.leftTrackedObject.transform.position.y - viveController.rightTrackedObject.transform.position.y;
                rightForce += (viveRightForce * 0.5f);

                Quaternion average = Quaternion.Slerp(viveController.leftTrackedObject.transform.localRotation
                    , viveController.rightTrackedObject.transform.localRotation, 0.5f);
                Vector3 ypr = average.eulerAngles;
                float pitch = ypr.x;
                if (pitch > 180.0f)
                {
                    pitch = pitch - 360.0f;
                }
                pitch = Utilities.Map(pitch, -90, 90, 1, -1);
                CreatureManager.Log("Pitch: " + pitch);
                upForce += (pitch * 0.5f);
                */

            }
        }

        //Debug.Log("Cont: " + contWalk);
    }

    public override Vector3 Calculate()
    {
        Vector3 force = boid.right * rightForce * power
            + boid.up * (upForce) * power;
        return force;
    }
}
