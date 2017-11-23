using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPathCode : MonoBehaviour {
    GameObject[] nodes;
    GameObject currentNode;
	GameObject[] players; 
    public float speedMultiplier = 3;
	public float accelerationDampener = 0.01f; 
	public bool changeDirection = false; 
    int i = 0; 
    // Use this for initialization
    void Start ()
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");
		players = GameObject.FindGameObjectsWithTag ("Player"); 
		Array.Sort (nodes, delegate(GameObject node1, GameObject node2) { return node1.name.CompareTo(node2.name); }); 
		currentNode = nodes[1];
	}
	
	// Update is called once per frame
	void Update ()
    {
		float averageYpos = currentNode.transform.position.y; 
		foreach(GameObject x in players)
			averageYpos += x.transform.position.y;

		if (averageYpos > transform.position.y + 1)
			pushUp (); 
		if (averageYpos < transform.position.y - 2)
			pushDown (); 
		
		foreach (GameObject x in players)
			if (x.transform.position.x > transform.position.x + 3)
				speedMultiplier = speedMultiplier + (x.transform.position.x - transform.position.x) * accelerationDampener; 

		if (speedMultiplier > 3)
		speedMultiplier = speedMultiplier - 0.05f; 
		
		float speed = speedMultiplier * Time.deltaTime; 

		//if (x.transform.position.x < transform.position.x + 3)
		//	speed /= speedMultiplier; 

		if (currentNode.transform.position.x > transform.position.x - 2 && currentNode.transform.position.x < transform.position.x + 2 &&
		    currentNode.transform.position.y > transform.position.y - 2 && currentNode.transform.position.y < transform.position.y + 2)
        {
			if (changeDirection) 
			{
				if (i == 0)
					i = nodes.Length - 1;
				else
				i--; 	
				currentNode = nodes[i];
			}
			else 
			{
				if (i >= nodes.Length - 1)
					i = 0; 
				else
				i++; 	
				currentNode = nodes[i];
			}
        }
        if (transform.position.x > currentNode.transform.position.x)
			transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        if (transform.position.x < currentNode.transform.position.x)
			transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
//        if (transform.position.y > currentNode.transform.position.y)
//			transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
//        if (transform.position.y < currentNode.transform.position.y)
//			transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
    }

	void pushUp()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
	}
	void pushDown()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
	}
}
