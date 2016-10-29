
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class School: MonoBehaviour
{
	[HideInInspector]
	public int numFlocking = 0;

	[HideInInspector]
	public int numCurious = 0;

	public float neighbourDistance;

	public float radius = 100;

	[HideInInspector]
	public volatile List<Boid> boids = new List<Boid>();

	[Range(0, 2)]
	public float timeMultiplier = 1.0f;
 
	[Header("Debug")]
	public bool drawGizmos;        


	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
		
	}

	void Start()
	{
	}
}
