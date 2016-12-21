using UnityEngine;
using BGE.Forms;
using System.Collections.Generic;

public class Palette : MonoBehaviour
{
    public Color[] colors;

    public Palette(int count)
    {
        colors = new Color[count];

        for (int i = 0; i < count; i++)
        {
            colors[i] = new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
        }
    }

    public static Color Random()
    {
       return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }
}
