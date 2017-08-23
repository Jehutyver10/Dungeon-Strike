using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemButton : MonoBehaviour {

	public GameObject prefab;

	// Use this for initialization
	void Start () {
		GetComponent<Toggle> ().group = FindObjectOfType<ToggleGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UseItem(){
		if(FindObjectOfType<PlayerController>().isMyTurn){
			if (GetComponent<Toggle> ().isOn) {

				FindObjectOfType<PlayerController> ().UseItem (prefab.name);
			} 
			else {//to deselect the item
				FindObjectOfType<PlayerController> ().RemoveItem (prefab.name);
			}
		}
	}
}
