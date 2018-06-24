using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float attackStrength;
    public float lifetime;
    public Character owner;
	public List<AudioClip> audioClips;
	AudioSource audioSource;
    bool hitSomething = false;
	// Use this for initialization


	void Start () {
		audioSource = GetComponent<AudioSource> ();
		Physics.IgnoreCollision (GetComponent<Collider> (), transform.parent.GetComponent<Collider> ());
		StartCoroutine (SelfDestruct());
		audioSource.Play ();
		audioSource.time = .1f;
        hitSomething = false;
        StartCoroutine(EnableSelfInjury());
	}
	
    IEnumerator EnableSelfInjury(float totalTime = 0.5f)
    {
        float elapsedTime = 0;
        //interrupt the coroutine in the case of collision
        while (elapsedTime < totalTime && !hitSomething)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Physics.IgnoreCollision(GetComponent<Collider>(), owner.GetComponent<Collider>(), false);
    }
	// Update is called once per frame

	IEnumerator SelfDestruct(){
        yield return new WaitForSeconds(3);
        StartCoroutine(owner.EndTurn());
		Destroy (gameObject);
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Health> ()) {
			audioSource.clip = audioClips [1];
			audioSource.Play ();
			audioSource.time = .2f;

			col.gameObject.GetComponent<Health> ().TakeDamage (attackStrength);
		}
        hitSomething = true;
	}
}
