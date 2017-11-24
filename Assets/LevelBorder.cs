using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBorder : MonoBehaviour {

	void OnDrawGizmos()
	{
		Gizmos.color = new Color (1, 0, 0, 0.5f);
		Gizmos.DrawWireCube (transform.position, new Vector3 (GetComponent<BoxCollider2D> ().size.x, GetComponent<BoxCollider2D> ().size.y, 0));
	}
	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color (1, 0, 0, 0.5f);
		Gizmos.DrawCube (transform.position, new Vector3 (GetComponent<BoxCollider2D> ().size.x, GetComponent<BoxCollider2D> ().size.y, 0));
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
