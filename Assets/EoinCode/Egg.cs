using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour {

	GameObject parent1;
	GameObject parent2;

	MatingDance md;

	Vector3 p1seek;
	Vector3 p2seek;
	Vector3 eggpos;
	//CreateCreature
	CreatureReproduction cr = new CreatureReproduction();

	bool hatchEgg = true;

	// calculate the seek points
	void FixedUpdate () {
		if (parent1 != null && parent2 != null) {
			Vector3[] arr = md.Calculate ();
			p1seek = arr [0];
			p2seek = arr [1];
			//Debug.Log ("eggcellent");
			eggpos = md.sphereCentre;

			//hatch the egg
			if (hatchEgg) {
				//spawn and color the creature
				Color c = (parent1.GetComponent<CreatureGenerator>().color + parent1.GetComponent<CreatureGenerator>().color)/2;

				Utilities.RecursiveSetColor(Instantiate (cr.Create (parent1, parent2), eggpos, transform.rotation), c);
				hatchEgg = false;
			}
		}
	}

	public void setParents(GameObject p1, GameObject p2){
		this.parent1 = p1;
		this.parent2 = p2;
		md = new MatingDance (parent1, parent2);
	}

	//return the position the creature needs to seek
	public Vector3 GetSeekPos(GameObject parent){
		if (parent == parent1) {
			return p1seek;
		}
		return p2seek;
	}

	public void Destroy(){
		Destroy (this.gameObject);
	}

	public Vector3 getCentre(){
		return eggpos;
	}
}
