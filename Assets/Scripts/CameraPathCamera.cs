using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraPathCamera : MonoBehaviour {
    //List<GameObject> nodes;
	[SerializeField]
	public CameraPathNode currentNode;
	[SerializeField]
	float speedMultiplier;
	[SerializeField]
	float accelerationDampener = 0.01f; 
	[SerializeField]
	bool changeDirection = false; 

	[SerializeField]
	float minY, maxY;

	GameObject[] players;
    int i = 0; 
    // Use this for initialization
    void Start ()
    {
        //nodes = GameObject.FindGameObjectsWithTag("Node");
		players = GameObject.FindGameObjectsWithTag ("Player"); 
		//Array.Sort (nodes, delegate(GameObject node1, GameObject node2) { return node1.name.CompareTo(node2.name); }); 
		//currentNode = nodes[1];
	}
	
	// Update is called once per frame
	void Update ()
    {
		//Increase Speed Multiplier
		foreach (GameObject x in players) {
			if (x.transform.position.x > transform.position.x + 3) {
				speedMultiplier = speedMultiplier + (x.transform.position.x - transform.position.x) * accelerationDampener;
			}
		}

		//Add Drag to Speed Multiplier
		if (speedMultiplier > 3) {
			speedMultiplier = speedMultiplier - 0.05f;
		}

		//Desired Next position
		Vector3 nextPosition = new Vector3(calculateXPos(), calculateYPos(), transform.position.z);

		Vector3 direction = nextPosition - transform.position;
		direction.Normalize ();
        direction.y += nextPosition.y - transform.position.y;

		//Move
		float speed = speedMultiplier * Time.deltaTime;
		transform.position += direction * speed;


		//Did we reach the node?
		if (currentNode.transform.position.x > transform.position.x - 2 && currentNode.transform.position.x < transform.position.x + 2)
		{
			if (currentNode.nextNode != null) {
				currentNode = currentNode.nextNode;
			}
			/*
			if (changeDirection) 
			{
				if (i == 0)
					i = nodes.Count - 1;
				else
					i--; 	
				currentNode = nodes[i];
			}
			else 
			{
				if (i >= nodes.Count - 1)
					i = 0; 
				else
					i++; 	
				currentNode = nodes[i];
			}
			*/
		}
    }

	float calculateYPos()
	{
		float averageYpos = currentNode.transform.position.y; 
		foreach(GameObject x in players)
			averageYpos += x.transform.position.y;

		averageYpos /= players.Length + 1;
		averageYpos = Mathf.Min (maxY, Mathf.Max (minY, averageYpos));
		return (averageYpos);
		/*
		 * if (averageYpos > transform.position.y + 1)
		 * 	pushUp (); 
		 * if (averageYpos < transform.position.y - 2)
		 * 	pushDown (); 
		 */
	}

	float calculateXPos()
	{
		return currentNode.transform.position.x;
		//		if (transform.position.x > currentNode.transform.position.x)
		//			transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
		//		if (transform.position.x < currentNode.transform.position.x)
		//			transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);



		//        if (transform.position.y > currentNode.transform.position.y)
		//			transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
		//        if (transform.position.y < currentNode.transform.position.y)
		//			transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
	}
}
