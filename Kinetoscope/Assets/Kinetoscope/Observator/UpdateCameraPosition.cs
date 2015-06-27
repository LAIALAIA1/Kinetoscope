using UnityEngine;
using System.Collections;

public class UpdateCameraPosition : MonoBehaviour {

	private KinectManager manager = null; //kinect manager
	private NetworkView view = null; // network view (network item for synchronization)
	private Matrix4x4 domainConversionMatrix; // matrix for conversion between the two kinects domain
	private LoadConfigurations.Configurations configs = null; // app configuration object

	private void Start()
	{
		//get the manager, the view and the app config object
		manager = Camera.main.GetComponent<KinectManager> ();
		view = GetComponent<NetworkView> ().isMine ? null : GetComponent<NetworkView> ();
		configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
		initMatrix (); //initialize conversion matrix
	}

	// Update is called once per frame
	void Update () {
		//if the view is found
		if (null != view) {
			Camera.main.transform.position = domainConversionMatrix.MultiplyPoint3x4 (view.transform.position);
		} else {
			Camera.main.transform.position = domainConversionMatrix.MultiplyPoint3x4 (configs.ObservatorPosition);
		}
	}

	/// <summary>
	/// Initialize the matrix.
	/// </summary>
	private void initMatrix()
	{
		domainConversionMatrix = Matrix4x4.identity;
		if (null != configs && null != manager) 
		{
			Vector3 translationVector = new Vector3 (configs.TranlsationX, configs.TranslationY, configs.TranslationZ);
			Vector3 scaleVector = Vector3.one;
			scaleVector.x = -1; // x and z axis are inverted between one and the other kinect
			scaleVector.z = -1;
			domainConversionMatrix.SetTRS (translationVector, Quaternion.identity, scaleVector);
		}
	}
}
