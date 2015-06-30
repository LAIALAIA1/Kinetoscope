using UnityEngine;
using System.Collections;


/// <summary>
/// Update the observator's eyes object position that will be pushed on network.
/// </summary>
public class UpdateEyesPosition : MonoBehaviour {

	private KinectManager manager = null;

	void Start()
	{
		manager = Camera.main.GetComponent<KinectManager> (); // get the manager to obtain the observator eyes position
	}

	void Update () {
		if(null != manager)
		{
			transform.position = manager.GetObservatorPointOfView (); //apply new position at each frame
		}
	}

}
