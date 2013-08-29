using UnityEngine;
using System.Collections;

public enum FlipCard { FRONT, BACK }
public enum StateCard { FLIP, DRAG_AND_DROP, DRAGGED, BLOCK }

public class CardMove : MonoBehaviour
{
	#region Fields

	public AudioClip audioClick, audioFlip, audioMove, audioError, audioPow;
	public FlipCard flipCard = FlipCard.BACK;
	public StateCard stateCard = StateCard.FLIP;
	private Vector3 screenPoint;
    private Vector3 offset;
	private bool overDeck = false;
	private Vector3 startPosition;
	private float upEffect = .5f;
	private float timeUpEffect = .2f;
	private float timeRotate = .5f;
		
	#endregion
	
	#region Methods

	void Start ()
	{
		startPosition = transform.position;
	}
	
	void OnMouseDown() 
	{
		if (ControlCard.flippedCards < 2) 
		{
			if (stateCard == StateCard.FLIP && flipCard == FlipCard.BACK)
			{
				iTween.Stab(gameObject, audioClick, 0);
				flipping();
			}
			else if (stateCard == StateCard.DRAG_AND_DROP)
			{
				screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				stateCard = StateCard.DRAGGED;
			}
			
		}
	}
	
	void OnMouseDrag()
	{
		if (stateCard == StateCard.DRAGGED)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z-upEffect);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}
	
	void OnMouseUp() 
	{
		if (stateCard == StateCard.DRAGGED)
		{
			if (overDeck)
			{
				//TODO - o elemento morre...
				iTween.Stab(gameObject, audioError, timeUpEffect);
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
			
			iTween.Stab(gameObject, audioError, timeUpEffect);
			iTween.ShakePosition(gameObject, iTween.Hash("y", .1f, "x", .1f, "z", .1f, "time", .1f, "delay", timeUpEffect, "onComplete", "setGameStateFree"));
			
		}
	}
	
	public void moveFrom(GameObject c)
	{
		float x, y;		
		x = c.transform.position.x - transform.position.x;
		y = transform.position.y - c.transform.position.y;		
		
		//Up two cards
		iTween.MoveBy(c, iTween.Hash("z", -upEffect, "time", timeUpEffect));
		iTween.MoveBy(gameObject, iTween.Hash("z", -upEffect, "time", timeUpEffect));
		
		//Move card and play sound
		iTween.Stab(gameObject, audioMove, timeUpEffect);
		iTween.MoveBy(c, iTween.Hash("x", x, "y", y, "easeType", "easeInOutExpo", "time", timeUpEffect, "delay", timeUpEffect, "onComplete", "selfDestruction", "onCompleteParams", c));
		
		iTween.Stab(gameObject, audioPow, timeUpEffect*2); //UNDONE
		iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("y", .1f, "x", .1f, "z", .1f, "time", .5f, "delay", (timeUpEffect+timeUpEffect), "onComplete", "setGameStateFree"));

	}
	
	public void moveTo(Vector3 pos)
	{
		float x, y;
		x = transform.position.x - pos.x;
		y = pos.y - transform.position.y;
		
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "z", -upEffect, "easeType", "easeInOutExpo", "delay", .1, "onComplete", "setStateCardDragAndDrop"));
	}
	
	void setGameStateFree()
	{
		ControlCard.gameState = StateGame.FREE;
		ControlCard.flippedCards = 0;		
	}
	
	void selfDestruction(GameObject o)
	{
		ControlCard.flippedCards = 0;
		Destroy(o);
	}
	
	void setStateCardDragAndDrop()
	{
		stateCard = StateCard.DRAG_AND_DROP;
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
