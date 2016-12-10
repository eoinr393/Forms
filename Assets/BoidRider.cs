using UnityEngine;
using System.Collections;

public class BoidRider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("Trigger!!!" + c);
        GameObject other = c.gameObject;
        if (other.tag == "Player")
        {
            other.transform.position = this.transform.position;
            other.transform.forward = this.transform.parent.forward;
            other.transform.parent = this.transform.parent;
            other.GetComponent<BGE.FPSController>().enabled = false;
            Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
            Debug.Log(boid);
        }
    }
}
