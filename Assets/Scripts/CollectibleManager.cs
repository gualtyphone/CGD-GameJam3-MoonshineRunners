using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class CollectibleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//beerCollectible ();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		Debug.Log ("HI");
        if (col.gameObject.name == "Player")
        {
			if (this.GetComponent<GameObject>().tag == "Beer Collectible")
            {
                beerCollectible();
            }

			if (this.GetComponent<GameObject>().tag == "Cocktail Collectible")
            {
                beerCollectible();
            }
        }
        else
        {
            return;
        }   
    }

    void beerCollectible()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
    }

    void cocktailCollectible()
    {
        this.GetComponent<SpriteRenderer>().color = Color.blue;
    }

};