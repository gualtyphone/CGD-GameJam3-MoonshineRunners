using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJoining : MonoBehaviour {

    public int playerID;
    bool joined = false;
    bool ready = false;

    [SerializeField]
    JoinReadyGo joinPanel;

    [SerializeField]
    PlayersJoined pJ;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump"+playerID))
        {
            if (!joined)
            {
                joined = true;
                joinPanel.setState(JoinState.Joined);
                pJ.join(playerID);
            }
            else if (!ready)
            {
                ready = true;
                joinPanel.setState(JoinState.Ready);
                pJ.ready(playerID);
            }
            else
            {
                ready = false;
                joinPanel.setState(JoinState.Joined);
                pJ.notReady(playerID);
            }
        }
	}
}
