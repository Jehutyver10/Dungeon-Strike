using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float health = 1000;
	public float maxHealth = 1000;
    public Character c;
	Renderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
        c = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		if (health > maxHealth) {
			health = maxHealth;
		}
		VisualizeDamage ();
	}

	public void TakeDamage(float damage, bool knockback = false){
        //find out how much damage is being taken;
        float totalDamage = 0;
        totalDamage = Mathf.RoundToInt(damage);
        //cut damage in half if defending
        if (GetComponent<Fighter> ()) {
			if (GetComponent<Fighter> ().defending) {
				totalDamage = Mathf.RoundToInt (damage / 2);
			}
		}
        Coroutine reduction = StartCoroutine(ReduceHealth(totalDamage));
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
    IEnumerator ReduceHealth(float totalDamage, float speed = 20)
    {
        
        float targetHealth = health - totalDamage;
        while (health > targetHealth)
        {
            health = Mathf.Clamp(health - Time.deltaTime * speed, targetHealth, maxHealth);
            if (c)
            {
                if (c.healthBarFill)
                {
                    print("reducing healthbar");
                    c.healthBarFill.fillAmount = health / maxHealth;
                }
            }

            yield return null;
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
