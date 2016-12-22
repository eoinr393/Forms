using System;
using UnityEngine;
using BGE.Forms;
using System.Collections.Generic;

public class Palette
{
    public Color[] creatureColors;
    public Color[] backColors;

    public float h = 0.25f;
    public Palette(int cSeed, int bSeed, int count)
    {
        this.h = h;
        System.Random cRandom = new System.Random(cSeed);
        System.Random bRandom = new System.Random(bSeed);
        creatureColors = new Color[count];
        backColors = new Color[count];

        // Algorithm 1, random hue, same sat and value
        HSBColor baseColor = new HSBColor(
            Utilities.RandomRange(cRandom, 0.0f, 1.0f)
            , Utilities.RandomRange(cRandom, 0.5f, 1.0f)
            , Utilities.RandomRange(cRandom, 0.5f, 1.0f)
            );
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                Utilities.RandomRange(cRandom, 0.0f, 1.0f)
                , baseColor.s
                , baseColor.b
                );
            creatureColors[i] = thisColor.ToColor();
        }

        float b = 0.6f;
        float range = 0.2f;
        float bb = Utilities.RandomRange(bRandom, 0, 0.6f);
        for (int i = 0; i < count; i++)
        {
            HSBColor thisColor = new HSBColor(
                Utilities.RandomRange(bRandom, b - (range) , b + range)
                , baseColor.s
                , bb 
                );
            backColors[i] = thisColor.ToColor();
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
            creatureColors[i] = thisColor.ToColor();
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
            creatureColors[i] = thisColor.ToColor();
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
            creatureColors[i] = thisColor.ToColor();
        }
        */

    }

    public static Color Random()
    {
       return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }
}
