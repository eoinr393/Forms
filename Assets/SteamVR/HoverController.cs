using UnityEngine;
using System.Collections;

public class HoverController : MonoBehaviour {

    public float height = 100.0f;
    public float power = 10.0f;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int i = 0;
    void FixedUpdate()
    {
        RaycastHit rch;
        if (Physics.Raycast(transform.position, -Vector3.up, out rch, height))
        {
            Debug.Log("Thrust up!!" + (i++));
            GetComponent<Rigidbody>().AddForce(Vector3.up * power / rch.distance);
        }
        if (Physics.Raycast(transform.position, transform.forward, out rch, height))
        {
            Debug.Log("Thrust from forward!!" + (i++));
            GetComponent<Rigidbody>().AddForce(Vector3.up * power / rch.distance);
        }
    }
}
