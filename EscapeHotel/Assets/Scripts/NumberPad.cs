using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;

public class NumberPad : Switch
{
	[SyncVar(hook="OnDisplayChanged")]
	private string textString;
	
	public Text textPrefab;
	[SerializeField]
	private Canvas displayCanvas;
	[SerializeField]
	private string combination;
	private Text displayText;
	private List<char> enteredCode = new List<char>();

	[Command]
	private void CmdUpdateDisplay(char value)
	{
		textString += value;
	}

	[Command]
	private void CmdClearDisplay()
	{
		textString = "";
	}

	private void OnDisplayChanged(string text)
	{
		displayText.text = text;
	}

	void Awake()
	{
		displayText = displayCanvas.gameObject.GetComponent<Text>();
	}

	public void GetButtonValue(char value)
	{
		if ('0' <= value && value <= '9')
		{
			if (enteredCode.Count > combination.Length)
			{
				ResetEntry();
			}
			else
			{
				ButtonPushed(value);
			}
		}
		else if (value == 'r')
		{
			ResetEntry();
		}
		else if (value == 'e')
		{
			CheckCombination();
		}
	}

	private void ButtonPushed(char buttonValue)
	{
		enteredCode.Add(buttonValue);
		CmdUpdateDisplay(buttonValue);
	}

	private void ResetEntry()
	{
		enteredCode.Clear();
		CmdClearDisplay();
	}

	private void CheckCombination()
	{
		if (enteredCode.Count > 0)
		{
			char[] combo = combination.ToCharArray();
			bool match = true;
			for (int i = 0; i < combo.Length; i++)
			{
				if (combo[i] != enteredCode[i])
				{
					match = false;
					break;
				}
			}

			if (match)
			{
				isActive = Activate();
			}
			else
			{
				ResetEntry();
			}
		}
	}
}
