using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private AudioManager audioManager;
    public string Tunes;

    // Use this for initialization
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlaySound(Tunes);
    }

	// Update is called once per frame
	void Update ()
    { 
		if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(1);
        }
	}
}
