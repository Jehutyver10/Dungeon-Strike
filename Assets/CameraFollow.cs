using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public CueController player;
	public Vector3 startDistance;
	GameObject target;
	// Use this for initialization
	void Start () {
		target = player.target;
		startDistance = transform.position - target.transform.position;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CamReset(){
		transform.position = Vector3.Lerp (transform.position, target.transform.position + startDistance, 1/(player.resetTime * 5f));
	}
}
