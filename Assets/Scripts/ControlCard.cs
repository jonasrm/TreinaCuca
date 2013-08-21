using UnityEngine;
using System.Collections;

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	public GameObject[] prefab;
	public static bool inGame = true;
	private static int flippedCards = 0;
	private static GameObject card1, card2;
	private int[] rdn = new int[] {0,0,0,0,0,0};
	//private bool clickable = true;
	//private float timer = 1.1f;
	
	#endregion
	
	#region Methods
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 50), "FP: " + flippedCards);	
	}
	
	void Start()
	{
		float marginLeft = -4.2f, index = 1.2f, y1 = .2f, y2 = -1.4f;

		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index, y1, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 2, y1, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 3, y1, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 4, y1, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 5, y1, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 6, y1, 0f), Quaternion.identity);
		//
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index, y2, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 2, y2, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 3, y2, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 4, y2, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 5, y2, 0f), Quaternion.identity);
		Instantiate(prefab[randomCards()], new Vector3(marginLeft + index * 6, y2, 0f), Quaternion.identity);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.LoadLevel("Menu");
		}
	}
	
	public static void cardFlip(GameObject card)
	{
		if (flippedCards == 0)
		{
			flippedCards++;
			card1 = card;
		}
		else if (flippedCards == 1)
		{
			if (card.GetComponent<CardMove>().flipCard == CardMove.FlipCard.FRONT)
			{
				flippedCards++;
				card2 = card;
				//inGame = false;
				if ( card1.name == card2.name )
				{
					//TODO - move
					card1.GetComponent<CardMove>().moveFrom(card2.transform.position);
					card2.GetComponent<CardMove>().stateCard = CardMove.StateCard.DRAG_AND_DROP;
					flippedCards = 0;
					//inGame = true;
				}
				else
				{
					card1.GetComponent<CardMove>().flipping();
					card2.GetComponent<CardMove>().flipping();
					flippedCards = 0;
					//inGame = true;
				}
			}
			else
			{
				flippedCards--;
				card1 = null;
				//inGame = true;
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
