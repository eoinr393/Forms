using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatingDance  {
	//public GameObject testobject = GameObject.CreatePrimitive (PrimitiveType.Cube);
	GameObject parent1;
	public GameObject parent2;
	float sphererad = 5f;
	public Vector3 sphereCentre;
	Vector3 seekpoint;
	Vector3 rotpoint;
	Vector3 oppseekpoint;
	bool changepoint = false;
	Vector3 angles = new Vector3 (0.7f, 0.5f,0.2f);

	public MatingDance(GameObject parent1, GameObject parent2){
		this.parent1 = parent1;
		this.parent2 = parent2;
	}

	public Vector3[] Calculate()	{

		//get point between the creatures, create sphere
		sphereCentre = Vector3.Lerp(parent1.transform.position, parent2.transform.position, 0.5f);
		Debug.Log ("line length = " + Vector3.Distance (parent1.transform.position, parent2.transform.position) + " from head to sphere = " + Vector3.Distance (parent1.transform.position, sphereCentre));
		//get the seekpoint
		if (!changepoint) {
			seekpoint = (Random.onUnitSphere * sphererad) + sphereCentre;
			changepoint = true;
		}
		//rotate the seekpoint around the centre of the sphere
		Vector3 dir = seekpoint - sphereCentre;
		dir = Quaternion.Euler (angles) * dir;
		seekpoint = dir + sphereCentre;
		//get the opposite point of the seekpoint
		oppseekpoint = seekpoint - (seekpoint - sphereCentre) * 2;

		//radonmly change the speed at which the points rotate
		//angles = new Vector3 (Random.Range (1, 9) * 0.1f, Random.Range (1, 9) * 0.1f, Random.Range (1, 9) * 0.1f);
		
		Vector3[] arr = new [] { seekpoint, oppseekpoint };
		return arr;
	}

}
