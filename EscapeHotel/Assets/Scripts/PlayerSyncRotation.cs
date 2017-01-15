using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncRotation : NetworkBehaviour
{
	[SyncVar]
	private Quaternion syncPlayerRotation;
	[SyncVar]
	private Quaternion syncCameraRotation;

	[SerializeField]
	private Transform playerTransform;
	[SerializeField]
	private Transform cameraTransform;
	[SerializeField]
	private float lerpRate = 5f;

	private Quaternion lastCameraRot;
	private Quaternion lastPlayerRot;
	private float threshold = 5f;
	
	[Command]
	void CmdProvideRotations(Quaternion playerRot, Quaternion cameraRot)
	{
		syncPlayerRotation = playerRot;
		syncCameraRotation = cameraRot;
	}

	[Client]
	void TransmitRotations()
	{
		if(isLocalPlayer)
        {
			if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold || Quaternion.Angle(cameraTransform.rotation, lastCameraRot) > threshold)
			{
				CmdProvideRotations(playerTransform.rotation, cameraTransform.rotation);

				lastPlayerRot = playerTransform.rotation;
				lastCameraRot = cameraTransform.rotation;
			}
		}
	}

	void Update ()
	{
		LerpRotations();
	}

	void FixedUpdate()
	{
		TransmitRotations();

	}

	void LerpRotations()
	{
		if(!isLocalPlayer)
		{
			playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayerRotation, Time.deltaTime * lerpRate);
			cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, syncCameraRotation, Time.deltaTime * lerpRate);
		}
	}


}
