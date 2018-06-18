﻿using System.Collections;
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
    int rage;
    PlayerController pc;


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
		startSpeed = GetComponent<PlayerController> ().launchForce;
        pc= GetComponent<PlayerController>();
	}

	public void Shield(){
		rb.mass = startMass * 10;
		defending = true;
	}

	public void Sword(){
	    pc.attackStrength = pc.attackStrength * 2;
		pc.launchForce = pc.launchForce * 4;
	
	}
		
	public void ResetStats(){
		rb.mass = startMass;
		defending = false;
		pc.attackStrength = startStrength;
		pc.launchForce = startSpeed;

	}
    public void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            rage += 1;
            print("Rage: " + rage);
        }
    }
}
