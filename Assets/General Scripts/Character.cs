﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Health))]
public abstract class Character : MonoBehaviour {

	public bool isMyTurn = false, inPlay = false;
    public enum CharacterState { Idle, Selected, Charging, Launched, Off };//off refers to off turn
    public CharacterState myState;
    [SerializeField]
    private GameObject healthBarPrefab;
    [HideInInspector]
    public Health health;
    public Image healthBar, healthBarFill;
    public float attackStrength;
    public void OnCollisionEnter(Collision col)
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

    public void Start()
    {
        health = GetComponent<Health>();
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        //the outline bar must be the parent gameobject and the fill its first child
        GameObject temp = Instantiate(healthBarPrefab) as GameObject;
        temp.name = name + " health bar";
        healthBarFill = temp.GetComponent<Image>();
        healthBar = healthBarFill.transform.GetChild(0).GetComponent<Image>();
        healthBarFill.transform.SetParent(UIManager.main.transform);
        AdjustHealthBarPosition();

        //        healthBarHolder.rectTransform.anchorMax = pos;
        //      healthBarHolder.rectTransform.anchorMin = pos;

        healthBarFill.gameObject.SetActive(false);
    }

    public void AdjustHealthBarPosition()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 screenPos = new Vector2(pos.x, pos.y) - UIManager.main.GetComponent<RectTransform>().anchoredPosition;
        healthBarFill.rectTransform.anchoredPosition = screenPos;

    }

    public abstract IEnumerator EndTurn();


}
