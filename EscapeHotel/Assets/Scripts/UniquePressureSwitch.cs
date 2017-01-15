using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
/* Switch that is active while a specific object is set on top of it. */
public class UniquePressureSwitch : Switch {

	[SerializeField]
	protected GameObject uniqueObject;

	private float hoverDistance = .75f;

	[Command]
	void CmdPlayerDropObject(GameObject obj)
	{
		GameObject carrier = NetworkServer.FindLocalObject(obj.GetComponent<Carriable>().GetCarrierId());
		if (carrier.GetComponent<PlayerInteract>())
		{
			carrier.GetComponent<PlayerInteract>().CmdDrop();
		}
	}

	[Command]
	void CmdFixObjectInPlace()
	{
		GameObject serverObject = NetworkServer.FindLocalObject(uniqueObject.gameObject.GetComponent<NetworkIdentity>().netId);
		serverObject.transform.position = (this.transform.position + (this.transform.up * hoverDistance));
	}

	// Use this for initialization
	void Start ()
	{
		ChangeDisplay(isActive);
	}
	
	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.GetComponent<Carriable>() &&
			collision.gameObject == uniqueObject)
		{
			// If the object is being placed have the player drop it into place
			CmdPlayerDropObject(collision.gameObject);
			//CmdPlayerDropObject(uniqueObject);
			uniqueObject.GetComponent<Rigidbody>().useGravity = false;
			// Lock the object in place on the switch
			CmdFixObjectInPlace();
			Activate();
		}
	}

	public void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject == uniqueObject)
		{
			Deactivate();
		}
	}

	public override void PlayerInteract()
	{
		// Do nothing since we don't want the player to click this 
	}
}
