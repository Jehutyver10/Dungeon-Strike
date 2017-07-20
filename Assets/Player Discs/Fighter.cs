using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter: MonoBehaviour {
	public enum Item {Shield, Sword, Axe, none};
	Rigidbody rb;
	Health health;
	public Item item;
	float startMass, startStrength, startSpeed;
	float defenseBuff;
	public bool defending;


	// Use this for initialization
	void Start () {
		item = Item.none;
		rb = GetComponent<Rigidbody> ();
		rb.mass = 1.5f * rb.mass;
		startMass = rb.mass;
		health = GetComponent<Health> ();
		health.maxHealth = 150;
		health.health = 150;
		startStrength = GetComponent<PlayerController> ().attackStrength;
		startSpeed = GetComponent<PlayerController> ().strengthBuffer;

	}

	public void Shield(){
		rb.mass = startMass * 10;
		defending = true;
	}

	public void Sword(){
		GetComponent<PlayerController> ().attackStrength = GetComponent<PlayerController> ().attackStrength * 2;
		GetComponent<PlayerController> ().strengthBuffer = GetComponent<PlayerController> ().strengthBuffer * 4;
	
	}
		
	public void ResetStats(){
		rb.mass = startMass;
		defending = false;
		GetComponent<PlayerController> ().attackStrength = startStrength;
		GetComponent<PlayerController> ().strengthBuffer = startSpeed;

	}


	// Update is called once per frame
	void Update () {
		
	}
}
