using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour {

	public PlayerController player;
	public Vector3 startDistance;
	public float resetTime;
	public bool resetting;

	// Use this for initialization
	void Start () {
		startDistance = transform.position - player.transform.position;

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (resetting) {
			transform.position = Vector3.Lerp (transform.position, player.transform.position + startDistance, 1 / (resetTime));
		}
	}

}
