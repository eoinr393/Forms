using UnityEngine;
using BGE.Forms;
using System.Collections.Generic;

public class Palette : MonoBehaviour {
    public static Palette Instance;

    Palette()
    {
        Instance = this;
        
    }

    public static Color Random()
    {
       //return Color.red;
       return new Color(UnityEngine.Random.Range(0.7f, 2.0f), UnityEngine.Random.Range(0.1f, 0.2f), UnityEngine.Random.Range(0.1f, 0.2f));
    }

    public static Color RandomNot(Color c)
    {
        return new Color(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
    }

    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
