using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public static UIManager main;
	public GameObject playerBar;
	public Image playerBarFill;
    
	// Use this for initialization
	void Awake () {
        main = this;

	}

	// Update is called once per frame
	void Update () {

	}
    public void SetBarFill(float fill) {
        playerBarFill.fillAmount = fill;
    }

    public void SetChargeMeterColor(Color color)
    {
        playerBarFill.color = color;
        print(playerBarFill.color);
    }
	public void SetPlayerBar(GameObject bar){
        playerBar = Instantiate(bar, this.transform) as GameObject;
        playerBarFill = playerBar.transform.Find("Fill").GetComponent<Image>();
        
 
	}
}
