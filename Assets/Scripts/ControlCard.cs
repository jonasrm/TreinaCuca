using UnityEngine;
using System.Collections;

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	private int flippedCards = 0;
	private GameObject card1, card2;
	private bool clickable = true;
	private float timer = 1.1f;
	
	#endregion
	
	#region Methods
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 200, 50), "FP: " + flippedCards);	
	}
	
	void Update ()
	{
		if(clickable)
		{
			RaycastHit hit;
		    if(Input.GetButtonDown("Fire1"))
			{
		        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		        if(Physics.Raycast(ray, out hit))
				{
		            if (hit.transform.tag == "carta")
					{
						if (hit.transform.gameObject.GetComponent<Card>().cardState == Card.StateCard.FLIP)
						{
							//CARD ONE
							if (flippedCards == 0)
							{
								card1 = hit.transform.gameObject;
								if (card1.GetComponent<Card>().InOut() == Card.FlipCard.BACK)
								{
									flippedCards++;
								}
								else
								{
									flippedCards--;
								}
							}
							//CARD TWO
							else if (flippedCards == 1)
							{
								card2 = hit.transform.gameObject;
								if (card2.GetComponent<Card>().InOut() == Card.FlipCard.BACK)
								{
									clickable = false;
									flippedCards++;
								}
								else
								{
									flippedCards--;
								}
							}
	
							if (flippedCards == 2)
							{
								if (card1.name.Substring(card1.name.Length -1, 1) ==
									card2.name.Substring(card2.name.Length -1, 1))
								{
									StartCoroutine(MoveCard (2f));
								}
								else
								{
									StartCoroutine(BackToOriginalPosition ());
								}
							}
						}
						else
						{
							Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
							hit.transform.gameObject.GetComponent<Card>().dragStartPosition = mousePos - hit.transform.position;
							
							hit.transform.gameObject.GetComponent<Card>().mouseDown = true;
						}
					}
				}
		    }
		}
		
		if (!clickable &&
			card1.name.Substring(card1.name.Length -1, 1) == card2.name.Substring(card2.name.Length -1, 1))
		{
			timer -= Time.deltaTime;
			if (timer <= 0)
			{
				card1.GetComponent<Card>().MoveTo(card2);
			}
		}
	}

	IEnumerator MoveCard (float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		
		card2.GetComponent<Card>().cardState = Card.StateCard.DRAG_AND_DROP;
		Destroy(card1);
		
		clickable = true;
		flippedCards = 0;
		
		timer = 1.1f;
	}

	IEnumerator BackToOriginalPosition ()
	{
		card1.transform.gameObject.GetComponent<Card>().InOut();
		card2.transform.gameObject.GetComponent<Card>().InOut();
		
		yield return new WaitForSeconds(1f);
		
		card1 = null;
		card2 = null;
		
		clickable = true;
		flippedCards = 0;
	}
	
	#endregion
}
