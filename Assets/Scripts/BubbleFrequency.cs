using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleFrequency : MonoBehaviour {

	ParticleSystem drunkBubbles; 
	[Range(0, 10)]
	public int Drunkness = 0; 
	int lastDrunkness; 
	// Use this for initialization
	void Start () {
		drunkBubbles = GetComponent<ParticleSystem> (); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (lastDrunkness != Drunkness) 
		{
			var em = drunkBubbles.emission; 
			em.rateOverTime = Drunkness; 
			lastDrunkness = Drunkness; 
		}
	}
}
