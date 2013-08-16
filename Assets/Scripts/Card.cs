using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour 
{
	#region Fields
	
	public enum FlipCard { FRONT, BACK }
	public FlipCard flipCard = FlipCard.FRONT;
	
	public enum StateCard { FLIP, DRAG_AND_DROP }
	public StateCard cardState = StateCard.DRAG_AND_DROP;//TODO
	
	public bool mouseDown = false;
	public Vector3 startPos;
	
	#endregion
	
	#region Methods
	
	void Start()
	{
		startPos = transform.position;
	}
	
	void Update()
	{
		if(Input.GetButtonUp("Fire1"))
		{
			mouseDown = false;
		}
		
		if (mouseDown)
		{
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			transform.position = new Vector3(mousePos.x, mousePos.y, 0f);
		}
		else
		{
			 transform.position = startPos;
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "deck")
		{
			Destroy(gameObject);
		}
	}
	
	public void InOut()
	{
		if (flipCard == FlipCard.FRONT) 
		{
			iTween.RotateBy(gameObject, 
				iTween.Hash("y", .50, "easeType", "easeInOutBack", "delay", .1));
		}
		else
		{
			iTween.RotateBy(gameObject, 
				iTween.Hash("y", -.50, "easeType", "easeInOutBack", "delay", .1));
		}
	}
	
	#endregion
}
