using UnityEngine;
using System.Collections;
using BGE.Forms;

public class Coalesse : MonoBehaviour {
    School flock;
    public GameObject player;
	// Use this for initialization
	void Start () {
        flock = GetComponent<School>();
        StartCoroutine("CoalesseBoids");
	}

    System.Collections.IEnumerator CoalesseBoids()
    {
        while (true)
        {
            Debug.Log("Schooling");
            foreach (Boid boid in flock.boids)
            {
                boid.GetComponent<Seperation>().enabled = false;
                boid.GetComponent<Cohesion>().enabled = true;
                boid.GetComponent<Alignment>().enabled = true;
                boid.GetComponent<JitterWander>().enabled = true;
                boid.GetComponent<Seek>().enabled = false;
            }
            yield return new WaitForSeconds(Random.Range(20.0f, 30.0f));
            Debug.Log("Coalessing");
            if (Vector3.Distance(player.transform.position, flock.transform.position) < flock.radius)
            {

                foreach (Boid boid in flock.boids)
                {                    
                    // Only affect boids in front of the player
                    Vector3 toBoid = boid.transform.position - player.transform.position;
                    if ((Vector3.Dot(player.transform.forward, toBoid) >= 0) && (Random.Range(0, 0.5f) < 0.5f))
                    {
                        boid.GetComponent<Seperation>().enabled = false;
                        boid.GetComponent<Cohesion>().enabled = false;
                        boid.GetComponent<Alignment>().enabled = true;
                        boid.GetComponent<JitterWander>().enabled = true;
                        boid.GetComponent<Seek>().enabled = true;
                        Vector3 unit = UnityEngine.Random.insideUnitSphere;
                        boid.GetComponent<Seek>().target = player.transform.position + (Random.insideUnitSphere * 10);
                        boid.GetComponent<Seek>().target.y += 10;
                    }
                }
            }
            yield return new WaitForSeconds(Random.Range(4.0f, 6.0f));
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
