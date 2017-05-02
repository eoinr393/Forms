using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

public class CreatureReproduction  {

	public GameObject Create(GameObject p1, GameObject p2, bool allowMutation = true, float mutationChance = 0.05f){
		CreatureGenerator c1 = p1.GetComponent<CreatureGenerator> ();
		CreatureGenerator c2 = p2.GetComponent<CreatureGenerator> ();

		GameObject babyCreature = new GameObject ();
		CreatureGenerator c3 = babyCreature.AddComponent<CreatureGenerator>();

		//get the publicly accessible variables
		FieldInfo[] fi = typeof(CreatureGenerator).GetFields (BindingFlags.Public | BindingFlags.Instance);

		foreach(FieldInfo f in fi){

			//Debug.Log (f.Name + " Type= " + f.GetValue(c1).GetType().ToString());

			//randomly select certain attibutes from either parent and put into child,
			//chance for mutation of certain variables
			int sel = Random.Range (0, 2);
			//Debug.Log (sel);
			//set values to either parent1 or parent2 values at random
			if (sel == 1) {
				typeof(CreatureGenerator).GetField (f.Name).SetValue (c3, typeof(CreatureGenerator).GetField (f.Name).GetValue (c1));
			} else {
				typeof(CreatureGenerator).GetField (f.Name).SetValue (c3, typeof(CreatureGenerator).GetField (f.Name).GetValue (c2));
			}
		}


		//set the name to longest parent name, or a combination of both
		if (p1.name.Length < p2.name.Length && p2.name.Contains (p1.name)) {
			babyCreature.name = p2.name;
		} else if (p2.name.Length < p1.name.Length && p1.name.Contains (p2.name)) {
			babyCreature.name = p1.name;
		} else {
			//indexes of the capital letters
			int p1index = (from ch in p1.name.ToCharArray ()
			                  where char.IsUpper (ch)
								select p1.name.IndexOf (ch)).First();

			int p2index = (from ch in p2.name.ToCharArray ()
							where char.IsUpper (ch)
							select p2.name.IndexOf (ch)).First();

			//if caps in name else just combine name
			if (p1index > 0 && p2index > 0)
				babyCreature.name = p1.name.Substring (0, p1index) + p2.name.Substring (0, p2index);
			else
				babyCreature.name = p1.name + p2.name;
		}

		//mutate the creature
		if (allowMutation) {
			foreach (FieldInfo f in fi) {
				float chance = Random.Range (0, 101) * 0.01f;

				//if the chance is met and the variable is a number, then edit number
				if (chance < mutationChance && (f.GetValue (c3).GetType ().ToString () == "System.Single" || f.GetValue (c3).GetType ().ToString () == "System.Int32")) {
					//if its greater than one increment or decrement by random percentage of the current value;

					if ((float)f.GetValue (c3) > 0) {
						float percent = Random.Range (-150, 150) * 0.01f;
						float oldVal = (float)typeof(CreatureGenerator).GetField (f.Name).GetValue (c3);
						float newVal =Mathf.Clamp( oldVal + oldVal * percent,0.1f,float.MaxValue);
						typeof(CreatureGenerator).GetField (f.Name).SetValue (c3, newVal);
						//Debug.Log ("mutated " + f.Name + " from " + oldVal.ToString () + " to " + newVal);
					}


				}
			}
		}

		//turn of reproduction and turn on movement behaviours

		return babyCreature;
	}
		
}
