using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody rb;
	private Vector3 startDrag, endDrag;
	public bool inPlay = false;
	public GameObject cam;
	public float strengthBuffer;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartDrag(){
		
		startDrag = Input.mousePosition;
	}

	public void EndDrag(){
		endDrag = Input.mousePosition;
		Launch ();

	}
	void Launch(){
		float magnitude = Vector3.Magnitude (endDrag - startDrag);
		rb.AddForce(cam.transform.forward *magnitude, ForceMode.Impulse);
			
	}
}
