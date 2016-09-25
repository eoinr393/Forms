using UnityEngine;
using System.Collections.Generic;
using BGE;

struct CreaturePart
{
    public Vector3 position;
    public float size;
    public enum Part { head, body, fin };
    public Part part;
    public GameObject prefab;

    public CreaturePart(Vector3 position, float scale, Part part, GameObject prefab)
    {
        this.position = position;
        this.size = scale;
        this.part = part;
        this.prefab = prefab;
    }
}

public class CreatureGenerator : MonoBehaviour {
    
    [Range(0.0f, Mathf.PI * 2.0f)]
    public float theta = 0.1f;

    [Range(0.0f, 10.0f)]
    public float frequency = 1.0f;


    [Range(1, 1000)]
    public int numParts = 5;

    [Range(-1000.0f, 1000.0f)]
    public float gap = 1;

    public Color color = Color.blue;

    [Range(1.0f, 5000.0f)]
    public float verticalSize = 1.0f;

    public bool flatten = false;

    public GameObject headPrefab;
    public GameObject bodyPrefab;

    public string fins;

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
            GameObject part = GameObject.Instantiate<GameObject>(cp.prefab);
            part.transform.position = cp.position;
            part.GetComponent<Renderer>().material.color = color;
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
            pos.z -= ((lastGap + partSize) / 2) + gap;
            if (flatten)
            {
                pos.y -= (partSize / 2);
            }
            lastGap = partSize;
            cps.Add(new CreaturePart(pos
                , partSize
                , (i == 0) ? CreaturePart.Part.head : CreaturePart.Part.body
                , (i == 0) ? headPrefab : bodyPrefab
                ));
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
