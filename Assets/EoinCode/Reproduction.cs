using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
/// <summary>
///script assumes the reproduction script is attatched to creatures
/// heads.
/// </summary>
public class Reproduction : SteeringBehaviour {

	public Light babyLight;
	public GameObject mate;
	GameObject egg;
	CreatureScan cs = new CreatureScan();
	public float sightLength = 50;
	public bool ismating = false;

	Vector3 seekpos;

	//find a mate to mate with
	void findMate(){
		List<GameObject> creatures = cs.ScanOther (this.transform.position, sightLength, this.transform.parent.gameObject);
	
		foreach (GameObject child in creatures) {

			foreach(Transform c in child.transform){
				
				if (c.name.ToLower ().Contains ("head")) {
					
					if (confirmMate (c.gameObject)) {
						Debug.Log (transform.parent.name + " is mating with " + mate.name);
						return;
					}
				}
			}
		}
	}

	//checks if the found mate is available to mate
	bool confirmMate(GameObject mateHead){
		Reproduction materep = mateHead.GetComponent<Reproduction> ();

		if (materep != null && !materep.ismating) {
			
			ismating = true; //lock self first

			//set mate and egg to that of the mate
			if (materep.setMate (this.transform.parent.gameObject)) {
				mate = mateHead.transform.parent.gameObject;
				egg = materep.getEgg ();
				return true;
			}
		}
		return false;
	}


	public override Vector3 Calculate(){
		if(!ismating)
			findMate ();
		if (ismating && mate != null && egg != null) {
			seekpos = egg.GetComponent<Egg>().GetSeekPos (this.transform.parent.gameObject);

			return boid.ArriveForce(seekpos,30,1);
		}
			
		return Vector3.zero;
	}
		
	//called from other creature to set mates, create egg
	public bool setMate(GameObject mate){
		if (!ismating && enabled) {
			Debug.Log (transform.parent.name + "is mating");
			ismating = true;
			this.mate = mate;

			//create new egg
			egg = createEgg(mate);

			return true;
		}
		return false;
	}

	//spawns an empty gameobject with the egg script attatched
	GameObject createEgg(GameObject mate){
		GameObject eggobject = Instantiate (new GameObject(), transform.position, transform.rotation);
		eggobject.name = "Egg(" + this.transform.parent.gameObject.name + ", " + mate.gameObject.name + ")";
		eggobject.AddComponent<Egg> ();
		eggobject.GetComponent<Egg> ().setParents (this.transform.parent.gameObject, mate);

		return eggobject;

	}

	public void resetMating(){
		ismating = false;
		this.enabled = false;
		this.mate = null;
		this.egg = null;
	}

	public GameObject getEgg(){
		return egg;
	}

	void OnDrawGizmos(){
		if (mate != null) {
			Egg e = egg.GetComponent<Egg> ();
			Vector3 centre = e.getCentre ();

			Gizmos.color = Color.green;
			Gizmos.DrawLine (this.transform.position, mate.transform.position);
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere (centre, 5);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere (seekpos, 1);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine (transform.position, seekpos);
		}
	}
}
