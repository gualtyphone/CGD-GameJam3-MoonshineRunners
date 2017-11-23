using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    PlayersJoined pj;

    [SerializeField]
    GameObject playerPrefab;

	List<PlayerController> players;

    [SerializeField]
    Transform startingPoint;

	[SerializeField]
	int roundNumber;

	[SerializeField]
	int roundsPerGame;


	[SerializeField]
	Camera cam;

	[SerializeField]
	ScoreManager scoreManager;

    // Use this for initialization
    void Awake () {
		players = new List<PlayerController> ();
        pj = FindObjectOfType<PlayersJoined>();
        if (pj == null)
        {
			GameObject go = new GameObject();
            go.AddComponent<PlayersJoined>();
            go.name = "PlayersJoined";
            pj = go.GetComponent<PlayersJoined>();
            pj.join(0);
            pj.join(1);
            pj.ready(0);
            pj.ready(1);
        }

        foreach (int playerID in pj.playersReady)
        {
			players.Add(Instantiate(playerPrefab, startingPoint.transform.position, Quaternion.identity).GetComponent<PlayerController>());
            players[players.Count - 1].playerNumber = playerID;
        }

		NextRound ();
	}
	
	// Update is called once per frame
	void Update () {
		if (players.FindAll (player => player.alive == true).Count == 1) {
			NextRound ();
		}
	}

	void NextRound()
	{
		if (roundNumber == roundsPerGame) {
			SceneManager.LoadScene (4);
			return;
		}
		roundNumber++;
		foreach (var player in players) {
			if (player.alive) {
				scoreManager.addScore (player.playerNumber);
			}
			player.alive = true;

			player.transform.position = startingPoint.position;

			//reset Camera;

		}
	}
}