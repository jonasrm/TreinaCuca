using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	#region Fields
	
	public AudioClip sound_click;
	public AudioClip sound_loop;
	private float posicaoX, posicaoY;	
	public GUIStyle style1;
	public GUIStyle style2;
	private bool fadeOutSound = false;
	
	#endregion
	
	#region Methods
	
	// Use this for initialization
	void Start () {
		posicaoX = Screen.width/2;
		posicaoY = Screen.height/2;
		audio.Play();
		audio.volume = 0f;
	}
	
	// OnGUI
	void OnGUI () {
		//Debug.Log(style1.normal.background.width);
		//Debug.Log(style1.normal.background.height);
		if(GUI.Button(new Rect(70, 260, 245, 75), "", style1)){
			fadeOutSound = true;
			audio.PlayOneShot(sound_click);
			AutoFade.LoadLevel("PlayGame", 1f, 1f, Color.black);
			//Application.LoadLevel("PlayGame");
		}
		if(GUI.Button(new Rect(70, 340, 245, 75), "", style2)){
			audio.PlayOneShot(sound_click);
			//AutoFade.LoadLevel("Tutorial", 1.5f, 1.5f, Color.black);
		}
	}
	
	void Update()
	{
		if(fadeOutSound)
		{
			audio.volume -= Time.deltaTime * 1f;
		}
		else
		{
			if (audio.volume >= 1f)
			{
				audio.volume = 1f;
				Debug.Log(audio.volume);
			}
			else
			{
				audio.volume += Time.deltaTime * 0.5f;
				Debug.Log(audio.volume);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	#endregion

}
