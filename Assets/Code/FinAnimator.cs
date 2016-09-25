using UnityEngine;
using System.Collections;

public class FinAnimator : MonoBehaviour {
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
        harmonic = boid.GetComponent<Harmonic>();
        initialAmplitude = harmonic.amplitude;
    }
	
	// Update is called once per frame
	void Update () {
        float offset = rotationOffset * Mathf.Deg2Rad;
        float angle = Mathf.Sin((harmonic.theta + offset))
            * (harmonic.rampedAmplitude / initialAmplitude) * amplitude * wigglyness;
        transform.localRotation = Quaternion.Euler(angle, 0, 0);
        theta += Time.deltaTime;
	}
}
