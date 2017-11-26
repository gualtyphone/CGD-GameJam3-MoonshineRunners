using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    PlayerController player;

    public Text drunkness;
    //public ScoreManager score;
	// Use this for initialization
	void Start () {
        if (drunkness != null)
        {
            drunkness.text = "";
        }
	}
	
	// Update is called once per frame
	void Update () {
        
    }

	public void addScore(PlayerController playerNumber)
	{
        playerNumber.score += 1;
    }

    public void addDrink(PlayerController player, string type)
    {
        if(type == "Beer")
        {
            player.beerCount++;
            player.alcoholLevel += 2.0f;
            
        }
        else if(type == "Cocktail")
        {
            player.cocktailCount++;
            player.alcoholLevel += 4.0f;
        }
    }

    public void finalScore(PlayerController player)
    {
        if (player.alcoholLevel > 10.0f)
        {
            drunkness.text = "You drunk boiii";
        }
        else
        {
            drunkness.text = "Boring";
        }

    }

    
}
