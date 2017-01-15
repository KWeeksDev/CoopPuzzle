using UnityEngine;
using System.Collections;


public class Switch : InteractiveEntity
{
	[SerializeField]
	protected Renderer activeDisplay;
	[SerializeField]
	protected Material onMaterial;
	[SerializeField]
	protected Material offMaterial;
	[SerializeField]
	protected InteractiveEntity target;

	void Start()
	{
		ChangeDisplay(isActive);
	}

	public override bool Activate()
	{
		base.Activate();
		return target.GetComponent<InteractiveEntity>().Activate();
	}

	public override bool Deactivate()
	{
		base.Deactivate();
		return target.GetComponent<InteractiveEntity>().Deactivate(); 
	}

	protected override void OnActiveChange(bool activeChange)
	{
		ChangeDisplay(activeChange);
	}
	
	protected void ChangeDisplay(bool active)
	{
		if (active)
		{
			activeDisplay.material = onMaterial;
		}
		else
		{
			activeDisplay.material = offMaterial;
		}
	}
}
