using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	#region Fields
	
	public AudioClip sound_click;
	public AudioClip sound_loop;
	private float posicaoX, posicaoY;	
	public GUIStyle style1;
	public GUIStyle style2;
	
	#endregion
	
	#region Methods
	
	// Use this for initialization
	void Start () {
		posicaoX = Screen.width/2;
		posicaoY = Screen.height/2;
	}
	
	// OnGUI
	void OnGUI () {
		//Debug.Log(style1.normal.background.width);
		//Debug.Log(style1.normal.background.height);
		if(GUI.Button(new Rect(70, 260, 245, 75), "", style1)){
			//fadeOutSound = true;
			audio.PlayOneShot(sound_click);
			AutoFade.LoadLevel("PlayGame", 1f, 1f, Color.black);
		}
		if(GUI.Button(new Rect(70, 340, 245, 75), "", style2)){
			audio.PlayOneShot(sound_click);
			//AutoFade.LoadLevel("Tutorial", 1.5f, 1.5f, Color.black);
		}
	}
	
	void Update()
	{		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	#endregion

}
