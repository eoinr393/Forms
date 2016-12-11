using UnityEngine;
using System.Collections;

public class BoidRider : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    Quaternion targetQuaternion = Quaternion.identity;

    void OnTriggerEnter(Collider c)
    {
        GameObject other = c.gameObject;
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform.parent;
            other.GetComponent<ForceController>().enabled = false;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
            FindObjectOfType<ViveController>().boid = boid;
            boid.GetComponent<Harmonic>().enabled = true;
            boid.damping = 0.01f;
            Debug.Log(boid);
        }
    }

    void OnTriggerStay(Collider c)
    {
        GameObject other = c.gameObject;
        if (other.tag == "Player")
        {
            other.transform.position = Vector3.Lerp(other.transform.position, this.transform.position, Time.deltaTime);
        }
    }

}
