using UnityEngine;
using System.Collections;

public class UpdateEyesPosition : MonoBehaviour {

	private KinectManager manager = null;

	void Start()
	{
		manager = Camera.main.GetComponent<KinectManager> ();
	}
	// Update is called once per frame

	void Update () {
		GetComponent<NetworkView> ().RPC ("UpdateObservatorEyesPosition", RPCMode.Others,manager.GetObservatorPointOfView());
	}

	[RPC]
	void UpdateObservatorEyesPosition(Vector3 position)
	{
		transform.position = position;
	}
}
