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

	[SerializeField]
	CollectibleType type;

	public Sprite beer;
	public Sprite cocktail;

    public ScoreManager score;
    public float alcoholLevel;

	// Use this for initialization
	void Start () {

		if (type == CollectibleType.Beer) {
			this.GetComponent<SpriteRenderer> ().sprite = beer;
		}
		else if (type == CollectibleType.Cocktail) {
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
			if (type == CollectibleType.Beer)
            {
                
				IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 5.0f);
                score.addDrink(col.GetComponent<PlayerController>(), "Beer");
            }

			if (type == CollectibleType.Cocktail)
            {
               
                IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 10.0f);
                score.addDrink(col.GetComponent<PlayerController>(), "Cocktail");

            }

			Destroy (gameObject);
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