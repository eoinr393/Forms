using UnityEngine;
using System.Collections;

using BGE;

public class VaryWiggle : MonoBehaviour {

    Harmonic harmonic;
    float initialSpeed;
    float initialAmplitude;

    [Range(0, 1)]
    public float speedVariation = 0.5f;

    [Range(0, 1)]
    public float amplitudeVariation = 0.5f;


    // Use this for initialization
    void Start () {
        harmonic = GetComponent<Harmonic>();
        initialAmplitude = harmonic.amplitude;
        initialSpeed = harmonic.speed;
        StartCoroutine("VaryWiggleInterval");
	}

    System.Collections.IEnumerator VaryWiggleInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 10));
            harmonic.amplitude = Random.Range(initialAmplitude - (initialAmplitude * speedVariation), initialAmplitude + (initialAmplitude * speedVariation));
            harmonic.speed = Random.Range(initialSpeed - (initialSpeed * amplitudeVariation), initialSpeed + (initialSpeed * amplitudeVariation));
        }
    }
}
