using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = 0.7f;
	[Range(0.5f, 1.5f)]
	public float pitch = 1f;

	[Range(0f, 0.5f)]
	public float randomVolume = 0.2f;
	[Range(0f, 0.5f)]
	public float randomPitch = 0.3f;

	private AudioSource source;

    public void setSource (AudioSource _source)
	{
		source = _source;
		source.clip = clip;
	}

	public void Play ()
	{
		source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
		source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        if (!source.isPlaying)
        {
            source.Play();
        }
	}
}

public class AudioManager : MonoBehaviour 
{
	[SerializeField]
	List<Sound> sounds;

    void Awake()
	{
		for (int i = 0; i < sounds.Count; i++) 
		{
			GameObject _go = new GameObject ("Sound_" + i + "_" + sounds [i].name);
			_go.transform.SetParent (this.transform);
			sounds [i].setSource (_go.AddComponent<AudioSource>());
		}
    }

	public void PlaySound(string _name)
	{
        Sound found = sounds.Find(sound => sound.name == _name);

        if (found != null)
        { 			
			found.Play ();
            Debug.Log("no sound");
		}
	}
}
