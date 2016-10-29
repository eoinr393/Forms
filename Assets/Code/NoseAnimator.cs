using UnityEngine;
using System.Collections;

public class NoseAnimator : Animator {
    private Harmonic harmonic;
    public float theta = 0;
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
                theta = harmonic.theta;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (harmonic != null)
        {
            float offset = rotationOffset * Mathf.Deg2Rad;

            float angle = Utilities.Map(Mathf.Sin((harmonic.theta + offset)), -1, 1, 0, amplitude);

            transform.localScale = new Vector3(1, 1, angle);
        }
	}
}
