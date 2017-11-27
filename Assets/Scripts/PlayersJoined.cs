using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersJoined : Singleton<PlayersJoined> {

    [SerializeField]
    List<int> playersJoined;
    [SerializeField]
    public List<int> playersReady;

    bool countdownRunning = false;
    Timer timer;

    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(gameObject);
        playersJoined = new List<int>();
        playersReady = new List<int>();
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
				SceneManager.LoadScene (2);
			}

		} else if (SceneManager.GetActiveScene ().buildIndex == 0) {
			Destroy (gameObject);
		}
    }

	void OnLevelWasLoaded(int level)
	{
		switch (level)
		{
		case 4:
			PlayerEndScreenCard[] playerCards = FindObjectsOfType<PlayerEndScreenCard> ();
			foreach (var card in playerCards) {
				if (playersJoined.Contains (card.playerID)) {

				} else {
					card.gameObject.SetActive (false);
				}
			}
			break;
		case 2:
			foreach (var player in FindObjectsOfType<PlayerJoining> ()) {
				player.pJ = this;
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


}