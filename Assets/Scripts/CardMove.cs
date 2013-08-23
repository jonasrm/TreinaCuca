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
	
	private Vector3 startPosition;
	private float upEffect = 2f;
	private float timeUpEffect = 1f;
	private float timeRotate = 1f;
		
	#endregion
	
	#region Methods

	void Start ()
	{
		startPosition = transform.position;
		
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
			}
			
		}
	}
	
	void OnMouseDrag()
	{
		if (stateCard == StateCard.DRAG_AND_DROP)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z-upEffect);
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
				iTween.MoveBy(gameObject, iTween.Hash("z", -upEffect, "time", timeUpEffect));
				iTween.RotateBy(gameObject, iTween.Hash("y", .5, "time", timeRotate, "easeType", "easeInOutBack", "delay", timeUpEffect));
			}
			else if (ControlCard.flippedCards == 2)
			{
				ControlCard.card2 = gameObject;
				iTween.MoveBy(gameObject, iTween.Hash("z", -upEffect, "time", timeUpEffect));
				iTween.RotateBy(gameObject, iTween.Hash("y", .5, "time", timeRotate, "easeType", "easeInOutBack", "delay", timeUpEffect, "onComplete", "setStateCardFlip"));
			}
		}
		else
		{
			flipCard = FlipCard.BACK;
			iTween.MoveBy(gameObject, iTween.Hash("z", -upEffect, "time", timeUpEffect));
			iTween.RotateBy(gameObject, iTween.Hash("y", -.5, "time", timeRotate/2, "easeType", "easeInOutBack", "delay", timeUpEffect, "onComplete", "setStateCardFlip"));
			iTween.ShakePosition(gameObject, iTween.Hash("y", .1f, "x", .1f, "z", .1f, "time", .1f, "delay", timeUpEffect));
		}
	}
	
	public void moveFrom(GameObject c)
	{
		float x, y;
		x = transform.position.x - c.transform.position.x;
		y = c.transform.position.y - transform.position.y;		
		iTween.MoveBy(c, iTween.Hash("z", -upEffect, "time", timeUpEffect));
		iTween.MoveBy(gameObject, iTween.Hash("z", -upEffect, "time", timeUpEffect));
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "easeType", "easeInOutExpo", "delay", timeUpEffect, "onComplete", "selfDestruction"));
		iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("y", .1f, "x", .1f, "z", .1f, "time", .5f, "delay", timeUpEffect*2));
	}
	
	public void moveTo(Vector3 pos)
	{
		float x, y;
		x = transform.position.x - pos.x;
		y = pos.y - transform.position.y;
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "z", -upEffect, "easeType", "easeInOutExpo", "delay", .1));
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
			if (gameObject.name == collider.gameObject.name)
			{
				overDeck = true;
			}
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "deck")
		{
			overDeck = false;
		}
	}
	
	#endregion

}
