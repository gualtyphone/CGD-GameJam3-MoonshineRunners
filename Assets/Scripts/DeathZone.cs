using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

	void OnTriggerEntry(Collider other)
	{
		if (other.tag == "Player")
		{
			other.GetComponent<PlayerController> ().alive = false;
		}
	}
}
