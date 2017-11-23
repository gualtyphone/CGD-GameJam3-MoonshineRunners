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

    public Image playerIcon;
    public Text playerNumber;
    public Text playerText;


    // Use this for initialization
    void Start () {
        setState(JoinState.NotJoined);
	}

    public void setState(JoinState state)
    {
        switch (state)
        {
            case JoinState.NotJoined:
                press.enabled = true;
                A.enabled = true;
                toSomething.enabled = true;
                ready.enabled = false;
                toSomething.text = "to Join!";

                playerIcon.enabled = false;
                playerNumber.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                playerText.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                break;
            case JoinState.Joined:
                press.enabled = true;
                A.enabled = true;
                toSomething.enabled = true;
                ready.enabled = false;
                toSomething.text = " When ready!";

                playerIcon.enabled = true;
                playerNumber.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                playerText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case JoinState.Ready:
                press.enabled = false;
                A.enabled = false;
                toSomething.enabled = false;
                ready.enabled = true;

                playerIcon.enabled = true;
                playerNumber.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                playerText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;

        }
    }
}
