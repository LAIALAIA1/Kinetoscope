using UnityEngine;
using System.Collections;

public class UpdateEyesPosition : MonoBehaviour {

	private KinectManager manager = null;

	void Start()
	{
		manager = Camera.main.GetComponent<KinectManager> ();
	}

	void Update () {
		if(null != manager)
		{
			transform.position = manager.GetObservatorPointOfView ();
			transform.localScale.Set(manager.sensorAngle, 0f, 0f);
		}
	}

}
