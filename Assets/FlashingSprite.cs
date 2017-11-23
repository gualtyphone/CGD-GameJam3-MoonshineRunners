using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingSprite : MonoBehaviour {

	[SerializeField]
	float timeBetweenChanges;

	[SerializeField]
	List<Sprite> sprites;

	Image img;
	Timer timer;
	int currSprite = 0;

	// Use this for initialization
	void Start () {
		timer = new Timer (timeBetweenChanges);
		img = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timer.Trigger ()) {
			currSprite++;
			if (currSprite >= sprites.Count) {
				currSprite = 0;
			}
			img.sprite = sprites [currSprite];
		}
	}
}
