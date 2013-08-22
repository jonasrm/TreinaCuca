using UnityEngine;
using System.Collections;

public enum StateGame { FREE, WAIT }

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	public static StateGame gameState = StateGame.FREE;
	private const float START_TIMER = 1f;
	private static float timer;
	
	public GameObject[] prefab;
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
		
		float marginLeft = -4.2f, index = 1.2f, y;

		GameObject prefabDeck = (GameObject)Resources.Load("Deck");
		Object[] materialsDeck = Resources.LoadAll("Materials", typeof(Material));
		for (int i = 1; i < 7; i++) {
			y = .2f;
			Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * i, y, 0f), Quaternion.identity);
			y = -1.4f;
			Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * i, y, 0f), Quaternion.identity);
			y = 2.5f;
			GameObject newDeck = (GameObject)Instantiate(prefabDeck, new Vector3(marginLeft + index * i, y, 0f), Quaternion.identity);	
			newDeck.name = "Deck" + i;
			newDeck.renderer.material = (Material)materialsDeck[i - 1];
		}
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
		while (generate) {
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
