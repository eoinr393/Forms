
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
	public List<GameObject> enemies = new List<GameObject>();
	[HideInInspector]
	public List<Vector3> enemyPositions = new List<Vector3>();

	[Range(0, 2)]
	public float timeMultiplier = 1.0f;
 
	[Header("Debug")]
	public bool drawGizmos;        

	[HideInInspector]
	public float threadTimeDelta;

	public Vector3 centreOfMass;

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
		
	}


	void Start()
	{
	}

	
	public void Update()
	{
		centreOfMass = transform.position;
	}
}
