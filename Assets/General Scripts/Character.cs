using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Character : MonoBehaviour {

	public bool isMyTurn = false, inPlay = false;
    public enum CharacterState { Idle, Selected, Charging, Launched, Off };//off refers to off turn
    public CharacterState myState;

    public float attackStrength;
    void OnCollisionEnter(Collision col)
    {
        if (myState == CharacterState.Launched)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().time = .25f;
            if (col.gameObject.GetComponent<Health>())
            {
                col.gameObject.GetComponent<Health>().TakeDamage(attackStrength);

            }
        }
    }

}
