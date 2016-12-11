using UnityEngine;
using System.Collections;

public class DetatchFromBoid : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
            if (boid != null)
            {
                boid.GetComponent<Harmonic>().enabled = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.parent = null;
            }
        }
	}
}
