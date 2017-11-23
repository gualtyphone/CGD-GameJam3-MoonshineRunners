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
	Vector3 zPos;

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

		//zPos = new Vector3 (-2.0f, 0.0f, -2.0f);
	}

	private void FixedUpdate()
	{
		if (parallax) 
		{
			float deltaX = cameraTransform.position.x - lastCameraX;

			if (gameObject.tag == "tree") {
				transform.position += Vector3.right * (deltaX * parallaxSpeed);
			} 
			else 
			{
				transform.position += Vector3.right * (deltaX * parallaxSpeed);
			}
		}
		lastCameraX = cameraTransform.position.x;

		if (scrolling) 
		{
			if (cameraTransform.position.x < (layers [leftIndex].transform.position.x + viewZone)) {//If camera position is less than the left most image, scroll left
				ScrollLeft ();
			}

			if (cameraTransform.position.x > (layers [rightIndex].transform.position.x - viewZone)) { //If camera position is more than the right most image, scroll right
				ScrollRight ();
			}
		}
	}

	private void ScrollLeft()
	{
		Vector3 a = Vector3.right;
		float b = (layers [leftIndex].localPosition.x - backgroundSize);
		Vector3 c = b * a;
		layers [rightIndex].localPosition = c; //Gets position of the most left image, and minuses the backgroundsize
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0) 
		{
			rightIndex = layers.Length - 1;
		}
	}

	private void ScrollRight()
	{
        Vector3 a = Vector3.right;
		float b = (layers[rightIndex].localPosition.x + backgroundSize);
        Vector3 c = a * b;

        layers[leftIndex].localPosition = c ; //Gets position of the most left image, and minuses the backgroundsize
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length) 
		{
			leftIndex = 0;
		}
	}
}
