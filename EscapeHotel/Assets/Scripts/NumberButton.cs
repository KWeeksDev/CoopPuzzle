using UnityEngine;

/* A switch that is active as while it is pushed creating a one time signal */
public class NumberButton : Switch
{

	[SerializeField]
	private char buttonValue;

	private NumberPad targetPad;

	void Start()
	{
		ChangeDisplay(false);
		target = gameObject.transform.parent.gameObject.GetComponent<InteractiveEntity>();
		targetPad = target.GetComponent<NumberPad>();
	}

	public override bool Activate()
	{
		if (targetPad != null)
		{
			targetPad.GetButtonValue(buttonValue);
		}
		return false;
	}





}
