using UnityEngine;
using System.Collections;

public class SoundFade : MonoBehaviour {

	// Use this for initialization
	void Start () {
		audio.Play();
		audio.volume = 0f;	
	}
	
	// Update is called once per frame
	void Update () {
		if (audio.volume >= 1f)
		{
			audio.volume = 1f;
		}
		else
		{
			audio.volume += Time.deltaTime * 0.5f;
		}
	}
}
