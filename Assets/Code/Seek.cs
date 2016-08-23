using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Seek : SteeringBehaviour
{
    public Vector3 target = Vector3.zero;
    public bool seekPlayer = false;
    
    public override Vector3 Calculate()
    {
        return boid.SeekForce(target);    
    }
}
