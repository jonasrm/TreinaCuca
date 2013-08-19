using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	#region Fields
	
	public GUISkin skin;
	public AudioClip sound;
	private float posicaoX, posicaoY;
	
	#endregion
	
	#region Methods
	
	// Use this for initialization
	void Start () {
		posicaoX = Screen.width/2;
		posicaoY = Screen.height/2;
	}
	
	// OnGUI
	void OnGUI () {
		GUI.skin = skin;
		GUI.Box(new Rect(posicaoX-250, 50, 500, 100), "Treina Cuca");
		if(GUI.Button(new Rect(posicaoX-100, posicaoY-20, 200, 40), "Jogar")){
			audio.PlayOneShot(sound);
		}
		if(GUI.Button(new Rect(posicaoX-100, posicaoY+30, 200, 40), "Tutorial")){
			audio.PlayOneShot(sound);
		}
		if(GUI.Button(new Rect(posicaoX-100, posicaoY+80, 200, 40), "Creditos")) {
			audio.PlayOneShot(sound);
			Application.LoadLevel("creditos");
		}
		if(GUI.Button(new Rect(posicaoX-100, posicaoY+130, 200, 40), "Sair")) {
			audio.PlayOneShot(sound);
			Application.Quit();
		}
	}
	
	#endregion

}
