using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Seek : SteeringBehaviour
{
    public Vector3 seekTargetPos = Vector3.zero;
    public bool seekPlayer = false;
    
    public override Vector3 Calculate()
    {
        return boid.Seek(seekTargetPos);    
    }
}
