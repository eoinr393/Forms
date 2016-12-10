using UnityEngine;
using System.Collections;

public class RotateMe : MonoBehaviour {
    public float speed = 0.1f;
    Vector3 axis;
	// Use this for initialization
	void Start () {
        axis = Random.insideUnitSphere;
	}   
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axis, speed * Time.deltaTime * 360);
	}
}
