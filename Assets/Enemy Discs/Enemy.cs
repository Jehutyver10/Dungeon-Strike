using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : Character {
	
	GameManager gm;
	Rigidbody rb;
    PlayerController player;
	public float velocityThreshold = 3;
	public float strength = 3, throwStrength;
	public enum EnemyClass{basic, Standard, Minion, Boss};
	public EnemyClass enemyClass;
	public GameObject projectilePrefab;
    Vector3 targetPosition;
    // Use this for initialization


    private new void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.GetComponent<Spikes>())
        {
            health.TakeDamage(999);
        }

    }
    void Awake(){
		rb = GetComponent<Rigidbody> ();
		DontDestroyOnLoad (this.gameObject);


	}


	new void Start () {
        base.Start();
		gm = FindObjectOfType<GameManager> ();
		gm.characters.Add (this);
        print("enemy start script executes");
        player = FindObjectOfType<PlayerController>();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isMyTurn && myState != CharacterState.Launched) {
			if (enemyClass == EnemyClass.Minion) {
				Throw ();
			} else if (enemyClass == EnemyClass.Boss) {
				Spit ();
			} else {
				Attack ();
			}
		}

        //target the player at the start of the player's turn;
        if(player.isMyTurn && player.myState == CharacterState.Idle)
        {
            targetPosition = player.transform.position;
        }
	}

	void Spit(){
		float fireZ = 4.5f;
		for (int i = 0; i < 3; i++) {
			print ("Spitting");
			GameObject fireball = Instantiate (projectilePrefab);
			fireball.transform.position = transform.position;
			fireball.transform.parent = transform;
			Vector3 magnitude = new Vector3 (-9, 0, fireZ) - transform.position;
			fireball.GetComponent<Rigidbody> ().AddForce (magnitude * throwStrength, ForceMode.Impulse);
			fireZ -= 4.5f;
		}

        myState = CharacterState.Launched;
        StartCoroutine (EndTurn());

	}
	void Throw(){
		GameObject spear = Instantiate (projectilePrefab);
		spear.transform.position = transform.position;
		spear.transform.parent = transform;
		Vector3 magnitude = targetPosition - transform.position;
		spear.GetComponent<Rigidbody> ().AddForce (magnitude * throwStrength, ForceMode.Impulse);
        spear.GetComponent<Projectile>().lifetime = 3;
        spear.GetComponent<Projectile>().owner = this;
        myState = CharacterState.Launched;

        StartCoroutine(EndTurn());

    }

    void Attack(){

        Vector3 targetDir = FindObjectOfType <PlayerController> ().transform.position - transform.position;
		rb.AddForce (targetDir * strength, ForceMode.Impulse);
        myState = CharacterState.Launched;
        StartCoroutine(EndTurn());
	}

	public override IEnumerator EndTurn(){// waits a bit before ending the turn to allow damage calculation to happen
        float elapsedTime = 0;
        StartCoroutine(gm.EndTurn(GetComponent<Character>()));

        while (elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        while (myState != CharacterState.Off)
        {
            if (System.Math.Round(rb.velocity.magnitude, 4) < velocityThreshold)
            {
                myState = CharacterState.Off;
            }
            yield return null;
        }
	}




}
