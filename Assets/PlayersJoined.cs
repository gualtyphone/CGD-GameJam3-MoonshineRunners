using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayersJoined : MonoBehaviour {

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
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (playersJoined.Count >= 2)
            {
                foreach (var id in playersJoined)
                {
                    if (!playersReady.Contains(id))
                    {
                        //stop and reset countdown
                        countdownRunning = false;
                        return;
                    }
                }
                if (!countdownRunning)//countdown not running
                {
                    //startCountdown
                    countdownRunning = true;
                    timer = new Timer(3.0f);
                }
                if (timer.Trigger())
                {
                    SceneManager.LoadScene(3);
                }
            }
        }

        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {

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
