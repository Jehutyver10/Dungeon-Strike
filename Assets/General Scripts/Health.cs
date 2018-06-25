using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float health = 1000;
	public float maxHealth = 1000;
    [SerializeField]
    private float healthBarReductionSpeed;
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

        if(health <= 0)
        {
            GameManager.main.characters.Remove(GetComponent<Character>());
            Destroy(c.healthBarFill);
            Destroy(c.healthBar);
            Destroy(gameObject);
        }

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
        if (totalDamage > health)
        {
            Coroutine reduction = StartCoroutine(ReduceHealth(totalDamage, 200));
        }
        else
        {
            Coroutine reduction = StartCoroutine(ReduceHealth(totalDamage));
        }
        if (GetComponent<Animator>()){
			GetComponent<Animator>().SetTrigger("Take Damage");
		}
	}
    IEnumerator ReduceHealth(float totalDamage, float speed = 20)
    {

        if (c.healthBarFill)
        {
            c.healthBarFill.gameObject.SetActive(true);
        }
        float targetHealth = health - totalDamage;

        while (health > targetHealth)
        {
            health = Mathf.Clamp(health - Time.deltaTime * speed, targetHealth, maxHealth);
            if (c)
            {
                if (c.healthBarFill)
                {
                    //TODO: add easing to the reduction speed
                   
                    c.healthBarFill.fillAmount = health / maxHealth;
                    c.AdjustHealthBarPosition();
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(2);
        if (c.healthBarFill) {
            c.healthBarFill.gameObject.SetActive(false);
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
