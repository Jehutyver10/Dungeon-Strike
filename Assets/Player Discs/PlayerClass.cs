using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Class", menuName = "Player Class")]
public class PlayerClass : ScriptableObject{
	public GameObject ChargeBar;
	public float ChargeTime;
	public float strengthBuffer;

}
