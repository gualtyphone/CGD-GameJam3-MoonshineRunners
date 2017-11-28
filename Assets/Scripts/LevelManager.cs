using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    Level()
    {
         Bits = new List<GameObject>();
    }

    [SerializeField]
    List<GameObject> Bits;

    public static void Destroy(Level lev)
    {
        foreach (var bit in lev.Bits)
        {
            GameObject.Destroy(bit);
        }
        lev.Bits.Clear();
        return;
    }

    public static Level Instantiate(Level lev, Vector3 pos)
    {
        Level newLevel = new Level();
        Vector3 movement = Vector3.zero;
        foreach (var bit in lev.Bits)
        {
            GameObject go = GameObject.Instantiate(bit, pos + movement, Quaternion.identity);
            if (newLevel.Bits.Count > 0)
            {
                newLevel.Bits[newLevel.Bits.Count - 1].GetComponent<CameraPathNode>().nextNode = go.GetComponent<CameraPathNode>();
            }
            newLevel.Bits.Add(go);
            movement.x += 100;
        }

        return newLevel;
    }

    public Vector3 getPosition()
    {
        return Bits[Bits.Count - 1].transform.position;
    }


    public CameraPathNode node { get { return Bits[Bits.Count - 1].GetComponent<CameraPathNode>(); }}
    public CameraPathNode nextNode { get { return Bits[Bits.Count - 1].GetComponent<CameraPathNode>().nextNode; } set { Bits[Bits.Count - 1].GetComponent<CameraPathNode>().nextNode = value; } }
}

public class LevelManager : MonoBehaviour {

	[SerializeField]
	List<Level> LevelPartsPrefabs;

    [SerializeField]
    Level FirstLevelPrefab;

    List<Level> currentInstantiatedLevels;

	[SerializeField]
	GameObject cam;

	[SerializeField]
	Transform cameraStartPoint;

	// Use this for initialization
	void Start () {
		currentInstantiatedLevels = new List<Level> ();
		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = transform.position;

		currentInstantiatedLevels.Add(Level.Instantiate (FirstLevelPrefab, pos));
		currentInstantiatedLevels.Add (InstantiateNewLevelPart ());

		cam.GetComponent<CameraPathCamera>().currentNode = currentInstantiatedLevels [0].node;
	}
	
	// Update is called once per frame
	void Update () {
		if (cam.GetComponent<Camera>().transform.position.x > currentInstantiatedLevels [currentInstantiatedLevels.Count - 1].getPosition().x) {
			currentInstantiatedLevels.Add (InstantiateNewLevelPart ());
		}

		if (currentInstantiatedLevels.Count > 6) {
			Level.Destroy(currentInstantiatedLevels [0]);
			currentInstantiatedLevels.RemoveAt (0);
		}
	}

    Level InstantiateNewLevelPart()
	{
		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = currentInstantiatedLevels [currentInstantiatedLevels.Count-1].getPosition();
		pos.x += 100.0f;
		Level go = Level.Instantiate (LevelPartsPrefabs [rand], pos);
		currentInstantiatedLevels [currentInstantiatedLevels.Count - 1].nextNode = go.node;
		return go;
	}

	public void reset()
	{
		foreach (var level in currentInstantiatedLevels) {
			Level.Destroy (level);
		}

		currentInstantiatedLevels.Clear ();

		int rand = Random.Range (0, LevelPartsPrefabs.Count);
		Vector3 pos = transform.position;

		currentInstantiatedLevels.Add(Level.Instantiate (FirstLevelPrefab, pos));
		currentInstantiatedLevels.Add (InstantiateNewLevelPart ());

		cam.GetComponent<CameraPathCamera>().currentNode = currentInstantiatedLevels [0].node;
        cam.GetComponent<CameraPathCamera>().reset();
        cam.transform.position = cameraStartPoint.position;
	}
}
