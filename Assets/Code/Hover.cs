using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
    
public class Hover:Harmonic
{
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(boid.transform.position, boid.transform.position + (force * 50)); 
    }

    public override Vector3 Calculate()
    {
        float n = Mathf.Sin(theta);
        //CreatureManager.print("N: " + n);

        rampedAmplitude = Mathf.Lerp(rampedAmplitude, amplitude, boid.TimeDelta);

        Vector3 force = Vector3.zero;
        rampedSpeed = Mathf.Lerp(rampedSpeed, speed, boid.TimeDelta);
        if (n < 0)
        {
            force = Vector3.up * Mathf.Abs(n) * rampedAmplitude;
        }
        
        theta += boid.TimeDelta * rampedSpeed * Mathf.Deg2Rad;
        return force;
    }
}

