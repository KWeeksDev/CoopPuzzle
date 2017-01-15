using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayerSetup : NetworkBehaviour {

	[SerializeField] Camera FPSCamera;
	[SerializeField] AudioListener FPSListener;
	// Use this for initialization
	void Start ()
	{
		if (isLocalPlayer)
		{
			GetComponent<CharacterController>().enabled = true;
			GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
			GetComponent<PlayerInteract>().enabled = true;
			FPSCamera.enabled = true;
			FPSListener.enabled = true;

		}
	}
	
}
