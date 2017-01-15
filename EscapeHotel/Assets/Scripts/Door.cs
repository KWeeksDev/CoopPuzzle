using UnityEngine;
using UnityEngine.Networking;

public class Door : InteractiveEntity
{
	// Variables
	public bool isOpen = false;
	private Vector3 doorOffset = new Vector3(0, 4.5f, 0);
	[SerializeField]
	private float lerpRate = 2f;
	[SerializeField]
	private int keysNeeded = 1;
	private int keysUsed = 0;

	[Server]
	void CmdOpenDoor()
	{
		if (!isOpen)
		{
			keysUsed++;
			if (keysUsed == keysNeeded)
			{
				transform.position = Vector3.Lerp(transform.position, transform.position + doorOffset, lerpRate);

				isOpen = true;
				isActive = true;
			}
		}
	}

	[Server]
	void CmdCloseDoor ()
	{
		keysUsed--;
		if (isOpen)
		{
			if (keysUsed < keysNeeded)
			{
				transform.position = Vector3.Lerp(transform.position, transform.position - doorOffset, lerpRate);

				isOpen = false;
				isActive = false;
			}
		}
	}

	public override bool Activate()
	{
		if (!isActive)
		{
			CmdOpenDoor();
		}

		return isActive;
	}

	public override bool Deactivate()
	{
		if (isActive)
		{
			CmdCloseDoor();
		}

		return isActive;
	}

	protected override void OnActiveChange(bool change)
	{
		// Some sound effect
	}
}
