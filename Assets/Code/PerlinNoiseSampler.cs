using UnityEngine;
using System.Collections;


public class PerlinNoiseSampler:Sampler
{
    
    public float origin = 0;
    public float scale = 0.2f;
    public float height;
    

    public PerlinNoiseSampler()
    {
    }

    public override float Sample(float x, float y)
    {
        float sample = Mathf.PerlinNoise(origin + (x * scale), origin + (y * scale)) * height;
        return sample;
    }
}
