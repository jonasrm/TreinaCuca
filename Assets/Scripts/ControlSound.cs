using UnityEngine;
using System.Collections;

public class ControlSound : MonoBehaviour {
	
	public Material materialSoundOn, materialSoundOff;
	private bool soundOn = true;
	private AudioSource audioBackground;
	
	// Use this for initialization
	void Start () {
		gameObject.renderer.material = materialSoundOn;
		soundOn = true;
	}
	
	void OnMouseDown()
	{
		if (soundOn)
		{
			soundOn = false;
			gameObject.renderer.material = materialSoundOff;
			//audioBackground = GameObject.Find("Background").transform.GetComponent(AudioSource);
			//audioBackground = GameObject.Find("Background").GetComponent(AudioSource);
			//audioBackground.Stop();
		}
		else
		{
			soundOn = true;
			gameObject.renderer.material = materialSoundOn;	
			audioBackground.Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
