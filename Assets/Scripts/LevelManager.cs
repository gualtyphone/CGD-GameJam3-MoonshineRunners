using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	[SerializeField]
	List<GameObject> LevelPartsPrefabs;

	List<GameObject> currentInstantiatedLevels;

	[SerializeField]
	GameObject cam;

	[SerializeField]
	Transform cameraStartPoint;

	// Use this for initialization
	void Start () {
		currentInstantiatedLevels = new List<GameObject> ();
		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = transform.position;

		currentInstantiatedLevels.Add(Instantiate (LevelPartsPrefabs [rand], pos, Quaternion.identity));
		currentInstantiatedLevels.Add (InstantiateNewLevelPart ());

		cam.GetComponent<CameraPathCamera>().currentNode = currentInstantiatedLevels [0].GetComponent<CameraPathNode>();
	}
	
	// Update is called once per frame
	void Update () {
		if (cam.GetComponent<Camera>().transform.position.x > currentInstantiatedLevels [currentInstantiatedLevels.Count - 1].transform.position.x) {
			currentInstantiatedLevels.Add (InstantiateNewLevelPart ());
		}

		if (currentInstantiatedLevels.Count > 6) {
			Destroy(currentInstantiatedLevels [0]);
			currentInstantiatedLevels.RemoveAt (0);
		}
	}

	GameObject InstantiateNewLevelPart()
	{
		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = currentInstantiatedLevels [currentInstantiatedLevels.Count-1].transform.position;
		pos.x += 100.0f;
		GameObject go = Instantiate (LevelPartsPrefabs [rand], pos, Quaternion.identity);;
		currentInstantiatedLevels [currentInstantiatedLevels.Count - 1].GetComponent<CameraPathNode> ().nextNode = go.GetComponent<CameraPathNode> ();
		return go;
	}

	public void reset()
	{
		foreach (var level in currentInstantiatedLevels) {
			Destroy (level);
		}

		currentInstantiatedLevels.Clear ();

		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = transform.position;

		currentInstantiatedLevels.Add(Instantiate (LevelPartsPrefabs [rand], pos, Quaternion.identity));
		currentInstantiatedLevels.Add (InstantiateNewLevelPart ());

		cam.GetComponent<CameraPathCamera>().currentNode = currentInstantiatedLevels [0].GetComponent<CameraPathNode>();
		cam.transform.position = cameraStartPoint.position;
	}
}
