using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : Character {
	
	GameManager gm;
	Rigidbody rb;
	float attackDuration = 3;
	public float attackStrength = 1, strength = 3, throwStrength;
	public enum EnemyClass{Minion, Standard, Elite, Boss};
	public EnemyClass enemyClass;
	public GameObject projectilePrefab;
	public bool launched = false;
	// Use this for initialization

	void Awake(){
		rb = GetComponent<Rigidbody> ();
		DontDestroyOnLoad (this.gameObject);


	}


	void Start () {
		gm = FindObjectOfType<GameManager> ();
		gm.characters.Add (this);

		
	}
	
	// Update is called once per frame
	void Update () {
		if (isMyTurn && !launched) {
			if (enemyClass == EnemyClass.Elite) {
				Throw ();
			} else if (enemyClass == EnemyClass.Boss) {
				Spit ();
			} else {
				Attack ();
			}
		}
	}

	void Spit(){
		float fireZ = 4.5f;
		for (int i = 0; i < 3; i++) {
			print ("Spitting");
			GameObject fireball = Instantiate (projectilePrefab);
			fireball.transform.position = transform.position;
			fireball.transform.parent = transform;
			Vector3 magnitude = new Vector3 (-9, 0, fireZ) - transform.position;
			fireball.GetComponent<Rigidbody> ().AddForce (magnitude * throwStrength, ForceMode.Impulse);
			fireZ -= 4.5f;
		}

		launched = true;
		StartCoroutine ("EndTurn");

	}
	void Throw(){
		GameObject spear = Instantiate (projectilePrefab);
		spear.transform.position = transform.position;
		spear.transform.parent = transform;
		Vector3 magnitude = FindObjectOfType <PlayerController> ().transform.position - transform.position;
		spear.GetComponent<Rigidbody> ().AddForce (magnitude * throwStrength, ForceMode.Impulse);
		launched = true;
		StartCoroutine ("EndTurn");

	}

	void Attack(){
		Vector3 targetDir = FindObjectOfType <PlayerController> ().transform.position - transform.position;
		rb.AddForce (targetDir * strength, ForceMode.Impulse);
		launched = true;
		StartCoroutine ("EndTurn");
	}

	IEnumerator EndTurn(){// waits a bit before ending the turn to allow damage calculation to happen
		yield return new WaitForSeconds (attackDuration);
		gm.EndTurn ();
	}

	void OnCollisionEnter(Collision col){
		if (isMyTurn) {
			GetComponent<AudioSource> ().Play ();
			GetComponent<AudioSource> ().time = .25f;
			if (col.gameObject.GetComponent<PlayerController> ()) {
				col.gameObject.GetComponent<PlayerController>().GetComponent<Health> ().TakeDamage (col.impulse.magnitude * attackStrength);

			}
		}
	}



}
