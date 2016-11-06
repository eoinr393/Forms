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
            harmonic.amplitude = initialAmplitude;
            harmonic.speed = initialSpeed;
            yield return new WaitForSeconds(Random.Range(3, 10));
            harmonic.amplitude = Random.Range(initialAmplitude - (initialAmplitude * speedVariation), initialAmplitude + (initialAmplitude * speedVariation));
            harmonic.speed = Random.Range(initialSpeed - (initialSpeed * amplitudeVariation), initialSpeed + (initialSpeed * amplitudeVariation));

            float variationThisTime = harmonic.speed / initialSpeed;

            boid.maxSpeed = initialBoidSpeed * variationThisTime;

            yield return new WaitForSeconds(Random.Range(3, 10));
            if (glide)
            {
                harmonic.amplitude = initialAmplitude * 0.15f;
                harmonic.speed = initialSpeed;                
            }
            yield return new WaitForSeconds(Random.Range(5, 7));
        }
    }
}
