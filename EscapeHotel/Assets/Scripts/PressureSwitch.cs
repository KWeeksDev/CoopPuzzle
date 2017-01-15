using UnityEngine;
using System.Collections;
/* Switch that is active while a carriable objcet or the player is on top of it */
public class PressureSwitch : Switch
{
	protected ArrayList overlappingObjects;


	void Start()
	{
		overlappingObjects = new ArrayList();
		ChangeDisplay(isActive);
	}

	public void OnCollisionEnter(Collision collision)
	{
		bool canActivate = true;
		// If not we only want basic carried objects to hold down the button
		if (collision.gameObject.GetComponent<Carriable>() == false)
		{
			canActivate = false;
		}
		
		if (canActivate)
		{
			overlappingObjects.Add(collision.gameObject);
			if (isActive == false)
			{
				Activate();
			}
		}
	}

	public void OnTriggerEnter(Collider col)
	{
		bool canActivate = true;
		// If not we only want basic carried objects to hold down the button
		if (col.gameObject.GetComponent<CharacterController>() == false)
		{
			canActivate = false;
		}

		if (canActivate)
		{
			overlappingObjects.Add(col.gameObject);
			if (isActive == false)
			{
				Activate();
			}
		}
	}

	public void OnCollisionExit(Collision collision)
	{
		bool canDeactivate = true;
		// If not we only want basic carried objects to hold down the button
		if (collision.gameObject.GetComponent<Carriable>() == false)
		{
			canDeactivate = false;
		}

		if (canDeactivate)
		{
			if (overlappingObjects.Contains(collision.gameObject))
			{
				overlappingObjects.Remove(collision.gameObject);
				if (overlappingObjects.Count == 0)
				{
					Deactivate();
				}
			}
		}
	}

	public void OnTriggerExit(Collider col)
	{
		bool canDeactivate = true;

		if (col.gameObject.GetComponent<CharacterController>() == false)
		{
			canDeactivate = false;
		}

		if (canDeactivate)
		{
			if (overlappingObjects.Contains(col.gameObject))
			{
				overlappingObjects.Remove(col.gameObject);
				if (overlappingObjects.Count == 0)
				{
					Deactivate();
				}
			}
		}
	}

	public override void PlayerInteract()
	{
		// Do nothing since we don't want the player to click this 
	}
}
