using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

	private AudioManager audioManager;

	public string DeathSoundEffect;

	void Awake()
	{
		audioManager = FindObjectOfType<AudioManager> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			audioManager.PlaySound (DeathSoundEffect);
			other.GetComponent<PlayerController> ().alive = false;

		}
	}
}
