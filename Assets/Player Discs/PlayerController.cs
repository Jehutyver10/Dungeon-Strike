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
	public GameObject selector, shotGuide;

    public PlayerClass myClass;
	public enum ClassName {Fighter, Wizard, Rogue};
	public enum PlayerState {Unselected, Selected, Charging};
	public ClassName playerClass;
	public PlayerState playerState;
	public GameObject axePrefab;

	public float rayLength, attackStrength = 1, throwBuffer = .02f, launchStrength = 50, launchForce;
	private float chargeDuration;
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

	void OnMouseDown(){
		GetStartPosition ();

	}

	public void GetStartPosition(){
		if (playerState == PlayerState.Unselected && isMyTurn) {
			//print ("starting drag");
			if (!inPlay && isMyTurn) {
				//print ("Player's turn");

				startDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y));
				StartCoroutine ("Select");

			}
		}
	}

	IEnumerator Select(){
		selector.SetActive (true);
		shotGuide.SetActive (true);
		Material mat = selector.GetComponent<Renderer> ().material;
		float colorAdjustment = 0;
		while (colorAdjustment <= 1f) {
			mat.SetColor ("_EmissionColor", Color.white * colorAdjustment);
			colorAdjustment = colorAdjustment + Time.deltaTime *10;
			yield return null;
		}
		yield return new WaitForSeconds (.1f);
		playerState = PlayerState.Selected;


	}

	public void GetEndPosition(){
		if (playerState == PlayerState.Selected) {
			StartCoroutine (Charge());


		}
		else if (playerState == PlayerState.Charging) {

			if (!inPlay && isMyTurn) {//actually executing the attack
				selector.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
				selector.SetActive (false);
				shotGuide.SetActive (false);

				if (usedItem == "Axe") {			
					Throw ();
				} else {
					Launch ();
				}
				playerState = PlayerState.Unselected;
			}
		}
	}

	IEnumerator Charge(float chargeTime = 3){
		yield return new WaitForSeconds (.1f);
		playerState = PlayerState.Charging;
		//print ("charging up");
		Material mat = shotGuide.GetComponent<Renderer> ().material;
		mat.SetColor ("_EmissionColor", Color.black);
		while (playerState == PlayerState.Charging){
			mat.SetColor("_EmissionColor", Color.Lerp (shotGuide.GetComponent<ShotGuide> ().colors [0], shotGuide.GetComponent<ShotGuide> ().colors [1], Mathf.PingPong (Time.time / chargeTime, 1)));
			launchForce = Mathf.PingPong (Time.time/chargeTime, 1) * launchStrength;
			yield return null;
			endDrag =Camera.main.ScreenToWorldPoint(Input.mousePosition);

		}
		mat.SetColor ("_EmissionColor", Color.white);

		//over a short period of time, lerp the color from start color to red to white then back again
		//if the player clicks before it reaches the start color again, retur
	}
	void Launch(float strength = 1){
		if (usedItem == "Sword") {
			GetComponent<Fighter> ().Sword ();
		}

		Vector3 magnitude = new Vector3 (endDrag.x - transform.position.x, 0, endDrag.z - transform.position.z);
		this.rb.AddForce(magnitude.normalized * launchForce, ForceMode.Impulse);
		inPlay = true;
		StartCoroutine ("LaunchDelay");

	}

	void Throw(){
		Vector3 magnitude = new Vector3 (endDrag.x -startDrag.x, 0, endDrag.y-startDrag.y);
		GameObject axe = Instantiate (axePrefab);
		axe.transform.position = transform.position;
		axe.transform.parent = transform;
	
		axe.GetComponent<Rigidbody> ().AddForce (magnitude.normalized * throwBuffer, ForceMode.Impulse);
	
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
