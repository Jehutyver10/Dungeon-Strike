using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class PlayerController : Character {
	private Vector3 startDrag, endDrag;
	Rigidbody rb;
	GameManager gm;
	CameraFollow cam;

	public float strengthBuffer, rayLength;
	public int speed;	



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		gm = FindObjectOfType<GameManager> ();
		speed = Mathf.RoundToInt(rb.velocity.magnitude);
		cam = FindObjectOfType<CameraFollow> ();



	}
	
	// Update is called once per frame
	void Update () {
//		rb = GetComponent<Rigidbody> ();
		if(isMyTurn){
		}
		speed = Mathf.RoundToInt(rb.velocity.magnitude);


	}

	public void StartDrag(){
		if (!inPlay && isMyTurn) {
			print ("Player's turn");

			cam.resetting = false;
			startDrag = Input.mousePosition;
		}

	}

	public void EndDrag(){
		if (!inPlay && isMyTurn) {
			endDrag = Input.mousePosition;
			Launch ();
		}

	}
	void Launch(){
		float magnitude = Vector3.Magnitude (endDrag - startDrag);
		this.rb.AddForce(cam.transform.forward *magnitude * strengthBuffer, ForceMode.Impulse);
		inPlay = true;
		StartCoroutine ("LaunchDelay");

	}

	IEnumerator LaunchDelay(){
		float timeSinceLaunch = Time.time;
		while (Time.time - timeSinceLaunch < 3) {
			yield return null;
		}
		inPlay = false;
		cam.resetting = true;

		yield return new WaitForSeconds (cam.resetTime /2f);

		gm.EndTurn ();

	}
	//



//	void ShotGuide(){
//		///raycast from character
//		/// for each collision, render a line between the origin or pivot point of the ray and the collision point + the end 
//	
//	}
}
