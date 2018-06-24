using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter: MonoBehaviour {
	public enum Item {Shield, Sword, Axe, none};
    public Gradient fighterGradient;
	Rigidbody rb;
	Health health;
	public Item item;
	float startMass, startStrength, startSpeed;

    public float baseAttack;
    public float rageAttack;
	float defenseBuff;
	public bool defending, raging = false;
    [SerializeField]
    int rage;
    public int rageLimit;
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
        baseAttack = pc.attackStrength;
        rageAttack = baseAttack * 2;
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

        rage += 1;
        print("Rage: " + rage);
    }

    public void CheckRage() {
        if (rage >= rageLimit)
        {
            raging = true;
        }
        else
        {
            raging = false;
        }
    }
    public void ResetRage()
    {
        rage = 0;
        CheckRage();
    }
}
