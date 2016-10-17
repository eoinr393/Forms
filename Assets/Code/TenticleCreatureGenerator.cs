using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TenticleCreatureGenerator : MonoBehaviour {
    public GameObject tenticlePrefab;
    public GameObject headPrefab;

    public int numTenticles = 8;
    public float radius = 20;
    public float scale = 1.0f;

    List<CreaturePart> CreateCreatureParams()
    {
        List<CreaturePart> list = new List<CreaturePart>();

        CreaturePart headPart = new CreaturePart(transform.position, radius * 2, CreaturePart.Part.head, headPrefab, Quaternion.identity);
        list.Add(headPart);

        float thetaInc = Mathf.PI * 2.0f / (numTenticles);
        for (int i = 0; i < numTenticles; i++)
        {
            float theta = i * thetaInc;
            Vector3 pos = new Vector3();
            pos.x = transform.position.x + Mathf.Sin(theta) * radius;
            pos.z = transform.position.z - Mathf.Cos(theta) * radius;
            pos.y = transform.position.y;
            CreaturePart cp = new CreaturePart(pos, scale
                , CreaturePart.Part.tenticle
                , tenticlePrefab, Quaternion.AngleAxis(Mathf.Rad2Deg * -theta, Vector3.up));
            list.Add(cp);
        }

        return list;
    }

    void CreateCreature()
    {
        List<CreaturePart> parts = CreateCreatureParams();
        Boid boid = null;
        for(int i = 0; i < parts.Count; i ++)
        {
            CreaturePart part = parts[i];
            
            GameObject newPart = GameObject.Instantiate<GameObject>(part.prefab);            
            newPart.transform.position = part.position;
            newPart.transform.rotation = part.rotation;
            newPart.transform.localScale = new Vector3(scale, scale, scale);
            newPart.transform.parent = transform;

            Boid thisBoid = newPart.GetComponentInChildren<Boid>();
            if (thisBoid != null)
            {
                boid = thisBoid;
            }

            FinAnimator anim = newPart.GetComponentInChildren<FinAnimator>();
            if (anim != null)
            {

                anim.boid = boid;
            }
        }
    }

    void Awake()
    {
        CreateCreature();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
