using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObj : MonoBehaviour {

    private AudioManager audioManager;

    public string TeleportSoundEffect;
	public GameObject ParticleEffect; 
	public int TeleportUpBy; 
	// Use this for initialization
	void Start () 
	{
        audioManager = FindObjectOfType<AudioManager>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		CreateParticleEffect (other.transform.position); 
		other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y + TeleportUpBy, other.transform.position.z); 
		CreateParticleEffect (other.transform.position);
        audioManager.PlaySound(TeleportSoundEffect);
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
