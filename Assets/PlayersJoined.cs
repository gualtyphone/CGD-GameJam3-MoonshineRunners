using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersJoined : MonoBehaviour {

    List<int> playersJoined;
    List<int> playersReady;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (playersJoined.Count >= 2)
		foreach(var id in playersJoined)
        {
            if (!playersReady.Contains(id))
            {
                return;
            }
        }

        //startCountdown
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
