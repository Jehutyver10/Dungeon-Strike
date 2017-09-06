using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float health = 1000;
	public float maxHealth = 1000;
	Renderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (health > maxHealth) {
			health = maxHealth;
		}
		VisualizeDamage ();
	}

	public void TakeDamage(float damage, bool knockback = false){
		if (GetComponent<Fighter> ()) {
			if (GetComponent<Fighter> ().defending) {
				health -= Mathf.RoundToInt (damage / 2);
			} else {
				health -= Mathf.RoundToInt (damage);
			}
		} else {
			health -= Mathf.RoundToInt (damage);
		}
		if(health <= 0f){
			FindObjectOfType<GameManager> ().characters.Remove (this.GetComponent<Character>());
			if (GetComponent<PlayerController> ()) {
				FindObjectOfType<GameManager> ().characters.Clear();
				FindObjectOfType<GameManager> ().OnPlayerDeath ();
			}
			if (GetComponent<Enemy> ()) {
				if (FindObjectOfType<PlayerController> ()) {
					FindObjectOfType<PlayerController> ().gold++;
				}
				if (	FindObjectOfType<GameManager> ().characters.Count == 1 && 	FindObjectOfType<GameManager> ().characters [0].GetComponent<PlayerController> ()) {//if last enemy
					FindObjectOfType<GameManager> ().OnVictory ();
				}
			}

			Destroy(gameObject);
		}
		if(GetComponent<Animator>()){
			GetComponent<Animator>().SetTrigger("Take Damage");
		}
	}

	public void RestoreHealth(float healthRestored){
		if (health + healthRestored > maxHealth) {
			health = maxHealth; //prevents overflow
		} else {
			health += healthRestored;
		}

	}
	void VisualizeDamage(){
		rend.material.color =  Color.Lerp (Color.red, Color.white, health / maxHealth);
	}
}
