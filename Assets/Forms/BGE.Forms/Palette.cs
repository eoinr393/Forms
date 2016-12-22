using System;
using UnityEngine;
using BGE.Forms;
using System.Collections.Generic;

public class Palette
{
    public Color[] colors;
    public float h = 0.25f;
    public Palette(int seed, int count, float h, int numCols, int rotation)
    {
        this.h = h;
        System.Random random = new System.Random(seed);
        colors = new Color[count];

        // Algorithm 1, random hue, same sat and value
        HSBColor baseColor = new HSBColor(
            Utilities.RandomRange(random, 0.0f, 1.0f)
            , Utilities.RandomRange(random, 0.5f, 1.0f)
            , Utilities.RandomRange(random, 0.5f, 1.0f)
            );
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                Utilities.RandomRange(random, 0.0f, 1.0f)
                , baseColor.s
                , baseColor.b
                );
            colors[i] = thisColor.ToColor();
        }

        // Algorithm 2, range of hues between 2 colours, same sat and value
        /*
        HSBColor baseColor1 = new HSBColor(
            Utilities.RandomRange(random, 0.0f, 1.0f)
            , Utilities.RandomRange(random, 0.0f, 1.0f)
            , Utilities.RandomRange(random, 0.5f, 1.0f)
            );
        HSBColor baseColor2 = new HSBColor(
            Utilities.RandomRange(random, 0.0f, 1.0f)
            , baseColor1.s
            , baseColor1.b
            );
        float interval = (baseColor2.h - baseColor1.h)/(float) count;
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                i * interval
                , baseColor1.s
                , baseColor1.b
                );
            colors[i] = thisColor.ToColor();
        }
        */

        // Algorithm 3 contrasting colours
        //Utilities.RandomRange(random, 0.0f, 1.0f);
        /*float s = 0.9f; // Utilities.RandomRange(random, 0.0f, 0.0f);
        float v = 0.8f; Utilities.RandomRange(random, 1.0f, 1.0f);
        float interval = 1.0f / numCols;
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                (h + ((i + rotation) * interval)) % 1.0f
                , s
                , v
                );
            colors[i] = thisColor.ToColor();
        }
        */
        /*
        float s = Utilities.RandomRange(random, 0.0f, 1.0f);
        float v = Utilities.RandomRange(random, 0.0f, 1.0f);
        float start = 0.8f;
        float end = 1.0f;
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                Utilities.RandomRange(start, end)                
                , s
                , v
                );
            colors[i] = thisColor.ToColor();
        }
        */

    }

    public static Color Random()
    {
       return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }
}
