using UnityEngine;
using System.Collections;

public class Sound : MonoBehaviour {	
	
	// Use this for initialization
	void Start () {
		PlayFile("Sounds/1");
	}
		
	public void PlayFile(string file) // file name without extension
	{
		AudioClip clip;
		clip = (AudioClip)Resources.Load(file, typeof(AudioClip));
		audio.clip = clip;
	    audio.Play();
	}

}
