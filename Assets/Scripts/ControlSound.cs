using UnityEngine;
using System.Collections;

public class ControlSound : MonoBehaviour {
	
	#region Fields
	
	public Material materialSoundOn, materialSoundOff;
	private bool soundOn = true;
	
	#endregion
	
	#region Methods
	
	// Use this for initialization
	void Start () {
		audio.Play();
		audio.volume = 0f;	
		gameObject.renderer.material = materialSoundOn;
		soundOn = true;
	}
	
	void OnMouseDown()
	{
		if (soundOn)
		{
			soundOn = false;
			gameObject.renderer.material = materialSoundOff;
			audio.Stop();
		}
		else
		{
			soundOn = true;
			gameObject.renderer.material = materialSoundOn;	
			audio.Play();
		}
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

	#endregion
	
}
