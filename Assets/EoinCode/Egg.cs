﻿using System.Collections;
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

	bool hatchEgg = false;

	Light l1;
	Light l2;

	float spawntime;
	float hatchTime = 10;
	float maxCreatureSize;

	void Start(){
		spawntime = Time.time;
	}

	// calculate the seek points
	void FixedUpdate () {
		if (parent1 != null && parent2 != null) {
			Vector3[] arr = md.Calculate ();
			p1seek = arr [0];
			p2seek = arr [1];
			//Debug.Log ("eggcellent");
			eggpos = md.sphereCentre;
			l1.transform.position = eggpos;
			l2.transform.position = eggpos;

			//hatch the egg destroy the egg
			if (hatchEgg) {
				hatchEgg = false;
				//spawn and color the creature
				Color c = (parent1.GetComponent<CreatureGenerator> ().color + parent2.GetComponent<CreatureGenerator> ().color) / 2;

				Debug.Log ("Instantiating");
				//spawn creature and set it up
				GameObject baby = Instantiate (cr.Create (parent1, parent2), eggpos, transform.rotation);
				Utilities.RecursiveSetColor (baby, c);

				//turn of reproduction and turn on movement behaviours
				resetBehaviours(baby);
				resetBehaviours (parent1);
				resetBehaviours (parent2);
					
				//reset parents
				parent1.GetComponent<CreatureGenerator> ().headPrefab.GetComponent<Reproduction> ().resetMating();
				parent2.GetComponent<CreatureGenerator> ().headPrefab.GetComponent<Reproduction> ().resetMating();
				Destroy (l1);
				Destroy (l2);
				Destroy (this.gameObject);
			} 

			//lights, camera, action
			if (l1 != null && l2 != null) {
				l1.intensity = Mathf.PingPong (Time.time, 5);
				l2.intensity = Mathf.PingPong (Time.time / 2, 5);

				l1.range = (Mathf.PingPong (Time.time, maxCreatureSize) + 0.1f) * 3;
				l2.range = (Mathf.PingPong (Time.time / 5, maxCreatureSize) + 0.1f) * 3;
			}

			if (Time.time - spawntime > hatchTime)
				hatchEgg = true;
		}


	}

	//set the parent of this egg
	public void setParents(GameObject p1, GameObject p2){
		this.parent1 = p1;
		this.parent2 = p2;
		spawnLight ();
		//largest creature
		maxCreatureSize = p1.GetComponent<CreatureGenerator> ().verticalSize > p2.GetComponent<CreatureGenerator> ().verticalSize ? p1.GetComponent<CreatureGenerator> ().verticalSize * 3 : p2.GetComponent<CreatureGenerator> ().verticalSize * 3;
		md = new MatingDance (parent1, parent2, maxCreatureSize);
	}

	//set lights to spawn
	void spawnLight(){
		try{
			l1 = parent1.GetComponent<CreatureGenerator> ().headPrefab.GetComponent<Reproduction> ().babyLight;
			l2 = parent2.GetComponent<CreatureGenerator> ().headPrefab.GetComponent<Reproduction> ().babyLight;
			l1.color = parent1.GetComponent<CreatureGenerator> ().color;
			l2.color = parent2.GetComponent<CreatureGenerator> ().color;

			l1 = Instantiate(l1,eggpos, transform.rotation);
			l2 = Instantiate(l2,eggpos, transform.rotation);
		}
		catch(System.Exception e){ 
			Debug.Log (e.ToString ());
		}
	}

	void resetBehaviours(GameObject creature){
		foreach (Transform child in creature.transform) {
			if (child.name.Contains ("head")) {
				foreach (SteeringBehaviour sb in child.gameObject.GetComponents<SteeringBehaviour> ()) {
					sb.enabled = true;
				}
				child.GetComponent<Reproduction>().enabled = false;

			}
		}
	}

	//return the position the creature needs to seek
	public Vector3 GetSeekPos(GameObject parent){
		if (parent == parent1) {
			return p1seek;
		}
		return p2seek;
	}

	public Vector3 getCentre(){
		return eggpos;
	}
}
