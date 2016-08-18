using UnityEngine;
using System.Collections;
using BGE;

public class SnakeGenerator : MonoBehaviour {

    public int numParts = 20;
    public float gap;
    public GameObject prefab;

    public void OnDrawGizmos()
    {
        for (int i = 0; i < numParts; i++)
        {
            Gizmos.DrawWireSphere(
        }
    }

    // Use this for initialization
    void Start () {
        for (int i = 0; i < numParts; i++)
        {
            GameObject part = (GameObject) GameObject.Instantiate<GameObject>(prefab);
            part.transform.parent = transform;
            part.transform.position = transform.position + new Vector3(0, 0, i * gap);            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
