using UnityEngine;
using System.Collections;

public class UpdateCameraPosition : MonoBehaviour {

	private NetworkView view = null;

	private void Start()
	{
		view = GetComponent<NetworkView> ();
	}

	// Update is called once per frame
	void Update () {
		if (null != view) 
		{
			if(!view.isMine)
			{
				Camera.main.transform.position = view.transform.position;
			}
		}
	}
}
