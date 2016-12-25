using UnityEngine;
using System.Collections;

namespace BGE.Forms
{
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
                other.transform.parent = this.transform;
                other.GetComponent<ForceController>().enabled = false;
                other.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Boid boid = Utilities.FindBoidInHierarchy(this.gameObject);
                FindObjectOfType<ViveController>().boid = boid;
                boid.GetComponent<PlayerSteering>().Activate(true);
                boid.GetComponent<Harmonic>().Activate(true);

                HarmonicController hc = boid.GetComponent<HarmonicController>();
                if (boid.GetComponent<HarmonicController>() != null)
                {
                    hc.enabled = false;
                    boid.GetComponent<Harmonic>().amplitude = hc.initialAmplitude;
                    boid.GetComponent<Harmonic>().speed = hc.initialSpeed;
                }
                
                Constrain con = boid.GetComponent<Constrain>();
                if (con != null)
                {
                    con.Activate(false);
                }
                boid.GetComponent<NoiseWander>().Activate(false);
                
                RotateMe r = GetComponent<RotateMe>();
                if (r != null)
                {
                    r.speed = 0;
                }
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
}
