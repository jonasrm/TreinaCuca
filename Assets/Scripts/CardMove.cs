using UnityEngine;
using System.Collections;

public enum FlipCard { FRONT, BACK }
public enum StateCard { FLIP, DRAG_AND_DROP, BLOCK }

public class CardMove : MonoBehaviour
{
	#region Fields
		
	public GameObject control;
	public FlipCard flipCard = FlipCard.BACK;

	public StateCard stateCard = StateCard.FLIP;

	private Vector3 screenPoint;
    private Vector3 offset;
	private bool overDeck = false;
	//private GameObject deck;
	
	private Vector3 startPosition;
	private Material originalMaterial;
	
	//public GameObject effectDrag;
	//private GameObject e;
	
	#endregion
	
	#region Methods

	void Start ()
	{
		startPosition = transform.position;
		originalMaterial = renderer.material;
		
	}
	
	void Update()
	{
		//TODO
	}
	
	void OnMouseDown() 
	{
		if (ControlCard.flippedCards < 2) 
		{
			if (stateCard == StateCard.FLIP && flipCard == FlipCard.BACK)
			{
				flipping();
			}
			else if (stateCard == StateCard.DRAG_AND_DROP)
			{
				screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				//e = (GameObject)Instantiate(effectDrag, transform.position, Quaternion.identity);
				//e.transform.parent = transform;
			}
			
		}
	}
	
	void OnMouseDrag()
	{
		if (stateCard == StateCard.DRAG_AND_DROP)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}
	
	void OnMouseUp() 
	{
		if (stateCard == StateCard.DRAG_AND_DROP)
		{
			if (overDeck)
			{
				//deck.renderer.material = originalMaterial;
				Destroy(gameObject);
			}
			else
			{
				moveTo(startPosition);
			}
		}
	}
	
	public void flipping()
	{
		stateCard = StateCard.BLOCK;
		
		if (flipCard == FlipCard.BACK)
		{
			ControlCard.flippedCards++;
			flipCard = FlipCard.FRONT;
			if (ControlCard.flippedCards == 1)
			{
				ControlCard.card1 = gameObject;
				iTween.RotateBy(gameObject, iTween.Hash("y", .5, "easeType", "easeInOutBack"));
			}
			else if (ControlCard.flippedCards == 2)
			{
				ControlCard.card2 = gameObject;
				iTween.RotateBy(gameObject, iTween.Hash("y", .5, "easeType", "easeInOutBack", "onComplete", "setStateCardFlip"));
			}
		}
		else
		{
			flipCard = FlipCard.BACK;
			iTween.RotateBy(gameObject, iTween.Hash("y", -.5, "easeType", "easeInOutBack", "onComplete", "setStateCardFlip"));
		}
	}
	
	public void moveFrom(Vector3 pos)
	{
		float x, y;
		x = transform.position.x - pos.x;
		y = pos.y - transform.position.y;
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "easeType", "easeInOutExpo", "delay", .1, "onComplete", "selfDestruction"));	
	}
	
	public void moveTo(Vector3 pos)
	{
		float x, y;
		x = transform.position.x - pos.x;
		y = pos.y - transform.position.y;
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "easeType", "easeInOutExpo", "delay", .1));
	}
	
	void selfDestruction()
	{
		ControlCard.flippedCards = 0;
		Destroy(gameObject);
	}
	
	void setStateCardFlip()
	{
		stateCard = StateCard.FLIP;
		ControlCard.cardFlip();
	}

	void setStateCardDragDrod()
	{
		stateCard = StateCard.DRAG_AND_DROP;
	}
	
	#endregion
	
	#region Deck
	
	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "deck")
		{
			if (renderer.material.color == collider.renderer.material.color)
			{
				//deck = collider.gameObject;
				//deck.renderer.material.color = Color.red;
				overDeck = true;
			}
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "deck")
		{
			overDeck = false;
			//deck.renderer.material = originalMaterial;
		}
	}
	
	#endregion

}
