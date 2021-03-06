﻿using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {

    Harmonic harmonic;
    Boid boid;
    float theta;
	// Use this for initialization
	void Start () {
        harmonic = GetComponent<Harmonic>();
        boid = GetComponent<Boid>();
    }
	
	// Update is called once per frame
	void Update () {
        theta = (harmonic.theta - (Mathf.PI / 2))  % (Mathf.PI * 2.0f);
        CreatureManager.Log("Harmonic theta:" + theta);
        if (theta < Mathf.PI)
        {
            boid.position += (boid.up * Mathf.Abs(theta) * 10);
        }
	}
}
