using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

	public float damage = 15;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<PlayerController> ()) {
			col.gameObject.GetComponent<Health> ().TakeDamage (col.relativeVelocity.magnitude);
			print (damage);
		}
	}
}
