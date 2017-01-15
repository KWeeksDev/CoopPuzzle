using UnityEngine;
using System.Collections;
/**
* Switch toggling between two different objects being on or off
*/
public class TwoObjectSwitch : Switch
{
	[SerializeField]
	private InteractiveEntity secondTarget;

	// Default state hase the first target active
	void Start()
	{
		secondTarget.GetComponent<InteractiveEntity>().Activate();
		ChangeDisplay(isActive);
	}

	// Turn off the second target then the first on
	public override bool Activate()
	{
		// Deactive the first and activate the second
		target.GetComponent<InteractiveEntity>().Activate();
		secondTarget.GetComponent<InteractiveEntity>().Deactivate();

		return base.Activate();
	}

	// Turn off the first target then the second on
	public override bool Deactivate()
	{
		// Deactive the first and activate the second
		target.GetComponent<InteractiveEntity>().Deactivate();
		secondTarget.GetComponent<InteractiveEntity>().Activate();

		return base.Deactivate();
	}

}
