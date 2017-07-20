using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NameText : MonoBehaviour {
	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (FindObjectOfType<PlayerController> ()) {
			if (FindObjectOfType<PlayerController> ().isMyTurn) {
				text.color = Color.white;
			} else {
				text.color = Color.black;

			}
		}
	}
}
