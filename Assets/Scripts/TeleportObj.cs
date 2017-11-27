using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObj : MonoBehaviour {

	public GameObject ParticleEffect; 
	public int TeleportUpBy; 
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		CreateParticleEffect (other.transform.position); 
		other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + TeleportUpBy, other.transform.position.z); 
		CreateParticleEffect (other.transform.position); 
		//wait
		wait(); 
		Destroy(gameObject); 
	}

	void CreateParticleEffect(Vector3 pos)
	{
		GameObject PE = Instantiate(ParticleEffect, pos, Quaternion.identity); 
	}
	IEnumerator wait()
	{
		yield return new WaitForSeconds(1); 
	}

}
