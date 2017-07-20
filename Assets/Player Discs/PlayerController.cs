using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Health))]
public class PlayerController : Character {
	private Vector3 startDrag, endDrag;
	Rigidbody rb;
	GameManager gm;
	CameraFollow cam;
	public string usedItem;
	public enum PlayerClass {Fighter, Wizard, Rogue};
	public PlayerClass playerClass;
	public GameObject axePrefab;

	public float strengthBuffer = .01f, rayLength, attackStrength = 1, throwBuffer = .02f;
	public int speed, gold;
	public List <string> inventory;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		gm = FindObjectOfType<GameManager> ();
		speed = Mathf.RoundToInt(rb.velocity.magnitude);
		cam = FindObjectOfType<CameraFollow> ();
		inventory = new List<string> ();


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

			startDrag = Input.mousePosition;
		}

	}

	public void EndDrag(){
		if (!inPlay && isMyTurn) {
			endDrag = Input.mousePosition;
			if (usedItem == "Axe") {
				Throw ();
			} else {
				Launch ();
			}
		}

	}
	void Launch(){
		if (usedItem == "Sword") {
			GetComponent<Fighter> ().Sword ();
		}
		Vector3 magnitude = new Vector3 (endDrag.x - startDrag.x, 0, endDrag.y-startDrag.y);
		this.rb.AddForce(magnitude * strengthBuffer, ForceMode.Impulse);
		inPlay = true;
		StartCoroutine ("LaunchDelay");

	}

	void Throw(){
		Vector3 magnitude = new Vector3 (endDrag.x - startDrag.x, 0, endDrag.y-startDrag.y);
		GameObject axe = Instantiate (axePrefab);
		axe.transform.position = transform.position;
		axe.transform.parent = transform;
	
		axe.GetComponent<Rigidbody> ().AddForce (magnitude * throwBuffer, ForceMode.Impulse);
		StartCoroutine ("LaunchDelay");

	}

	IEnumerator LaunchDelay(){
		float timeSinceLaunch = Time.time;
		while (Time.time - timeSinceLaunch < 3) {
			yield return null;
		}
		inPlay = false;

		yield return new WaitForSeconds (cam.resetTime /2f);
		ActivateItems ();

		gm.EndTurn ();

	}

	public void UseItem(string itemName){ //readies item to be used
		if (itemName == "Shield") {
			GetComponent<Fighter> ().item = Fighter.Item.Shield;

		} else if (itemName == "Axe") {
			GetComponent<Fighter> ().item = Fighter.Item.Axe;
		} else if (itemName == "Sword") {
			GetComponent<Fighter> ().item = Fighter.Item.Axe;
		}
		usedItem = itemName;
		
	}

	public void RemoveItem(string itemName){
		if (GetComponent<Fighter> ()) {
			GetComponent<Fighter> ().item = Fighter.Item.none;
		}
	}
	public void ActivateItems(){
		//use the item 
		if (GetComponent<Fighter> () && GetComponent<Fighter>().item == Fighter.Item.Sword) {
			GetComponent<Fighter> ().Invoke (usedItem, 0);
		}
		inventory.Remove (usedItem);
		usedItem = null;

		//revert the button to normal
		gm.AdjustButtons();

	}

	public void ClearItem(){
		if(GetComponent<Fighter>()){
			GetComponent<Fighter>().ResetStats();
		}
	}
	void OnCollisionEnter(Collision col){
		if (isMyTurn) {
			if (col.gameObject.GetComponent<Enemy> ()) {
				GetComponent<AudioSource> ().Play ();
				GetComponent<AudioSource> ().time = .25f;
				col.gameObject.GetComponent<Health> ().TakeDamage (col.impulse.magnitude * attackStrength);
			}
		}
	}
	//



//	void ShotGuide(){
//		///raycast from character
//		/// for each collision, render a line between the origin or pivot point of the ray and the collision point + the end 
//	
//	}
}
