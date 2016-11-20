using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
    
public class Hover:Harmonic
{
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(boid.transform.position, boid.transform.position + (force));
        CreatureManager.Log("Theta" + theta);
        CreatureManager.Log("Force" + force);
    }

    public override Vector3 Calculate()
    {
        Vector3 force = Vector3.zero;
        theta = theta % (Utilities.TWO_PI);
        rampedAmplitude = Mathf.Lerp(rampedAmplitude, amplitude, boid.TimeDelta);


        if (theta < Mathf.PI)
        {
            force = boid.forward 
                * Mathf.Abs(Utilities.Map(theta, 0, Mathf.PI, 0, 1)) 
                * rampedAmplitude;                    
        }

        rampedSpeed = Mathf.Lerp(rampedSpeed, speed, boid.TimeDelta);
        theta += boid.TimeDelta * rampedSpeed * Mathf.Deg2Rad;
        return force;
    }
}

