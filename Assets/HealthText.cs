using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthText : MonoBehaviour {

	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
		text.text = "Health: " + FindObjectOfType<PlayerController> ().GetComponent<Health>().health.ToString();

	}
	
	// Update is called once per frame
	void Update () {
		if (FindObjectOfType<PlayerController>()) {
			text.text = "Health: " + FindObjectOfType<PlayerController> ().GetComponent<Health> ().health.ToString ();
		}
	}
}
