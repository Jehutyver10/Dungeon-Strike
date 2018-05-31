using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager main;
	int turnCount = 0, tierWorth = 3;
	public List<GameObject> ButtonList;
	public GameObject EmptyButton;
	public GameObject GameOverScreen;
	public GameObject VictoryScreen;
	public GameObject FighterUpgradeScreen;
	public GameObject minionPrefab, standardPrefab, elitePrefab, bossPrefab;
	public List<Character> characters;
	public List<Transform> enemyPositions;
	public bool endTurn = false;
	public int bossNum, levelCount = 1;
	Vector3 playerStartPosition;
	// Use this for initialization


	void Awake(){
		DontDestroyOnLoad (gameObject);
		DontDestroyOnLoad (GameObject.Find ("Environment"));
		DontDestroyOnLoad (FindObjectOfType<PlayerController> ().gameObject);
		DontDestroyOnLoad (FindObjectOfType<Camera> ().gameObject);
		DontDestroyOnLoad (FindObjectOfType<Canvas> ());
		DontDestroyOnLoad(GameObject.Find("EventSystem"));
		DontDestroyOnLoad (FindObjectOfType<MusicManager> ().gameObject);
		characters = new List<Character>();
		characters.Insert (0, FindObjectOfType<PlayerController> ());
	}
	void Start () {
		playerStartPosition = FindObjectOfType<PlayerController> ().transform.position;
	}

	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("Main Camera") && levelCount < 15) {
			Destroy (GameObject.Find ("Main Camera").gameObject);
		}
