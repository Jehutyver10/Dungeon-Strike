using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float attackStrength;
	public List<AudioClip> audioClips;
	AudioSource audioSource;
	// Use this for initialization


	void Start () {
		audioSource = GetComponent<AudioSource> ();
		Physics.IgnoreCollision (GetComponent<Collider> (), transform.parent.GetComponent<Collider> ());
		StartCoroutine ("SelfDestruct");
		audioSource.Play ();
		audioSource.time = .1f;


	}
	
	// Update is called once per frame
	void Update () {
	}
	IEnumerator SelfDestruct(){
		yield return new WaitForSeconds (3);
		Destroy (gameObject);
	}
	void OnCollisionEnter(Collision col){
		if (col.gameObject.GetComponent<Health> ()) {
			audioSource.clip = audioClips [1];
			audioSource.Play ();
			audioSource.time = .2f;

			col.gameObject.GetComponent<Health> ().TakeDamage (col.impulse.magnitude * attackStrength);
		}
	}
}
