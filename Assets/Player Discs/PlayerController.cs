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

    public PlayerClass playerClass;
	public enum ClassName {Fighter, Wizard, Rogue};
	public ClassName myClass;
	public GameObject axePrefab;

	public float rayLength, throwBuffer = .02f, launchForce, velocityThreshold = 0.01f;
    [HideInInspector]
    public float launchStrength;

    private float chargeDuration, chargeRate;
	public int speed, gold;
	public List <string> inventory;

    private void Awake()
    {
        launchStrength = playerClass.strengthBuffer;

    }
    // Vector3 test;
    // Use this for initialization
    new void Start() {
        base.Start();
        //test = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<GameManager>();
        speed = Mathf.RoundToInt(rb.velocity.magnitude);
        cam = FindObjectOfType<CameraFollow>();
        inventory = new List<string>();
        chargeDuration = playerClass.chargeTime;
        chargeRate = playerClass.chargeRate;
        UIManager.main.SetPlayerBar(playerClass.chargeBar);

	}
	
	// Update is called once per frame
	void Update () {
//		rb = GetComponent<Rigidbody> ();
		if(isMyTurn){
		}
		speed = Mathf.RoundToInt(rb.velocity.magnitude);

        //fDebug.DrawRay(transform.position, test);
        if (myClass == ClassName.Fighter)
        {
            if (GetComponent<Fighter>().raging)
            {
                launchStrength = GetComponent<Fighter>().rageLaunchStrength;
            }
            else
            {
                launchStrength = GetComponent<Fighter>().baseLaunchStrength;
            }
        }
    }


    void OnMouseDown(){
		GetStartPosition ();

	}

	public void GetStartPosition(){
		if (myState == CharacterState.Idle && isMyTurn) {
            shotGuide.SetActive(false);
			//print ("starting drag");
			if (!inPlay && isMyTurn) {
				//print ("Player's turn");

				startDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y));
				StartCoroutine (Select());
                ResolveStartOfTurn();
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
		myState = CharacterState.Selected;


	}

    public void ResolveStartOfTurn()
    {
        if(myClass == ClassName.Fighter)
        {
            GetComponent<Fighter>().CheckRage();
        }
    }
	public void GetEndPosition(){
		if (myState == CharacterState.Selected) {
            StartCoroutine(Charge(GetChargeTime()));
            endDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        }
        else if (myState == CharacterState.Charging) {
			if (!inPlay && isMyTurn) {//actually executing the attack

                selector.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", Color.black);
				selector.SetActive (false);
				shotGuide.SetActive (false);

                if (usedItem == "Axe") {			
					Throw ();
				} else {
					Launch ();
				}
				myState = CharacterState.Launched;
                attackStrength = GetAttackStrength();
                

            }
        }
	}

    float GetChargeTime()
    {
        if (myClass == ClassName.Fighter)
        {
            if (GetComponent<Fighter>().raging)
            {
                return 5f;
            }
        }
        return 3f;
    }
	IEnumerator Charge(float chargeTime = 3){
        float elapsedTime = 0;
        float currentCharge = 0;
		yield return new WaitForSeconds (.1f);
		myState = CharacterState.Charging;
		//print ("charging up");
		Material mat = shotGuide.GetComponent<Renderer> ().material;
		//mat.SetColor ("_EmissionColor", Color.black);
		while (myState == CharacterState.Charging){
            currentCharge = Mathf.PingPong(elapsedTime / chargeTime, 1);
            UIManager.main.SetBarFill(currentCharge);
            AdjustChargeMeter(currentCharge);
            elapsedTime += Time.deltaTime * chargeRate;
            //mat.SetColor("_EmissionColor", Color.Lerp (shotGuide.GetComponent<ShotGuide> ().colors [0], shotGuide.GetComponent<ShotGuide> ().colors [1], Mathf.PingPong (Time.time / chargeTime, 1)));
            launchForce = Mathf.PingPong (elapsedTime/chargeTime, 1) * launchStrength;
            
            yield return null;

        }

        mat.SetColor ("_EmissionColor", Color.white);

		//over a short period of time, lerp the color from start color to red to white then back again
		//if the player clicks before it reaches the start color again, retur
	}

    void AdjustChargeMeter(float charge)
    {
        if(myClass == ClassName.Fighter)
        {
            if (!GetComponent<Fighter>().raging)
            {
                charge = charge * .67f;
            }
            UIManager.main.SetChargeMeterColor(GetComponent<Fighter>().fighterGradient.Evaluate(charge));
        }

      
    }
	void Launch(float strength = 1){
		if (usedItem == "Sword") {
			GetComponent<Fighter> ().Sword ();
		}

        Vector3 magnitude = new Vector3 (endDrag.x - transform.position.x, 0, endDrag.z - transform.position.z);
		this.rb.AddForce(magnitude.normalized * launchForce, ForceMode.Impulse);
		inPlay = true;
		StartCoroutine (EndTurn());

	}

	void Throw(){
		Vector3 magnitude = new Vector3 (endDrag.x -startDrag.x, 0, endDrag.y-startDrag.y);
		GameObject axe = Instantiate (axePrefab);
		axe.transform.position = transform.position;
		axe.transform.parent = transform;
	
		axe.GetComponent<Rigidbody> ().AddForce (magnitude.normalized * throwBuffer, ForceMode.Impulse);
	
		StartCoroutine (EndTurn());

	}

	public override IEnumerator EndTurn(){
        float elapsedTime = 0;
        //ActivateItems();
        StartCoroutine(gm.EndTurn(GetComponent<Character>(), 2));

        while (elapsedTime < .5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        myState = CharacterState.Launched;
        attackStrength = GetAttackStrength();
        while (myState != CharacterState.Off) {
            if (System.Math.Round(rb.velocity.magnitude, 4) < velocityThreshold)
            {
                myState = CharacterState.Off;
                ResolveEndOfTurn();
            }
            yield return null;
		}
		inPlay = false;

		yield return new WaitForSeconds (cam.resetTime /2f);



    }

    void ResolveEndOfTurn()
    {
        if(myClass == ClassName.Fighter)
        {
            if (GetComponent<Fighter>().raging)
            {
                GetComponent<Fighter>().ResetRage();
            }
        }
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
    //

    float GetAttackStrength()
    {
        if (myState == CharacterState.Launched)
        {
            if (myClass == ClassName.Fighter)
            {
                if (GetComponent<Fighter>().raging)
                {
                    return GetComponent<Fighter>().rageAttack;
                }
                return GetComponent<Fighter>().baseAttack;
            }
        }
        return attackStrength;
    }



    //	void ShotGuide(){
    //		///raycast from character
    //		/// for each collision, render a line between the origin or pivot point of the ray and the collision point + the end 
    //	
    //	}
}
