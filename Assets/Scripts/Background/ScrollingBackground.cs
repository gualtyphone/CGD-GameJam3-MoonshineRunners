using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

	public bool scrolling, parallax;

	public float backgroundSize;

	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;

	public float parallaxSpeed;
	private float lastCameraX;
	public GameObject foregroundObject1;
	public GameObject foregroundObject2;
	public GameObject foregroundObject3;
	private Vector2 foregroundPos;



	private void Start()
	{
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) 
		{
			layers [i] = transform.GetChild (i);

			leftIndex = 0;
			rightIndex = layers.Length - 1;
		}
	}

	private void Update()
	{
		if (parallax) 
		{
			float deltaX = cameraTransform.position.x - lastCameraX;
			transform.position += Vector3.right * (deltaX * parallaxSpeed);
		}
		lastCameraX = cameraTransform.position.x;

		if (scrolling) 
		{
			if (cameraTransform.position.x < (layers [leftIndex].transform.position.x + viewZone)) {//If camera position is less than the left most image, scroll left
				ScrollLeft ();
				//foregroundObject1.transform.position = foregroundPos;
			}

			if (cameraTransform.position.x > (layers [rightIndex].transform.position.x - viewZone)) { //If camera position is more than the right most image, scroll right
				ScrollRight ();
			}
		}

		//foregroundObject1.transform.position = foregroundPos;
		//foregroundObject2.transform.position = foregroundPos;
		//foregroundObject3.transform.position = foregroundPos;
	}

	private void ScrollLeft()
	{
		int lastRight = rightIndex;
		layers [rightIndex].position = Vector3.right * (layers [leftIndex].position.x - backgroundSize); //Gets position of the most left image, and minuses the backgroundsize
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0) 
		{
			rightIndex = layers.Length - 1;
		}
	}

	private void ScrollRight()
	{
		int lastLeft = leftIndex;
		layers [leftIndex].position = Vector3.right * (layers [rightIndex].position.x + backgroundSize); //Gets position of the most left image, and minuses the backgroundsize
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length) 
		{
			leftIndex = 0;
		}
		//layers[leftIndex]

	}
}
