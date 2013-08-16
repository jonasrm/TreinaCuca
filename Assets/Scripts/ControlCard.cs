using UnityEngine;
using System.Collections;

public class ControlCard : MonoBehaviour
{
	#region Fields
	
	#endregion
	
	#region Methods
	
	void Update () 
	{
		RaycastHit hit;
		// clique p/ FLIP
	    if(Input.GetButtonDown("Fire1"))
	    {
	        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if(Physics.Raycast(ray, out hit))
			{
	            if (hit.transform.tag == "carta")
				{
					if (hit.transform.gameObject.GetComponent<Card>().cardState == Card.StateCard.FLIP)
					{
						hit.transform.gameObject.GetComponent<Card>().InOut();
					}
					else
					{
						hit.transform.gameObject.GetComponent<Card>().mouseDown = true;
					}
				}
			}
	    }
		
	}
	
	#endregion
}
