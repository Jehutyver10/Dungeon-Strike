using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoldText : MonoBehaviour {

	Text text;
	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (FindObjectOfType<PlayerController> ()) {
			text.text = FindObjectOfType<PlayerController> ().gold.ToString () + "G";
		}
	}
}
