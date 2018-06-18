using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Health))]
public class Character : MonoBehaviour {

	public bool isMyTurn = false, inPlay = false;
    public enum CharacterState { Idle, Selected, Charging, Launched, Off };//off refers to off turn
    public CharacterState myState;
    [SerializeField]
    private GameObject healthBarPrefab;
    [HideInInspector]
    public Image healthBar, healthBarFill;
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

    public void Start()
    {
        SetHealthBar();
    }

    public void SetHealthBar()
    {
        //the outline bar must be the parent gameobject and the fill its first child
        GameObject temp = Instantiate(healthBarPrefab) as GameObject;
        temp.name = name + " health bar";
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 screenPos = new Vector2(pos.x, pos.y) - UIManager.main.GetComponent<RectTransform>().anchoredPosition;
        healthBarFill = temp.GetComponent<Image>();
        
        healthBar = healthBarFill.transform.GetChild(0).GetComponent<Image>();
        healthBarFill.transform.SetParent(UIManager.main.transform);
        healthBarFill.rectTransform.anchoredPosition = screenPos;
//        healthBarHolder.rectTransform.anchorMax = pos;
  //      healthBarHolder.rectTransform.anchorMin = pos;
    }
}
