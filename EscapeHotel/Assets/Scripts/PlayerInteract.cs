using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInteract : NetworkBehaviour
{
	[SyncVar]
	public bool isCarrying = false;
	[SyncVar]
	private GameObject carriedObject;
	[SyncVar]
	private NetworkInstanceId carriedId;
	[SyncVar]
	private float carryDist = 0f;


	public Texture2D defaultTexture;
	public Texture2D interactTexture;
	public bool showReticle = true;
	public Text textPrefeb;

	public float smoothCarry = 1f;
	private bool useInteractiveTexture = false;
	private Rect texturePos;
	private static float playerReach = 4f;
	private static float throwForce = 8.0f;
	private Text interactText;
	public Camera mCamera;


	/* Network Commands*/ 
	[Command]
	void CmdPickUp(GameObject obj)
	{
		isCarrying = true;
		carriedObject = obj.gameObject;
		carriedId = obj.GetComponent<NetworkIdentity>().netId;
		carriedObject.GetComponent<Rigidbody>().useGravity = false;
		carriedObject.GetComponent<Carriable>().SetCarrierId(GetComponent<NetworkIdentity>().netId);
		// Adjust the arm length so we're not pushing the object through the floor

		carryDist = Vector3.Distance(carriedObject.transform.position, mCamera.transform.position);

	}
	[Command]
	public void CmdDrop()
	{
		isCarrying = false;
		if (carriedObject != null)
		{
			carriedObject.GetComponent<Rigidbody>().useGravity = true;
			carriedObject = null;
		}
		carryDist = 0f;
	}
	[Command]
	void CmdThrow()
	{
		isCarrying = false;
		if (carriedObject != null)
		{
			if (carriedObject.GetComponent<Carriable>())
			{
				carriedObject.GetComponent<Rigidbody>().AddForce(mCamera.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);
			}

			carriedObject.GetComponent<Rigidbody>().useGravity = true;
			carriedObject = null;
		}
		carryDist = 0f;
	}
	[Command]
	void CmdCarryObject()
	{
		if (carriedObject == null) return;
		GameObject serverCarry = NetworkServer.FindLocalObject(carriedId);
		serverCarry.transform.position = Vector3.Lerp(serverCarry.transform.position, mCamera.transform.position + mCamera.transform.forward * carryDist, Time.deltaTime * smoothCarry);
	}
	[Command]
	void CmdPlayerInteract(NetworkInstanceId netId)
	{
		GameObject serverInteractive = NetworkServer.FindLocalObject(netId);
		if (serverInteractive != null)
		{
			serverInteractive.GetComponent<InteractiveEntity>().PlayerInteract();
		}
	}

	/* Start up assignments*/ 
	void Awake()
	{
		Canvas canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
		interactText = Instantiate(textPrefeb, new Vector3(0,0,0), Quaternion.identity) as Text;
		interactText.transform.SetParent(canvas.transform,false);
		texturePos = new Rect((Screen.width - defaultTexture.width) / 2, (Screen.height - defaultTexture.height) / 2, defaultTexture.width, defaultTexture.height);
	}

	// Update is called once per frame
	void Update ()
	{
		useInteractiveTexture = false;
		SetInteractText(" ");
		GameObject targetObject = GetMouseHoverObject();

		if (targetObject != null)
		{
			string objectTag;
			if (IsInteractive(targetObject, out objectTag))
			{
				SetInteractText(objectTag);
				useInteractiveTexture = true;

				// Read input to see if the user wants to interact with the object
				if(Input.GetKeyDown(KeyCode.Mouse0))
				{
					if (targetObject.tag == "Carriable")
					{
						if (isCarrying == false)
						{
							CmdPickUp(targetObject);
						}
						else
						{
							CmdDrop();
						}
					}
					else if (targetObject.tag == "Interactive")
					{
						CmdPlayerInteract(targetObject.GetComponent<NetworkIdentity>().netId);
					}
				}
				else if(Input.GetKeyDown(KeyCode.Mouse1) && isCarrying)
				{
					// Throw item
					CmdThrow();
				}

			}
        }

		if (isCarrying)
		{
			CmdCarryObject();
		}
	}

	void OnGUI()
	{
		if (showReticle)
		{ 
			if (useInteractiveTexture)
			{
				GUI.DrawTexture(texturePos, interactTexture);
			}
			else
			{
				GUI.DrawTexture(texturePos, defaultTexture);
			}
		}
	}

	// Returns the object the mouse is hovering over within the player's reach
	GameObject GetMouseHoverObject()
	{
		RaycastHit raycastHit;
		Debug.DrawRay(mCamera.transform.position, mCamera.transform.TransformDirection(Vector3.forward) * playerReach, Color.green);
		if (Physics.Linecast(mCamera.transform.position, mCamera.transform.position + mCamera.transform.TransformDirection(Vector3.forward) * playerReach, out raycastHit))
		{
			return raycastHit.collider.gameObject;
		}
		return null;

	}

	// Returns true if the target can be interacted with
	bool IsInteractive(GameObject target, out string outTag)
	{
		outTag = " ";
		if(target.tag == "Interactive" || target.tag == "Carriable")
		{
			outTag = target.tag;
		}
		return outTag != " ";
	}

	void SetInteractText(string tag)
	{
		string displayText = " ";
		if(tag == "Interact")
		{
			displayText = "Interact";
		}

		if(tag == "Carriable")
		{
			if(isCarrying)
			{
				displayText = "Drop Here";
			}
			else
			{
				displayText = "Pick Up";
			}
		}

		interactText.text = displayText;
	}

}