//		if (GameObject.Find ("Directional Light")) {
//			Destroy (GameObject.Find ("Directional Light").gameObject);
//			print ("Destroy default light");
//		}
		//if (endTurn) {
		//	EndTurn ();
		//	endTurn = !endTurn;
		//}
	}

    public IEnumerator EndTurn(Character c, float delayTime = 3) {
        c.isMyTurn = false;

        int listPos = (characters.IndexOf(c) + 1) % (characters.Count);
        while (c.myState != Character.CharacterState.Off)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delayTime);

        if (characters.Count > 1)
        {
            BeginTurn(characters[listPos]);
        }
        //if (characters.count > 1 && findobjectoftype<playercontroller>())
        //{

        //    for (int i = 0; i < characters.count; i++)
        //    {

        //        if (characters[i].ismyturn == true)
        //        {
        //            characters[i].ismyturn = false;
        //            characters[(i + 1) % characters.count].ismyturn = true;
        //            if (characters[(i + 1) % characters.count].getcomponent<enemy>())
        //            {
        //                characters[(i + 1) % characters.count].getcomponent<enemy>().launched = false;
        //            }
        //            else
        //            {
        //                if (characters[(i + 1) % characters.count].getcomponent<fighter>())
        //                {
        //                    characters[(i + 1) % characters.count].getcomponent<fighter>().resetstats();
        //                }
        //            }

        //            print("it was the " + characters[i].name + "'s turn, but now it is the " + characters[(i + 1) % characters.count].name + "'s turn");
        //            break;

        //        }
        //    }
        //    turncount++;
        //}

    }
    void BeginTurn(Character c)
    {
        foreach(Character ch in characters)
        {
            ch.isMyTurn = false;
            ch.myState = Character.CharacterState.Off;
        }
        c.isMyTurn = true;
        c.myState = Character.CharacterState.Idle ;
    }

    public void OnPlayerDeath(){
		GameOverScreen.SetActive(true);
	}

	public void Retry(){
		Enemy[] enemies = FindObjectsOfType (typeof(Enemy)) as Enemy[];
		foreach (Enemy enemy in enemies) {
			Destroy (enemy);		
		}
		GameOverScreen.SetActive (false);
		SceneManager.LoadScene ("Level 1");
		Destroy (FindObjectOfType<Camera> ().gameObject);
		Destroy (GameObject.Find ("EventSystem").gameObject);
		if (FindObjectOfType<Enemy> ()) {
			Destroy (FindObjectOfType<Enemy> ().gameObject);
		}
		Destroy (GameObject.Find ("Environment").gameObject);
		Destroy (FindObjectOfType<Canvas> ().gameObject);
		Destroy (FindObjectOfType<MusicManager> ().gameObject);
		Destroy (gameObject);

	}

	public void ContinueToNextLevel(){
		FighterUpgradeScreen.SetActive (false);
		levelCount++;

		SceneManager.CreateScene ("Level " + levelCount.ToString ());
		SceneManager.LoadScene ("Level " + levelCount.ToString ());
		if (levelCount < 15) {
			FindObjectOfType<PlayerController> ().transform.position = playerStartPosition;
			FindObjectOfType<PlayerController> ().GetComponent<Health> ().health = FindObjectOfType<PlayerController> ().GetComponent<Health> ().maxHealth;
			FindObjectOfType<PlayerController> ().GetComponent<Rigidbody> ().velocity = Vector3.zero;
			SpawnEnemies ();
			turnCount = 0;
            BeginTurn(FindObjectOfType<PlayerController>());
		} else {
			Destroy (FindObjectOfType<Camera> ().gameObject);
			Destroy (GameObject.Find ("EventSystem").gameObject);
			Destroy (FindObjectOfType<PlayerController> ().gameObject);
			Destroy (GameObject.Find ("Environment").gameObject);
			Destroy (FindObjectOfType<Canvas> ().gameObject);
			Destroy (FindObjectOfType<MusicManager> ().gameObject);
			Destroy (gameObject);
		}
		if (levelCount == 14) {
			FindObjectOfType<MusicManager> ().audioSource.clip = FindObjectOfType<MusicManager> ().audioClips [1];
			FindObjectOfType<MusicManager> ().audioSource.Play ();
		}



	}

	public void OnVictory (){
		VictoryScreen.SetActive(true);

	}

	public void AdjustButtons(){	
		for(int i = 0; i < ButtonList.Count; i++) {
			if (ButtonList[i].GetComponent<Toggle>().isOn) {
				GameObject newButton = Instantiate (EmptyButton, ButtonList [i].transform.parent);
				Destroy (ButtonList [i]);
				ButtonList[i] = newButton;
			}
		}
	}
	public void ContinueToUpgrades(){
		VictoryScreen.SetActive (false);
		if (FindObjectOfType<PlayerController>().GetComponent<Fighter>()) {
			FighterUpgradeScreen.SetActive (true);

		}
		foreach (PurchaseButton b in FindObjectsOfType<PurchaseButton>()) {
			b.GetComponent<Button> ().interactable = true;
		}
	}

	public void SpawnEnemies(){
		List<GameObject> enemyList = new List<GameObject> ();
		int eliteNum, standardNum, minionNum, enemyCount;
		if (levelCount >= bossNum) {//if at the final boss
		} else {
			enemyCount = levelCount * 2 - 1;
			//counting elites

			if (enemyCount < tierWorth * tierWorth) {//if there are too few enemies for even one elite
				eliteNum = 0;
			} else {
				eliteNum = (enemyCount - enemyCount % (tierWorth * tierWorth)) / (tierWorth * tierWorth);
				enemyCount = enemyCount % (tierWorth * tierWorth);
			}
			//counting standards
			if (enemyCount % (tierWorth) >= enemyCount) {//if there are too few enemies for even one standard
				standardNum = 0;
			} else {
				standardNum = (enemyCount - enemyCount % (tierWorth)) / tierWorth;
				enemyCount = enemyCount % tierWorth;
			}
			//counting minions
			minionNum = enemyCount;
		//	print ("Enemy stock: " + (levelCount * 2 -1).ToString() + ", Elites: " + eliteNum.ToString() + ", Standards: " + standardNum.ToString() + ", Minions: " + minionNum.ToString());

			//assign enemies to spots 
			if (levelCount < 14) {
				for (int i = 0; i < minionNum; i++) {
					GameObject minion = Instantiate (minionPrefab);
					minion.transform.position = enemyPositions [i].position;

				}
				for (int i = minionNum; i < minionNum + standardNum; i++) {
					GameObject standard = Instantiate (standardPrefab);
					standard.transform.position = enemyPositions [i].position;

				}
				for (int i = minionNum + standardNum; i < minionNum + standardNum + eliteNum; i++) {
					GameObject elite = Instantiate (elitePrefab);
					elite.transform.position = enemyPositions [i].position;
				}
			} else if (levelCount == 14) {
				GameObject boss = Instantiate (bossPrefab) as GameObject;
				enemyList.Add (boss);
				boss.transform.position = new Vector3 (3, 0.059f, 0);

			}

		}

		for (int i = 0; i < enemyList.Count; i++) {
			
		}
	}


}
