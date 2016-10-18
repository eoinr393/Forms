using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TenticleCreatureGenerator : MonoBehaviour {
    public GameObject tenticlePrefab;
    public GameObject headPrefab;

    public int numTenticles = 8;
    public float radius = 20;
    public float headScale = 1.0f;
    public float tenticleScale = 1.0f;

    public Color color;

    public void OnDrawGizmos()
    {
        List<CreaturePart> cps = CreateCreatureParams();
        Gizmos.color = Color.cyan;
        foreach (CreaturePart cp in cps)
        {
            switch (cp.part)
            {
                case CreaturePart.Part.head:
                    Gizmos.DrawWireSphere(cp.position, cp.size);
                    break;
                case CreaturePart.Part.tenticle:
                    Gizmos.DrawWireSphere(cp.position, cp.size / 10);
                    break;
            }
        }

    }

    List<CreaturePart> CreateCreatureParams()
    {
        List<CreaturePart> list = new List<CreaturePart>();

        CreaturePart headPart = new CreaturePart(transform.position, headScale, CreaturePart.Part.head, headPrefab, Quaternion.identity);
        list.Add(headPart);

        float thetaInc = Mathf.PI * 2.0f / (numTenticles);
        for (int i = 0; i < numTenticles; i++)
        {
            float theta = i * thetaInc;
            Vector3 pos = new Vector3();
            pos.x = transform.position.x + Mathf.Sin(theta) * radius;
            pos.z = transform.position.z - Mathf.Cos(theta) * radius;
            pos.y = transform.position.y;
            CreaturePart cp = new CreaturePart(pos, tenticleScale
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
            newPart.transform.localScale = new Vector3(part.size, part.size, part.size);           

            Boid thisBoid = newPart.GetComponentInChildren<Boid>();
            if (thisBoid != null)
            {
                boid = thisBoid;
                newPart.transform.parent = transform;
            }

            FinAnimator anim = newPart.GetComponentInChildren<FinAnimator>();
            if (anim != null)
            {
                newPart.transform.parent = boid.transform;
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
        Utilities.RecursiveSetColor(this.gameObject, color);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
