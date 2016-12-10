using UnityEngine;
using System.Collections;
using System;

public class HumanController : SteeringBehaviour {
    public float power = 500.0f;

    public float contWalk;

    public float contStrafe;
    
    public void Start()
    {
    }

    public override void Update()
    {
        base.Update();
        contWalk = Input.GetAxis("Vertical");
        contStrafe = Input.GetAxis("Horizontal");
        //Debug.Log("Cont: " + contWalk);
    }

    public override Vector3 Calculate()
    {
        Vector3 force = boid.right * contStrafe * power
            + boid.up * (- contWalk) * power;
        return force;
    }
}
