using UnityEngine;
using System.Collections;

public class CardMove : MonoBehaviour
{
	
	#region Fields
	
	//Vector3 mMouseDownPos;
	//Vector3 mMouseUpPos; 
	//public float speed = .1f;
	
	public GameObject control;
	
	public enum FlipCard { FRONT, BACK }
	public FlipCard flipCard = FlipCard.BACK;
	
	public enum StateCard { FLIP, DRAG_AND_DROP, BLOCK }
	public StateCard stateCard = StateCard.FLIP;

	private Vector3 screenPoint;
    private Vector3 offset;
	private bool overDeck = false;
	private GameObject deck;
	
	#endregion
	
	#region Methods

	void Start ()
	{

	}
	
	void Update()
	{
		//TODO
	}
	
	void OnMouseDown() 
	{
		
		if (ControlCard.inGame) {
			
			if (stateCard == StateCard.FLIP)
			{
				flipping();
			}
			else if (stateCard == StateCard.DRAG_AND_DROP)
			{
				screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)); 				
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
		if (overDeck)
		{
			deck.renderer.material.color = Color.red;
			Destroy(gameObject);
		}
	}
	
	public void flipping()
	{
		//stateCard = StateCard.BLOCK;
		ControlCard.inGame = false;
		if (flipCard == FlipCard.BACK) {
			flipCard = FlipCard.FRONT;
			iTween.RotateBy(gameObject, iTween.Hash("y", .5, "easeType", "easeInOutBack", "onComplete", "setStateCardFlip"));
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
		ControlCard.inGame = false;
		x = transform.position.x - pos.x;
		y = pos.y - transform.position.y;
		iTween.MoveBy(gameObject, iTween.Hash("x", x, "y", y, "easeType", "easeInOutExpo", "delay", .1, "onComplete", "selfDestruction"));	
		//iTween.RotateBy(gameObject, iTween.Hash("x", 1, "easeType", "easeInOutBack", "delay", .1));
		//stateCard = StateCard.DRAG_AND_DROP;
	}
	
	void selfDestruction()
	{
		Destroy(gameObject);
		ControlCard.inGame = true;
	}
	
	void setStateCardFlip()
	{
		//stateCard = StateCard.FLIP;
		ControlCard.inGame = true;
		ControlCard.cardFlip(gameObject);
	}

	void setStateCardDragDrod()
	{
		stateCard = StateCard.DRAG_AND_DROP;
	}

	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "deck")
		{
			deck = collider.gameObject;
			deck.renderer.material.color = Color.grey;
			overDeck = true;
		}
	}
	
	void OnTriggerExit(Collider collider)
	{
		if (collider.tag == "deck")
		{
			overDeck = false;
			deck.renderer.material.color = Color.red;
		}
	}
	
	#endregion

}
