using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum StateGame { FREE, WAIT }

public class ControlCard : MonoBehaviour
{
	#region Fields

	public static int countCardFinished = 0;
	public static StateGame gameState = StateGame.FREE;
	public GameObject deckPrefab;
	public GameObject cardPrefab;
	public GameObject progress_bar;
	public List<Material> materialsCardLine1;
	public List<Material> materialsDeck;	
	public static int flippedCards = 0;
	public static GameObject card1, card2;
	private System.Random _random = new System.Random();
	private float countdown = 100;

	#endregion
	
	#region Methods
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 50), "FP: " + flippedCards);
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
		if (progress_bar.transform.position.y <= -1.4)
		{
			Debug.Log ("FINISH");
			AutoFade.LoadLevel("Menu", 1f, 1f, Color.red);
		}

		//Back to menu
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			AutoFade.LoadLevel("Menu", 1f, 1f, Color.black);
		}		
	}
	
	public static void cardFlip()
	{
		if (flippedCards == 2)
		{
			gameState = StateGame.WAIT;
			
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

	#endregion	
}
