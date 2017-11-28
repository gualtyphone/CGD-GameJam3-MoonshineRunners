using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public enum CollectibleType
{
	Beer,
	Cocktail,
	Bed,
	Food
}

public class CollectibleManager : MonoBehaviour {

    private AudioManager audioManager;

	[SerializeField]
	CollectibleType type;

	public Sprite beer;
	public Sprite cocktail;
	public Sprite pizza;
	public Sprite bed;

    public ScoreManager score;
    public float alcoholLevel;
	public GameObject collectionParticle;
    [SerializeField]
    public string beerSoundEffect;
    [SerializeField]
    public string cocktailSoundEffect;
    [SerializeField]
    public string foodSoundEffect;
    [SerializeField]
    public string bedSoundEffect;


	// Use this for initialization
	void Start ()
    {
		if (type == CollectibleType.Beer) {
            this.GetComponent<SpriteRenderer>().sprite = beer;
		}
		else if (type == CollectibleType.Cocktail) {
			this.GetComponent<SpriteRenderer> ().sprite = cocktail;
		}
		else if (type == CollectibleType.Food) {
			this.GetComponent<SpriteRenderer> ().sprite = pizza;
		}
		else if (type == CollectibleType.Bed) {
			this.GetComponent<SpriteRenderer> ().sprite = bed;
		}

       // score = FindObjectOfType<ScoreManager>();
	}

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player")
        {
			int playeNumber = col.GetComponent<PlayerController>().playerNumber;
			//FindObjectOfType<PlayersJoined>().drinks.Find(drink => drink.playerID == playeNumber).drinksObtained.Add(type);

			foreach (var drink in FindObjectOfType<PlayersJoined>().drinks) {
				if (drink.playerID == playeNumber) {
					drink.drinksObtained.Add(type);
				}
			}

			if (type == CollectibleType.Beer)
            {   
				IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 5.0f);
				changeParticleColour (Color.red);
                audioManager.PlaySound(beerSoundEffect);
                
            }

			else if (type == CollectibleType.Cocktail)
            {
                IncreasePlayerDrunkness(col.GetComponent<PlayerController>(), 12.0f);
				changeParticleColour(Color.cyan);
                audioManager.PlaySound(cocktailSoundEffect);
                
            }
			else if (type == CollectibleType.Food)
			{   
				DecreasePlayerDrunkness(col.GetComponent<PlayerController>(), 10.0f);
				changeParticleColour(Color.green);
                audioManager.PlaySound(foodSoundEffect);
            }

			else if (type == CollectibleType.Bed)
			{
				DecreasePlayerDrunkness(col.GetComponent<PlayerController>(), 30.0f);
				changeParticleColour(Color.magenta);
                audioManager.PlaySound(bedSoundEffect);
            }
			createParticles ();
			gameObject.SetActive (false);
			Destroy (gameObject);
        }
        else
        {
            return;
        }   
	}

	void changeParticleColour(Color colour)
	{
		var ps = collectionParticle.GetComponent<ParticleSystem> ();

		var main = ps.main;
		main.startColor = colour;
	}

	void IncreasePlayerDrunkness(PlayerController player, float drunknessAmount)
    {
		player.GetComponent<PlayerController> ().drunknessLevel += drunknessAmount;
    }

	void DecreasePlayerDrunkness(PlayerController player, float drunknessAmount)
	{
		player.GetComponent<PlayerController> ().drunknessLevel -= drunknessAmount;
	}

	void createParticles()
	{
		Instantiate (collectionParticle, this.transform.position, this.transform.rotation);
	}
};