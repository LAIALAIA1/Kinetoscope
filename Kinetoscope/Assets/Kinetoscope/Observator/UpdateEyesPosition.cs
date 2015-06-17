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
		if (null != manager) {
			transform.position = manager.GetObservatorPointOfView();
		}
	}
}
