using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour 
{
	#region Fields
	
	public enum FlipCard { FRONT, BACK }
	public FlipCard flipCard = FlipCard.FRONT;
	
	public enum StateCard { FLIP, DRAG_AND_DROP }
	public StateCard cardState = StateCard.FLIP;
	
	public bool mouseDown = false;
	public Vector3 dragStartPosition;
	
	public bool overDeck = false;
	public Vector3 startPos;
	
	private GameObject deck;
	
	#endregion
	
	#region Methods
	
	void Start()
	{
		startPos = transform.position;
		transform.position = startPos;
	}
	
	void Update()
	{
		if(Input.GetButtonUp("Fire1"))
		{
			mouseDown = false;
			if (overDeck)
			{
				deck.renderer.material.color = Color.red;
				Destroy(gameObject);
			}
		}
		
		if (mouseDown)
		{
			//TODO:pegar a diferença
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			
			float x = mousePos.x - dragStartPosition.x;
			float y = mousePos.y - dragStartPosition.y;
			
			transform.Translate(new Vector3(x, y, 0f));
		}
		else if (cardState == StateCard.DRAG_AND_DROP)
		{
			transform.position = startPos;
		}
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

//	void OnTriggerEnter(Collider collider)
//	{
//		Debug.Log("TriggerEnter");
//		if (collider.tag == "Deck")
//		{
//			Debug.Log("MouseDown");
//			Destroy(gameObject);//UNDONE
//		}
//	}

	public FlipCard InOut()
	{
		if (flipCard == FlipCard.FRONT)
		{
			flipCard = FlipCard.BACK;
			iTween.RotateBy(gameObject, 
				iTween.Hash("y", .50, "easeType", "easeInOutBack", "delay", .1));
		}
		else
		{
			flipCard = FlipCard.FRONT;
			iTween.RotateBy(gameObject, 
				iTween.Hash("y", -.50, "easeType", "easeInOutBack", "delay", .1));
		}
		return flipCard;
	}
	
	public void MoveTo(GameObject otherCard)
	{
		transform.position = Vector3.Slerp(transform.position, 
										   otherCard.transform.position, 
										   5f * Time.deltaTime);
	}
	
	#endregion
}
