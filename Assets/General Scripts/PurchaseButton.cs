using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PurchaseButton : MonoBehaviour {

	public string itemName;
	public int cost;
	PlayerController player;
	public GameObject itemButton; //item to add to panel

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(player.gold < cost){
			GetComponent<Button> ().interactable = true;

		}
	}

	public void TryToBuy(){
		if (player.gold >= cost) { //if they have enough gold
			player.gold = player.gold - cost;
			player.inventory.Add (itemName);

			for (int i = 0; i < FindObjectOfType<GameManager> ().ButtonList.Count; i++) {
				if (!FindObjectOfType<GameManager> ().ButtonList [i].GetComponent<ItemButton> ()) {
					GameObject newButton = Instantiate (itemButton, FindObjectOfType<GameManager> ().ButtonList [i].transform.parent);
					newButton.transform.position = FindObjectOfType<GameManager> ().ButtonList [i].transform.position;
					Destroy (FindObjectOfType<GameManager> ().ButtonList [i].gameObject);
					FindObjectOfType<GameManager> ().ButtonList [i] = newButton;
					break;
				}
			}
			GetComponent<Button> ().interactable = false;
		}


	}
}
