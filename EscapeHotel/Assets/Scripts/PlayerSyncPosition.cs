using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour
{
	[SyncVar]
	private Vector3 syncPosition;

	[SerializeField]
	private Transform myTransform;
	[SerializeField]
	private float lerpRate = 5f;

	private Vector3 lastPos;
	private float threshold = 0.5f;
	
	[Command]
	void CmdProvidePosition(Vector3 pos)
	{
		syncPosition = pos;
	}

	[Client]
	void TransmitPosition()
	{
		if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold) 
		{
			
			CmdProvidePosition(myTransform.position);

			lastPos = myTransform.position;
		}
	}

	void Update ()
	{
		LerpPosition();
	}

	void FixedUpdate()
	{
		TransmitPosition();
	}

	void LerpPosition()
	{
		if(!isLocalPlayer)
		{
			myTransform.position = Vector3.Lerp(myTransform.position, syncPosition, Time.deltaTime * lerpRate);
		}
	}


}
