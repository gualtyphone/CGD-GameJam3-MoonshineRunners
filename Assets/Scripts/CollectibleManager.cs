using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

enum CollectibleType
{
	Beer,
	Cocktail
}

public class CollectibleManager : MonoBehaviour {
	
	public Sprite beer;
	public Sprite cocktail;

	// Use this for initialization
	void Start () {

		if (this.gameObject.tag == "Beer Collectible") {
			this.GetComponent<SpriteRenderer> ().sprite = beer;
		}
		else if (this.gameObject.tag == "Cocktail Collectible") {
			this.GetComponent<SpriteRenderer> ().sprite = cocktail;
		}


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player")
        {
			if (this.gameObject.tag == "Beer Collectible")
            {
				IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 5.0f);
            }

			if (this.gameObject.tag == "Cocktail Collectible")
            {
				IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 10.0f);
            }
        }
        else
        {
            return;
        }   
    }

	void IncreasePlayerDrunkness(PlayerController player, float drunknessAmount)
    {
		player.GetComponent<PlayerController> ().drunknessLevel += drunknessAmount;
    }

};