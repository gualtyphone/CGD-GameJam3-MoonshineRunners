using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;


[System.Serializable]
public class collectedDrinks
{
	public collectedDrinks()
	{
		drinksObtained = new List<CollectibleType> ();

	}

    public List<CollectibleType> drinksObtained;
    public int playerID;
	public int playerScore;
};

public class PlayersJoined : Singleton<PlayersJoined> {

	[SerializeField]
	GameObject drinkPrefab;

    [SerializeField]
    List<int> playersJoined;
    [SerializeField]
    public List<int> playersReady;

    bool countdownRunning = false;
    Timer timer;


	public Sprite beer;
	public Sprite cocktail;
	public Sprite pizza;
	public Sprite bed;

	public Sprite first;
	public Sprite second;
	public Sprite third;
	public Sprite last;

    public List<collectedDrinks> drinks;

	public List<collectedDrinks> sortedList;

	Vector2 pos = new Vector2 (-150.0f, -50.0f);
	Vector2 size = new Vector2 (100.0f, 100.0f);

    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(gameObject);

        playersJoined = new List<int>();
        playersReady = new List<int>();
        drinks = new List<collectedDrinks>();

		sortedList = new List<collectedDrinks> ();

		//Application.LoadLevel (4);
		//OnLevelWasLoaded(4);
    }

    // Update is called once per frame
    void Update()
    {
		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			if (playersJoined.Count >= 2) {
				foreach (var id in playersJoined) {
					if (!playersReady.Contains (id)) {
						//stop and reset countdown
						countdownRunning = false;
						return;
					}
				}
				if (!countdownRunning) {//countdown not running
					//startCountdown
					countdownRunning = true;
					timer = new Timer (3.0f);
				}
				if (timer.Trigger ()) {
					SceneManager.LoadScene (3);
				}
			}
		} else if (SceneManager.GetActiveScene ().buildIndex == 4) {
			if (Input.GetButtonDown ("Submit")) {
				timer = new Timer (3.0f);
				playersReady.Clear ();
				playersJoined.Clear ();
				SceneManager.LoadScene (2);
			}

		} else if (SceneManager.GetActiveScene ().buildIndex == 0) {
			Destroy (gameObject);
		}
    }

	void OnLevelWasLoaded(int level)
	{
		switch (level) {
		case 4:
			sortedList = drinks.OrderByDescending (i => i.playerScore).ToList();
			PlayerEndScreenCard[] playerCards = FindObjectsOfType<PlayerEndScreenCard> ();
			foreach (var card in playerCards) {
				if (playersJoined.Contains (card.playerID)) {

					if (drinks.Find (dr => dr.playerID == card.playerID).drinksObtained.Count > 0) {
						pos.x = -170.0f;
						float space;

					
						space = 400 / drinks.Find (dr => dr.playerID == card.playerID).drinksObtained.Count;

						foreach (var drink in  drinks.Find(dr => dr.playerID == card.playerID).drinksObtained) {

							GameObject clone = Instantiate (drinkPrefab, Vector3.zero, Quaternion.identity, card.transform);
							clone.transform.localPosition = pos;
							pos.x += space - space/drinks.Find (dr => dr.playerID == card.playerID).drinksObtained.Count;
							if (drinks.Find (dr => dr.playerID == card.playerID).drinksObtained.Count > 6) {
								clone.GetComponent<RectTransform> ().sizeDelta = new Vector2 (space, space);

							} 
						//else if (drinks.Find (dr => dr.playerID == card.playerID).drinksObtained.Count > 10) {
							//pos.y += space;
							//pos.x = -150.0f;
						//} 
							else {
								clone.GetComponent<RectTransform> ().sizeDelta = size;
							}
							


							switch (drink) {
							case CollectibleType.Beer:
								clone.GetComponent<Image> ().overrideSprite = beer;
								break;
							case CollectibleType.Cocktail:
								clone.GetComponent<Image> ().overrideSprite = cocktail;
								break;
							case CollectibleType.Food:
								clone.GetComponent<Image> ().overrideSprite = pizza;
								break;
							case CollectibleType.Bed:
								clone.GetComponent<Image> ().overrideSprite = bed;
								break;

							}
						}
					}
					switch (sortedList.FindIndex (i => i.playerID == card.playerID)) {
					case 0:
						card.transform.Find ("PlayerMedal").GetComponent<Image> ().overrideSprite = first;
						break;
					case 1:
						card.transform.Find ("PlayerMedal").GetComponent<Image> ().overrideSprite = second;
						break;
					case 2:
						card.transform.Find ("PlayerMedal").GetComponent<Image> ().overrideSprite = third;
						break;
					case 3:
						card.transform.Find ("PlayerMedal").GetComponent<Image> ().overrideSprite = last;
						break;
					}


				}
				else {
					card.gameObject.SetActive (false);
				}

			}


				

			break;
		case 2:
			foreach (var player in FindObjectsOfType<PlayerJoining> ()) {
				player.pJ = this;
			}
			drinks.Clear ();
			break;
       case 3:
                foreach (var player in playersJoined)
                {
                    drinks.Add(new collectedDrinks());
                    drinks[drinks.Count - 1].playerID = player;
                }
            break;
		}
		}

	


    public void join(int playerId)
    {
        if (!playersJoined.Contains(playerId))
        {
            playersJoined.Add(playerId);
        }
    }

    public void ready(int playerId)
    {
        if (!playersReady.Contains(playerId))
        {
            playersReady.Add(playerId);
        }
    }

    public void notReady(int playerId)
    {
        if (playersReady.Contains(playerId))
        {
            playersReady.Remove(playerId);
        }
    }

	public void addScore(int playerNumber)
	{
		drinks.Find (i => i.playerID == playerNumber).playerScore++;
	}
}