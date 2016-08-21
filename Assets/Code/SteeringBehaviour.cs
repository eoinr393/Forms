using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public abstract class SteeringBehaviour: MonoBehaviour
{
    [Range(0.0f, 200.0f)]
    public float weight = 1.0f;

    public abstract Vector3 Calculate();

    [HideInInspector]
    public Boid boid;

    public void Start()
    {
        boid = GetComponent<Boid>();
    }
}
