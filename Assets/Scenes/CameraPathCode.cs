using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPathCode : MonoBehaviour {
    GameObject[] nodes;
    GameObject currentNode;
    public float speed = 0.01f;
	public bool changeDirection = false; 
    int i = 0; 
    // Use this for initialization
    void Start ()
    {
        nodes = GameObject.FindGameObjectsWithTag("Node");
        currentNode = nodes[0];
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (currentNode.transform.position.x > transform.position.x - 1 && currentNode.transform.position.x < transform.position.x + 1 &&
		    currentNode.transform.position.y > transform.position.y - 1 && currentNode.transform.position.y < transform.position.y + 1)

        {
			if (changeDirection) 
			{
				if (i == 0)
					i = nodes.Length; 
				currentNode = nodes[i];
				i--; 
			}
			else 
			{
				if (i == nodes.Length)
					i = 0; 
				currentNode = nodes[i];
				i++; 
			}
        }
        if (transform.position.x > currentNode.transform.position.x)
			transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
        if (transform.position.x < currentNode.transform.position.x)
			transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        if (transform.position.y > currentNode.transform.position.y)
			transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
        if (transform.position.y < currentNode.transform.position.y)
			transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
    }
}
