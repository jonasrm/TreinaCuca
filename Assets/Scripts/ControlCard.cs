using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum StateGame { FREE, WAIT }

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	public GUISkin skin;
	public AudioClip audioWin, audioLose, audioPause;
	public static int countCardFinished = 0;
	public static StateGame gameState = StateGame.FREE;
	public GameObject deckPrefab;
	public GameObject cardPrefab;
	public GameObject progress_bar;
	public GameObject progress_bar_base;
	public List<Material> materialsCardLine1;
	public List<Material> materialsDeck;	
	public static int flippedCards = 0;
	public static GameObject card1, card2;
	private System.Random _random = new System.Random();
	private float countdown;
	private const float START_COUNTDOWN = 100;
	private float positionX, positionY;
	private string popupText = "";

	#endregion
	
	#region Methods
	
	void OnGUI()
	{
		if (gameState == StateGame.WAIT)
		{
			Time.timeScale = 0;
			int w = 300, h = 100;
			GUI.skin = skin;
			
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
			
			GUI.Label(new Rect(positionX - w/2, positionY - h/2 - 50, w, h), popupText);
			
			if (GUI.Button(new Rect(positionX - w/2, positionY - h/2 + 60, w/2 - 10, h-30), "Reiniciar"))
			{
				iTween.Stab(gameObject, audioPause, 0);
				ManagerLevel.LoadPlayGame();
				enabled = false;
			}
			if (GUI.Button(new Rect(positionX - w/2 + w/2 + 10, positionY - h/2 + 60, w/2 -10, h-30), "Menu"))
			{
				iTween.Stab(gameObject, audioPause, 0);
				ManagerLevel.LoadMenu();
				enabled = false;
			}		
		}
	}
	
	void Awake()
	{
		Time.timeScale = 1;
		
		positionX = Screen.width / 2;
		positionY = Screen.height / 2;
		countdown = START_COUNTDOWN;
		countCardFinished = 0;
		flippedCards = 0;
		gameState = StateGame.FREE;
		card1 = null;
		card2 = null;
		progress_bar.transform.position = new Vector3(progress_bar_base.transform.position.x,
													  progress_bar_base.transform.position.y,
													  progress_bar_base.transform.position.z + 0.1f);
	}
	
	void Start()
	{
		Object[] auxMaterialsCard = Resources.LoadAll("CardMaterials", typeof(Material));
		foreach (Material item in auxMaterialsCard)
		{
			materialsCardLine1.Add(item);
		}
		Object[] auxMaterialsDeck = Resources.LoadAll("DeckMaterials", typeof(Material));
		foreach (Material item in auxMaterialsDeck)
		{
			materialsDeck.Add(item);	
		}
		
		while (materialsCardLine1.Count > 6) {
			int r = Random.Range(0, materialsCardLine1.Count);
			materialsDeck.RemoveAt(r);
			materialsCardLine1.RemoveAt(r);			
		}

		// Copia das cartas da linha 1.
		List<Material> materialsCardLine2 = materialsCardLine1;
		
		// SHUFFLE
		materialsDeck = Shuffle(materialsDeck);		
		materialsCardLine1 = Shuffle(materialsCardLine1);
		materialsCardLine2 = Shuffle(materialsCardLine2);

		// Posicionamento.
		//TODO - melhorar o shuffle
		float marginLeft = -4.7f, index = 1.25f;
		
		for (int i = 1; i < materialsDeck.Count+1; i++)
		{
			GameObject newDeck = (GameObject)Instantiate(deckPrefab, new Vector3(marginLeft + index * i, 1.6f, 0f), Quaternion.identity);	
			newDeck.renderer.material = materialsDeck[i - 1];
			newDeck.name = materialsDeck[i - 1].name;
			newDeck.transform.Rotate(new Vector3(0f, 0f, 180f));
		}
		for (int i = 1; i < materialsCardLine1.Count + 1; i++)
		{
			GameObject newCard = (GameObject)Instantiate(cardPrefab, new Vector3(marginLeft + index * i, -.1f, 0f), Quaternion.identity);	
			newCard.renderer.material = materialsCardLine1[i - 1];
			newCard.name = materialsCardLine1[i - 1].name;
		}
		for (int i = 1; i < materialsCardLine2.Count + 1; i++)
		{
			GameObject newCard = (GameObject)Instantiate(cardPrefab, new Vector3(marginLeft + index * i, -1.7f, 0f), Quaternion.identity);	
			newCard.renderer.material = materialsCardLine2[i - 1];
			newCard.name = materialsCardLine2[i - 1].name;
		}
	}
	
	public List<Material> Shuffle(List<Material> arr)
    {
		
		List<KeyValuePair<int, Material>> list = new List<KeyValuePair<int, Material>>();
		
		foreach (Material s in arr)
		{
		    list.Add(new KeyValuePair<int, Material>(_random.Next(), s));
		}
		
		var sorted = from item in list
			     	orderby item.Key
			     	select item;
		
		Material[] result = new Material[arr.Count];
		
		int index = 0;
		foreach (KeyValuePair<int, Material> pair in sorted)
		{
		    result[index] = pair.Value;
		    index++;
		}
		
		return result.ToList();
    }
	
	void Update()
	{
		countdown -= Time.deltaTime;
		progress_bar.transform.position -=  new Vector3(0, Time.deltaTime/countdown, 0);
		
		float deadLine = -1.1f;
		
		//Finish Game
		if (gameState != StateGame.WAIT)
		{
			if (progress_bar.transform.position.y <= deadLine)
			{
				GameObject go = CreateSound(audioLose);
				Destroy(go, audioLose.length);				
	
				popupText = "Derrota";
				Time.timeScale = 0;
				gameState = StateGame.WAIT;
			}
			else if (countCardFinished >= 6)
			{
				GameObject go = CreateSound(audioWin);
				Destroy(go, audioWin.length);				
	
				popupText = "Vitoria";
				Time.timeScale = 0;
				gameState = StateGame.WAIT;
			}
		}

		//Back to menu
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			//AutoFade.LoadLevel("Menu", 1f, 1f, Color.black);
			if (gameState == StateGame.FREE)
			{
				GameObject go = CreateSound(audioPause);
				Destroy(go, audioPause.length);				

				popupText = "Pausado";
				Time.timeScale = 0;
				gameState = StateGame.WAIT;
			}
			else
			{
				GameObject go = CreateSound(audioPause);
				Destroy(go, audioPause.length);				

				Time.timeScale = 1;
				gameState = StateGame.FREE;
			}
		}		
		
		if (progress_bar.transform.position.y <= -0.8)
		{
			progress_bar.guiTexture.color = Color.red;
		}
		else if (progress_bar.transform.position.y <= -0.5)
		{
			progress_bar.guiTexture.color = Color.yellow;
		}
		else
		{
			progress_bar.guiTexture.color = Color.green;
		}
	}
	
	public static void cardFlip()
	{
		if (flippedCards == 2)
		{
			if ( card1.name == card2.name )
			{
				card1.GetComponent<CardMove>().moveFrom(card2);
				card1.GetComponent<CardMove>().stateCard = StateCard.DRAG_AND_DROP;
			}
			else
			{
				card1.GetComponent<CardMove>().flipping();
				card2.GetComponent<CardMove>().flipping();
			}
		}
	}
	
	GameObject CreateSound (AudioClip audioError)
	{
		GameObject go = new GameObject("Sound");
		go.AddComponent<AudioSource>();
		Instantiate(go, Vector3.zero, Quaternion.identity);
		
		AudioSource audio = go.GetComponent<AudioSource>();
		audio.clip = audioError;
		audio.loop = false;
		audio.playOnAwake = false;
		audio.Play();
		return go;
	}
	
	
	#endregion	
}
