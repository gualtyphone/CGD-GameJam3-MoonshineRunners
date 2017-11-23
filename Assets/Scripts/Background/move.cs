using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {

	public float speed;

	private Rigidbody2D rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal0");
		float moveVertical = Input.GetAxis ("Vertical0");

		Vector3 movement = new Vector3 (moveHorizontal, transform.position.y, moveVertical);

		rb.AddForce (movement * speed);
	}
}
