﻿using UnityEngine;
using System.Collections;

public class FinAnimator : MonoBehaviour {

    public enum Side { left, right };
    public Side side = Side.left;

    public Boid boid;
    private Harmonic harmonic;
    float theta = 0;
    float initialAmplitude;
    public float amplitude = 40.0f;

    [Range(0, 360)]
    public float rotationOffset = 220;

    [Range(0, 2)]
    public float wigglyness = 1;
	// Use this for initialization
	void Start () {
        if (boid != null)
        {
            harmonic = boid.GetComponent<Harmonic>();
            if (harmonic != null)
            {
                initialAmplitude = harmonic.amplitude;
            }
        }
    }

    public float leftRightAmp;
    public float maxBank = float.MinValue;
    // Update is called once per frame
    void Update () {
        if (harmonic != null)
        {
            float offset = rotationOffset * Mathf.Deg2Rad;
            // Left right stuff
            if (side == Side.right && boid.bank < 0)
            {
                leftRightAmp = 40 - Mathf.Abs(boid.bank); 
            }

            if (side == Side.left && boid.bank > 0)
            {
                leftRightAmp = 40 - Mathf.Abs(boid.bank);
            }

            if (Mathf.Abs(boid.bank) > maxBank)
            {
                maxBank = Mathf.Abs(boid.bank);
            }

            float angle = Mathf.Sin((harmonic.theta + offset))
                * (harmonic.rampedAmplitude / initialAmplitude) * leftRightAmp * wigglyness;
            transform.localRotation = Quaternion.Euler(angle, 0, 0);
            theta += Time.deltaTime;
        }
	}
}
