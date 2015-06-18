using UnityEngine;
using System.Collections;

public class UpdateEyesPosition : MonoBehaviour {

	private KinectManager manager = null;

	void Start()
	{
		manager = Camera.main.GetComponent<KinectManager> ();
	}

	void Update () {
		transform.position = manager.GetObservatorPointOfView ();
	}
	
}
