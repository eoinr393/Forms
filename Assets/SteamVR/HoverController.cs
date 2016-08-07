using UnityEngine;
using System.Collections;

public class HoverController : MonoBehaviour {

    public float height = 100.0f;
    public float power = 1;
    public GameObject head;
    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int i = 0;
    void FixedUpdate()
    {
        float contWalk = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * contWalk);
        RaycastHit rch;
        if (Physics.Raycast(transform.position, -Vector3.up, out rch, height))
        {
            //Debug.Log("Thrust up!!" + (i++));
            //GetComponent<Rigidbody>().AddForce(Vector3.up * power * (1.0f - (rch.distance / height)));
            Vector3 target = rch.point + (Vector3.up * height);
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * power);
        }
        //if (Physics.Raycast(transform.position, transform.forward, out rch, height))
        //{
        //    Debug.Log("Thrust from forward!!" + (i++));
        //    GetComponent<Rigidbody>().AddForce(Vector3.up * power / rch.distance);
        //}

        
    }
}
