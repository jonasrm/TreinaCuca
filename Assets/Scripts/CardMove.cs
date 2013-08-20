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
	public FlipCard flipCard = FlipCard.FRONT;
	
	public enum StateCard { FLIP, DRAG_AND_DROP, BLOCK }
	public StateCard stateCard = StateCard.FLIP;

	private Vector3 screenPoint;
    private Vector3 offset;
	
	#endregion
	
	#region Methods

	void Start ()
	{
		//iTween.RotateBy(gameObject, iTween.Hash("y", 1, "easeType", "easeInOutBack", "delay", .4, "onComplete", "ShakeComplete"));
	}
	
	void Update()
	{
		//TODO
	}
	
	void OnMouseDown() 
	{
		
		if (stateCard == StateCard.FLIP)
		{
			stateCard = StateCard.BLOCK;
			if (flipCard == FlipCard.BACK) {
				flipCard = FlipCard.FRONT;
				iTween.RotateBy(gameObject, iTween.Hash("y", -.5f, "easeType", "easeInOutBack", "delay", .4, "onComplete", "setStateCardFlip"));
				control.GetComponent<ControlCard>().cardFlip(gameObject);
			}
			else if (flipCard == FlipCard.FRONT) {
				flipCard = FlipCard.BACK;
				iTween.RotateBy(gameObject, iTween.Hash("y", .5f, "easeType", "easeInOutBack", "delay", .4, "onComplete", "setStateCardFlip"));
				control.GetComponent<ControlCard>().cardFlip(gameObject);
			}
		}
		
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)); 
		
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

	}
	
	void setStateCardFlip()
	{
		stateCard = StateCard.FLIP;
	}

	void setStateCardDragDrod()
	{
		stateCard = StateCard.DRAG_AND_DROP;
	}

	#endregion

}
