using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum JoinState
{
    NotJoined,
    Joined,
    Ready
}

public class JoinReadyGo : MonoBehaviour {

    public Text press;
    public Text toSomething;
    public Image A;
    public Text ready;

	// Use this for initialization
	void Start () {
		
	}

    public void setState(JoinState state)
    {
        switch (state)
        {
            case JoinState.NotJoined:

                break;
            case JoinState.Joined:

                break;
            case JoinState.Ready:

                break;

        }
    }
}
