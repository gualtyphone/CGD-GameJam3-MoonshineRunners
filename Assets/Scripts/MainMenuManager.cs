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

    }

	// Update is called once per frame
	void Update ()
    {
        audioManager.PlaySound(Tunes);
        if (Input.GetButtonDown("Submit"))
        {
            SceneManager.LoadScene(1);
        }
	}
}
