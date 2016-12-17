using UnityEngine;
using System.Collections;
using BGE.Forms;

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
                boid.GetComponent<Harmonic>().Activate(true);
                boid.GetComponent<NoiseWander>().Activate(true);
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.transform.parent = null;
            }
            RotateMe[] r = FindObjectsOfType<RotateMe>();
            foreach (RotateMe rm in r)
            {
                rm.speed = 0.1f;
            }

        }
	}
}
