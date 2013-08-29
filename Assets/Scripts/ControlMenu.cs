using UnityEngine;
using System.Collections;

public class ControlMenu : MonoBehaviour {
	
	#region Fields
	
	public AudioClip sound_click;
	public GUIStyle style1;
	public GUIStyle style2;
	
	#endregion
	
	#region Methods
	
	// OnGUI
	void OnGUI () {
		if(GUI.Button(new Rect(70, 260, 245, 75), "", style1)){
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
