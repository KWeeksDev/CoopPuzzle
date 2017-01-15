using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public abstract class InteractiveEntity : Entity
{
	[SyncVar(hook="OnActiveChange")]
	protected bool isActive = false;

	public bool GetIsActive() { return isActive; }

	public virtual bool Activate()
	{
		isActive = true;
		return isActive;
	}

	public virtual bool Deactivate()
	{
		isActive = false;
		return isActive;
	}

	protected abstract void OnActiveChange(bool change);

	public virtual void PlayerInteract()
	{
		if (isActive)
		{
			Deactivate();
		}
		else
		{
			Activate();
		}
	}
}
