using UnityEngine;
using System.Collections;

using BGE;

public class VaryWiggle : MonoBehaviour {

    Harmonic harmonic;
    Boid boid;
    float initialSpeed;
    float initialBoidSpeed;
    float initialAmplitude;

    [Range(0, 1)]
    public float speedVariation = 0.5f;

    [Range(0, 1)]
    public float amplitudeVariation = 0.5f;

    public bool glide = false;

    // Use this for initialization
    void Start () {
        harmonic = GetComponent<Harmonic>();
        boid = GetComponent<Boid>();
        initialBoidSpeed = boid.maxSpeed;
        initialAmplitude = harmonic.amplitude;
        initialSpeed = harmonic.speed;
        StartCoroutine("VaryWiggleInterval");
	}

    System.Collections.IEnumerator VaryWiggleInterval()
    {
        while (true)
        {
            Debug.Log("Default");
            harmonic.amplitude = initialAmplitude;
            harmonic.speed = initialSpeed;
            boid.applyGravity = false;
            harmonic.enabled = true;
            yield return new WaitForSeconds(Random.Range(3, 10));
            Debug.Log("Accelerated");
            harmonic.enabled = true;
            harmonic.amplitude = Random.Range(initialAmplitude - (initialAmplitude * speedVariation), initialAmplitude + (initialAmplitude * speedVariation));
            harmonic.speed = Random.Range(initialSpeed - (initialSpeed * amplitudeVariation), initialSpeed + (initialSpeed * amplitudeVariation));

            float variationThisTime = harmonic.speed / initialSpeed;

            boid.maxSpeed = initialBoidSpeed * variationThisTime;
            boid.applyGravity = false;
            harmonic.enabled = false;
            yield return new WaitForSeconds(Random.Range(5, 10));
            if (glide)
            {
                harmonic.amplitude = initialAmplitude * 0.15f;
                harmonic.speed = initialSpeed;
                boid.applyGravity = true;
                boid.gravityAcceleration = 0;
                Debug.Log("Gliding");
            }
            yield return new WaitForSeconds(Random.Range(12, 20));
        }
    }
}
