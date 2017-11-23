using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    PlayersJoined pj;

    [SerializeField]
    GameObject playerPrefab;

    List<GameObject> players;

    [SerializeField]
    Transform startingPoint;

    // Use this for initialization
    void Awake () {
		players = new List<GameObject> ();
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
            players.Add(Instantiate(playerPrefab, startingPoint.transform.position, Quaternion.identity));
            players[players.Count - 1].GetComponent<PlayerController>().playerNumber = playerID;
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
