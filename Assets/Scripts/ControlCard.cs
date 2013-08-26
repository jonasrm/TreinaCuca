using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum StateGame { FREE, WAIT }

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	public static StateGame gameState = StateGame.FREE;
	private const float START_TIMER = 1f;
	private static float timer;
	
	public GameObject deckPrefab;
	public GameObject cardPrefab;
	public List<Material> materialsCardLine1;
	public List<Material> materialsDeck;
	
	//public GameObject[] prefab;
	public static int flippedCards = 0;
	public static GameObject card1, card2;
	private int[] rdn = new int[] {0,0,0,0,0,0};
	
	#endregion
	
	#region Methods
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 50), "FP: " + flippedCards);	
	}
	
	void Start()
	{
		timer = START_TIMER;
		float marginLeft = -4.2f, index = 1.2f;

		Object[] auxMaterialsCard = Resources.LoadAll("CardMaterials", typeof(Material));
		foreach (Material item in auxMaterialsCard) {
			materialsCardLine1.Add(item);
		}
		Object[] auxMaterialsDeck = Resources.LoadAll("DeckMaterials", typeof(Material));
		foreach (Material item in auxMaterialsDeck) {
			materialsDeck.Add(item);	
		}
		
		do 
		{
			int r = Random.Range(0, materialsCardLine1.Count);
			materialsDeck.RemoveAt(r);
			materialsCardLine1.RemoveAt(r);
		} 
		while (materialsCardLine1.Count > 6);
		
		// Copia das cartas da linha 1.
		List<Material> materialsCardLine2 = materialsCardLine1;
		
		// SHUFFLE
		materialsDeck = Shuffle(materialsDeck);
		materialsCardLine1 = Shuffle(materialsCardLine1);
		materialsCardLine2 = Shuffle(materialsCardLine2);
		
		// Posicionamento.
		for (int i = 1; i < materialsDeck.Count + 1; i++) 
		{
			GameObject newDeck = (GameObject)Instantiate(deckPrefab, new Vector3(marginLeft + index * i, 2.5f, 0f), Quaternion.identity);	
			newDeck.renderer.material = materialsDeck[i - 1];
			newDeck.name = materialsDeck[i - 1].name;
		}

		for (int i = 1; i < materialsCardLine1.Count + 1; i++) 
		{
			GameObject newCard = (GameObject)Instantiate(cardPrefab, new Vector3(marginLeft + index * i, .2f, 0f), Quaternion.identity);	
			newCard.renderer.material = materialsCardLine1[i - 1];
			newCard.name = materialsCardLine1[i - 1].name;
		}
		for (int i = 1; i < materialsCardLine2.Count + 1; i++) 
		{
			GameObject newCard = (GameObject)Instantiate(cardPrefab, new Vector3(marginLeft + index * i, -1.4f, 0f), Quaternion.identity);	
			newCard.renderer.material = materialsCardLine2[i - 1];
			newCard.name = materialsCardLine2[i - 1].name;
		}
	}
	
	private System.Random _random = new System.Random();
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
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("Menu");
		}
		
		if (gameState == StateGame.WAIT)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				gameState = StateGame.FREE;
				flippedCards = 0;
				timer = START_TIMER;
			}
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
				card2.GetComponent<CardMove>().stateCard = StateCard.DRAG_AND_DROP;
			}
			else
			{
				card1.GetComponent<CardMove>().flipping();
				card2.GetComponent<CardMove>().flipping();
			}
		}
	}
	
	public int randomCards()
	{
		bool generate = true;
		int i = 0;
		while (generate)
		{
			i = Random.Range(0,6);
			if (rdn[i] < 2)
			{
				rdn[i]++;
				generate = false;
			}
		}
		return i;
	}
	
	#endregion	
}
