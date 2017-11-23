using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class CollectibleManager : MonoBehaviour {


	public GameObject playerChar;
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
        if (col.gameObject.name == "Player")
        {
			if (this.gameObject.tag == "Beer Collectible")
            {
                beerCollectible();
            }

			if (this.gameObject.tag == "Cocktail Collectible")
            {
                cocktailCollectible();
            }
        }
        else
        {
            return;
        }   
    }

    void beerCollectible()
    {
		playerChar.GetComponent<PlayerController> ().drunknessLevel += 5.0f;
    }

    void cocktailCollectible()
    {
		playerChar.GetComponent<PlayerController> ().drunknessLevel += 10.0f;
    }

};