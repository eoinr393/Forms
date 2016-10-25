using UnityEngine;
using System.Collections;

public class TailAnimator : MonoBehaviour {
    public enum Axis { Y, X };
    public Axis axis = Axis.Y;

    public Boid boid;
    public float theta = 0;
    public float amplitude = 40.0f;
    public float speed = 0.1f;

    [Range(0, 2)]
    public float wigglyness = 1;
    // Use this for initialization
    void Start()
    {
        theta = 0;
    }

    public float lerpedAmplitude;
    // Update is called once per frame
    void Update()
    {
        if (boid != null)
        {
            float angle = Mathf.Sin(theta) * amplitude * wigglyness;
            switch (axis)
            {
                case Axis.Y:
                    transform.localRotation = Quaternion.Euler(0, angle, 0);
                    break;
                case Axis.X:
                    transform.localRotation = Quaternion.Euler(angle, 0, 0);
                    break;
            }
            theta += speed * Time.deltaTime * boid.acceleration.magnitude;
        }
    }
}
