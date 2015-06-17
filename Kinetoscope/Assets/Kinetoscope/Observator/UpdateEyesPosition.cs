using UnityEngine;
using System.Collections;

public class UpdateEyesPosition : MonoBehaviour {

	private KinectManager manager = null;

	void Start()
	{
		Debug.Log ("GTFO");
		manager = Camera.main.GetComponent<KinectManager> ();
	}

	void Update () {
		Debug.Log (manager.GetObservatorPointOfView ().ToString ());
		transform.position = manager.GetObservatorPointOfView ();
	}
	
}
