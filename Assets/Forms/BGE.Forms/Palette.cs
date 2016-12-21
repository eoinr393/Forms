using UnityEngine;
using BGE.Forms;
using System.Collections.Generic;

public class Palette
{
    public Color[] colors;

    public Palette(int count)
    {
        colors = new Color[count];

        HSBColor baseColor = new HSBColor(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.5f, 1.0f));
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                UnityEngine.Random.Range(0.0f, 1.0f)
                , baseColor.s
                , baseColor.b
                );
            colors[i] = thisColor.ToColor();
        }
    }

    public static Color Random()
    {
       return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }
}
