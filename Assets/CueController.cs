using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueController : MonoBehaviour {
	float yMove = 0, zMove = 0, xOrbit = 0;
	Rigidbody rb;

	public bool orbiting = true, aiming = false, shooting = false;
	public float orbitSpeed;
	public GameObject target;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		yMove = Input.GetAxis ("Mouse X");
		zMove = Input.GetAxis ("Mouse Y");
		//these axes are purposefully switched for better gamefeel

		xOrbit = Input.GetAxis ("Horizontal");


		if (orbiting) {
			Orbit ();
		}
		if (aiming) {
			Aim ();
		}
		if (shooting) {
			Shoot ();
		}
		AdjustStep ();

	}

	void Orbit(){
		transform.RotateAround (target.transform.position, Vector3.up, xOrbit);
	}


	void AdjustStep(){//handles whether the player is orbiting, aiming, or shooting.
		if(orbiting){
			if (Input.GetMouseButtonDown(0)) {
				orbiting = false;
				aiming = true;
			}
		}
		else if (aiming) {
			if (Input.GetMouseButtonDown(1)) {
				aiming = false;
				orbiting = true;
			} 
			else if (Input.GetMouseButtonDown (0)) {
				aiming = false;
				shooting = true;
				rb.isKinematic = false;
			}
		}

		else if (shooting) {
			if (Input.GetMouseButtonDown (1)) {
				shooting = false;
				orbiting = true;
				rb.isKinematic = true;
			}
		}


	}

	void Aim(){
		rb.MoveRotation (Quaternion.Euler (new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y + yMove, transform.eulerAngles.z + zMove)));
	}

	void Shoot(){
		rb.MovePosition (transform.InverseTransformPoint(new Vector3 (transform.position.x + zMove, transform.position.y, transform.position.z)));
	}

}
