using UnityEngine;
using System.Collections.Generic;
using BGE;

struct CreaturePart
{
    public Vector3 position;
    public float size;

    

    public CreaturePart(Vector3 position, float scale)
    {
        this.position = position;
        this.size = scale;
    }
}

public class CreatureGenerator : MonoBehaviour {
    
    [Range(0.0f, Mathf.PI * 2.0f)]
    public float theta = 0.1f;

    [Range(0.0f, 10.0f)]
    public float frequency = 1.0f;


    [Range(1, 100)]
    public int numParts = 5;

    [Range(-10.0f, 10.0f)]
    public float gap = 1;

    public Color color = Color.blue;

    [Range(1.0f, 100.0f)]
    public float verticalSize = 1.0f;

    public bool flatten = false;

    public GameObject prefab;


    public void OnDrawGizmos()
    {
        List<CreaturePart> creatureParts = CreateCreatureParams();
        Gizmos.color = Color.yellow;
        foreach(CreaturePart cp in creatureParts)
        {
            Gizmos.DrawWireSphere(cp.position, cp.size * 0.5f);
        }
    }

    void CreateCreature()
    {
        List<CreaturePart> creatureParts = CreateCreatureParams();
        Gizmos.color = Color.yellow;
        foreach(CreaturePart cp in creatureParts)
        {
            GameObject part = GameObject.Instantiate<GameObject>(prefab);
            part.transform.position = cp.position;
            part.GetComponent<Renderer>().material.color = Color.blue;
            part.transform.localScale = new Vector3(cp.size, cp.size, cp.size);
            part.transform.parent = transform;
        }
    }

    List<CreaturePart> CreateCreatureParams()
    {
        List<CreaturePart> cps = new List<CreaturePart>();
        float thetaInc = (Mathf.PI * frequency) / (numParts);
        float theta = this.theta;
        float lastGap = 0;
        Vector3 pos = transform.position;
        for (int i = 0; i < numParts; i++)
        {            
            float partSize = verticalSize * Mathf.Abs(Mathf.Sin(theta));
            theta += thetaInc;
            pos.z += ((lastGap + partSize) / 2) + gap;
            if (flatten)
            {
                pos.y -= (partSize / 2);
            }
            lastGap = partSize;
            cps.Add(new CreaturePart(pos, partSize));
        }
        return cps;
    }

    // Use this for initialization
    void Start () {
        CreateCreature();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
