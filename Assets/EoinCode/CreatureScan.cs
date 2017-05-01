using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureScan{

	//list of creatures not including the object that called the scan
	public List<GameObject> ScanOther(Vector3 pos, float radius, GameObject self){
		List<GameObject> retlist = Scan (pos, radius);
		retlist.RemoveAll (i => i.name == self.name);

		return  retlist;
	}

	//list of all creatures in sphere at a point
	public List<GameObject> Scan(Vector3 pos, float radius){
		
		List<GameObject> creaturesFound = new List<GameObject> ();
		Collider[] colliders = Physics.OverlapSphere(pos, radius);

		foreach (Collider c in colliders) {
			if (c.transform.parent != null) {
				GameObject parent = c.transform.parent.gameObject;

				if (parent.GetComponent<CreatureGenerator> () != null) {
					creaturesFound.Add (parent);
				}
			}
		}
		return creaturesFound;
	}

}
