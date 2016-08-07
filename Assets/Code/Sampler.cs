﻿using UnityEngine;
using System.Collections;

public abstract class Sampler : MonoBehaviour
{
    public enum operations { add, subtract, multiply, divide };
    public operations operation = operations.add;

    public Sampler()
    {
    }

    public abstract float Sample(float x, float y);

    public float Operate(float input, float x, float y)
    {
        switch (operation)
        {
            case operations.add:
                return input + Sample(x, y);
            case operations.subtract:
                return input - Sample(x, y);
            case operations.multiply:
                return input * Sample(x, y);
            case operations.divide:
                return input / Sample(x, y);
            default:
                return 0;
        }
    }
}
