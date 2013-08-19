using UnityEngine;
using System.Collections;

public class Creditos : MonoBehaviour {
	
	#region Fields
	
	public GUISkin skin;
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
		if(GUI.Button(new Rect(posicaoX-100, posicaoY+130, 200, 40), "Voltar")) {
			Application.LoadLevel("menu");
		}
	}
	
	#endregion

}
