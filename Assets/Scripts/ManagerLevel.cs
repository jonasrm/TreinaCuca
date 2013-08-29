using UnityEngine;
using System.Collections;

public class ManagerLevel : MonoBehaviour {

	public static void LoadPlayGame()
	{
		Time.timeScale = 1f;
		AutoFade.LoadLevel("PlayGame", 1f, 1f, Color.black);
	}
	
	public static void LoadMenu()
	{
		Time.timeScale = 1f;
		AutoFade.LoadLevel("Menu", 1f, 1f, Color.black);
	}
}
