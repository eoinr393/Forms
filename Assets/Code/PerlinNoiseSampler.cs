using UnityEngine;
using System.Collections;


public class PerlinNoiseSampler:Sampler
{

    [Range(0, 1)]
    public float low = 1.0f;
    public float high = 0.0f;

    [Range(0, 10)]
    public float origin = 0;

    [Range(0.0f, 1.0f)]
    public float scale = 0.2f;

    [Range(0, 10000)]
    public float height = 100;
    
    public PerlinNoiseSampler()
    {
    }

    public override float Sample(float x, float y)
    {
        float noise = Mathf.PerlinNoise(origin + (x * scale), origin + (y * scale));
        noise = (noise > high) ? (noise - high) : high;
        float sample =  noise * height;
        return sample;
    }
}
