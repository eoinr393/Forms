using UnityEngine;
using System.Collections.Generic;

public class FormationGenerator : MonoBehaviour {
    public int sideWidth = 10;
    public float gap = 50;

    [Range(0.0f, 1.0f)]
    public float variance = 0.1f;

    public GameObject prefab;

    private List<Vector3> positions = new List<Vector3>();


    void GenerateCreaturePosition(Vector3 pos, int current, int depth)
    {
        positions.Add(pos);
        if (current < depth)
        {
            if (pos.x <= transform.position.x)
            {
                Vector3 left = new Vector3(-1, 1, -1) * gap;
                left.x *= Random.Range(1.0f, 1.0f - variance);
                left.y *= Random.Range(1.0f, 1.0f - variance);
                left.z *= Random.Range(1.0f, 1.0f - variance);
                GenerateCreaturePosition(pos + left, current + 1, depth);
                
            }
            if (pos.x >= transform.position.x)
            {
                Vector3 right = new Vector3(1, 1, -1) * gap;
                right.x *= Random.Range(1.0f, 1.0f - variance);
                right.y *= Random.Range(1.0f, 1.0f - variance);
                right.z *= Random.Range(1.0f, 1.0f - variance);
                GenerateCreaturePosition(pos + right, current + 1, depth);
            }
        }
    }

    void GeneratePositions()
    {
        positions.Clear();
        GenerateCreaturePosition(transform.position, 0, 10);
    }

    void OnDrawGizmos()
    {
        GeneratePositions();
        if (isActiveAndEnabled)
        {
            foreach (Vector3 pos in positions)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(pos, Vector3.one * gap * 0.5f);
            }
        }
    }


	// Use this for initialization
	void Start () {
        GeneratePositions();
        GameObject leader = null;

        for (int i = 0; i < positions.Count; i++)
        {
            if (i == 0)
            {
                leader = GameObject.Instantiate<GameObject>(prefab);
                leader.transform.position = positions[i];
                leader.transform.parent = transform;
                leader.SetActive(true);
            }
            else
            {
                GameObject go = GameObject.Instantiate<GameObject>(prefab);
                go.transform.position = positions[i];
                go.transform.parent = transform;
                go.SetActive(true);
                //Formation formation = go.GetComponent<Formation>();
                //if (formation == null)
                //{
                //    formation = go.AddComponent<Formation>();
                //}
                //formation.leader = leader.GetComponent<Boid>();
            }       
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
