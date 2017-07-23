using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGuide : MonoBehaviour {
	Vector3 mousePos;
	PlayerController player;
	public Color[] colors;
	float length = 3;
	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();		
	}
	
	// Update is called once per frame
	void Update () {
		if (player.playerState != PlayerController.PlayerState.Charging) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition) -  player.transform.position;
			mousePos = Vector3.ClampMagnitude (new Vector3 (mousePos.x, .059f, mousePos.z), length);
			GetComponent<LineRenderer> ().SetPosition (0, player.transform.position);
			GetComponent<LineRenderer> ().SetPosition (1,  player.transform.position + new Vector3(mousePos.x, 0.59f, mousePos.z));
		}
	}
}
