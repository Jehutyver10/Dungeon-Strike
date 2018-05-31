using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGuide : MonoBehaviour {
	Vector3 mousePos;
	PlayerController player;
	LineRenderer lr;
	int collisionCounter; 

	public Color[] colors;
	public float length = 3;
	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer> ();
		player = FindObjectOfType<PlayerController> ();	

	}
	
	// Update is called once per frame
	void Update () {
		if (player.myState != PlayerController.CharacterState.Charging) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos = Vector3.ClampMagnitude (new Vector3 (mousePos.x, 0, mousePos.z), length*3) ;
			//print (mousePos);
			CalculateRicochet (mousePos);
			//lr.SetPosition (0, player.transform.position);
			//lr.SetPosition (1,  player.transform.position + new Vector3(mousePos.x, 0.59f, mousePos.z));
		}
	}

	void CalculateRicochet(Vector3 mousePos){
		float remainingLength = length;
		int bounceCounter = 0;


		Ray shotRay = new Ray (player.transform.position, mousePos - transform.position);
		RaycastHit hit;
		Ray outRay;

		lr.positionCount = 2;

		//
		while (remainingLength > 0) {
			if (Deflect (shotRay, out outRay, out hit) && Vector3.Distance (hit.point, shotRay.origin) <= remainingLength) {
				lr.positionCount++;
				lr.SetPosition (bounceCounter, shotRay.origin);
				lr.SetPosition (bounceCounter+1, hit.point);
				remainingLength = remainingLength - Vector3.Distance (lr.GetPosition (bounceCounter), lr.GetPosition (bounceCounter + 1));
				bounceCounter++;
				shotRay = outRay;


				//lr.SetPosition (2, outRay.origin + outRay.direction * 3);
			} else {
				lr.SetPosition (bounceCounter, shotRay.origin);
				lr.SetPosition (bounceCounter + 1, lr.GetPosition(bounceCounter) + new Vector3 (shotRay.direction.x , player.transform.position.y, shotRay.direction.z ) * remainingLength);
				break;
		
			}
		}
		//raycast from origin out
		//if it hits, increase number of points, calculate the angle of reflection and raycast again
		//repeat until line is as long as length 


	}

	bool Deflect(Ray ray, out Ray deflected, out RaycastHit hit){
		int layerMask = 1 << 8;
		layerMask = ~layerMask;
		if (Physics.Raycast (ray, out hit, length, layerMask)) {
			Vector3 normal = hit.normal;
			Vector3 deflect = Vector3.Reflect (ray.direction, normal);
			deflected = new Ray (hit.point, deflect);

			return true;
		} else {

			deflected = new Ray (Vector3.zero, Vector3.zero);
			return false;
		}
	}
}
