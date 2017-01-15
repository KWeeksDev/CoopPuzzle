using UnityEngine;
using UnityEngine.Networking;

public class Carriable : Entity
{
	[SyncVar]
	protected bool mIsCarried = false;
	[SyncVar]
	private NetworkInstanceId mCarrierId;

	public void SetIsCarried(bool carried){ mIsCarried = carried; }
	public bool GetIsCarried(){ return mIsCarried; }

	public void SetCarrierId(NetworkInstanceId id){ mCarrierId = id; }
	public NetworkInstanceId GetCarrierId(){ return mCarrierId; }

}
