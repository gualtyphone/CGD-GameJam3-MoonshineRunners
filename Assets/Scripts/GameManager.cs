using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    PlayersJoined pj;

    private AudioManager audioManager;

    public string backgroundTune;
	public string countdownSoundEffect;

    [SerializeField]
    GameObject[] playerPrefabs;

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
	[SerializeField]
	LevelManager levelManager;

	[SerializeField]
	Text CenterText;

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
			players.Add(Instantiate(playerPrefabs[playerID], startingPoint.transform.position, Quaternion.identity).GetComponent<PlayerController>());
            players[players.Count - 1].playerNumber = playerID;
        }

		foreach (var player in players) {
			player.alive = true;

			player.transform.position = startingPoint.position;
		}

        audioManager = FindObjectOfType<AudioManager>();

		StartCoroutine (RoundCountdown ());
	}
	
	// Update is called once per frame
	void Update () {

        audioManager.PlaySound(backgroundTune);

		if (players.FindAll (player => player.alive == true).Count <= 1) {
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
				pj.addScore (player.playerNumber);
			}
			player.alive = true;

			player.transform.position = startingPoint.position;
			player.GetComponent<Rigidbody2D> ().velocity = new Vector3 ();

			//reset Camera;
			levelManager.reset();
            

			StartCoroutine (RoundCountdown ());
		}


	}

	IEnumerator RoundCountdown()
	{
		//Time.timeScale = 0.0f;
		//Disable Players movement and camera
		cam.GetComponent<CameraPathCamera>().enabled = false;
		foreach (var player in players) {
			player.enabled = false;
		}

		audioManager.PlaySound (countdownSoundEffect);

		CenterText.text = "3";
		yield return new WaitForSecondsRealtime (1.0f);

		CenterText.text = "2";
		yield return new WaitForSecondsRealtime (1.0f);

		CenterText.text = "1";
		yield return new WaitForSecondsRealtime (1.0f);

		CenterText.text = "RUN!";
		//Time.timeScale = 1.0f;
		//Enable Player movement and camera
		cam.GetComponent<CameraPathCamera>().enabled = true;
		foreach (var player in players) {
			player.enabled = true;
		}

		yield return new WaitForSecondsRealtime (1.0f);

		CenterText.text = "";
		yield return null;

	}
}