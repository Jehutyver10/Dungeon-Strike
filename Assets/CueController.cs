using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueController : MonoBehaviour {
	float yMove = 0, xMove = 0, xOrbit = 0;
	Vector3 startDistance;
	Rigidbody rb;
	bool orbiting = true, aiming = false, shooting = false, resetting = false, ballHit = false, ballStoppedMoving = false;

	public InstructionText instructionText;
	public CameraFollow cam;
	public float orbitSpeed, strength, resetTime = 2;
	public GameObject target;
	// Use this for initialization
	void Start () {
		startDistance = transform.position - target.transform.position;
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		yMove = Input.GetAxis ("Mouse X");
		xMove = Input.GetAxis ("Mouse Y");
		//these axes are purposefully switched to suit the axes they are added to

		xOrbit = Input.GetAxis ("Horizontal");
		if (resetting) {
			transform.position = Vector3.Lerp (transform.position, target.transform.position + startDistance, resetTime * Time.deltaTime);
			cam.CamReset ();

		}
		FullReset ();


	}

	void FixedUpdate(){

		if (orbiting) {
			instructionText.text.text = instructionText.orbitText;
			Orbit ();
		}
		if (aiming) {
			Aim ();
			instructionText.text.text = instructionText.aimingText;
		}
		if (shooting) {
			Shoot ();
			instructionText.text.text = instructionText.shootingText;
		}
		AdjustStep ();

	}

	void Orbit(){
		transform.RotateAround (target.transform.position, Vector3.up, xOrbit);
		transform.LookAt (target.transform);
	}


	void AdjustStep(){//handles whether the player is orbiting, aiming, or shooting.
		if(ballHit){
			instructionText.text.text = instructionText.resettingText;
			if(Mathf.Round(target.GetComponent<Rigidbody>().velocity.magnitude) == 0){
				ballStoppedMoving = true;
				ballHit = false;


			}
		}
		if (ballStoppedMoving) {
			resetting = true;
			ballStoppedMoving = false;
			Invoke ("Reset", resetTime);
		}
		if(orbiting){
			if (Input.GetMouseButtonDown(0)) {
				orbiting = false;
				aiming = true;
				resetting = false;
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
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}
		}

		else if (shooting) {
			if (Input.GetMouseButtonDown (1)) {
				shooting = false;
				aiming = true;
				rb.isKinematic = true;
			}
		}


	}

	void Aim(){
		rb.MoveRotation (Quaternion.Euler (new Vector3 (transform.eulerAngles.x - xMove, transform.eulerAngles.y + yMove, transform.eulerAngles.z)));
	}

	void Shoot(){
		rb.AddForce (transform.forward * xMove * strength);
	}

	void Reset(){
		orbiting = true;
		rb.isKinematic = true;
		resetting = false;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Ball> ()) {
			shooting = false;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			ballHit = true;
			shooting = false;
		}
	}

	void FullReset(){//TODO: get rid of this, for development purposes only.
		if(Input.GetKeyDown(KeyCode.Space)){
			print("Resetting...");
			target.transform.position = target.GetComponent<Ball> ().startPosition;
			transform.position = target.transform.position + startDistance;
			orbiting = true;
			cam.transform.position = target.transform.position + cam.startDistance;
			ballHit = false;
			ballStoppedMoving = false;
			aiming = false;
			shooting = false;
			resetting = false;
			rb.isKinematic = true;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;

		}
	}
}
