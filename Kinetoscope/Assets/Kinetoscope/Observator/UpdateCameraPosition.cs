using UnityEngine;
using System.Collections;

public class UpdateCameraPosition : MonoBehaviour {

	private KinectManager manager = null;
	private NetworkView view = null;
	private Matrix4x4 domainConversionMatrix;
	private LoadConfigurations.Configurations configs = null;

	private void Start()
	{
		manager = Camera.main.GetComponent<KinectManager> ();
		view = GetComponent<NetworkView> ();
		configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
		initMatrix ();
	}

	// Update is called once per frame
	void Update () {
		if (null != view) 
		{
			Camera.main.transform.position = domainConversionMatrix.MultiplyPoint3x4(view.transform.position);
		}
	}

	private void initMatrix()
	{
		domainConversionMatrix = Matrix4x4.identity;
		if (null != configs && null != manager && null != view) 
		{
			Vector3 translationVector = new Vector3 (configs.TranlsationX, configs.TranslationY, configs.TranslationZ);
			Vector3 scaleVector = Vector3.one;
			scaleVector.x = -1; // x and z axis are inverted between one and the other kinect
			scaleVector.z = -1;
			// we know that the x axis rotation value of the kinect sensor is actually store in the scaling transform vector x coordinate of the view
			float xAxisRotOtherSensor = view.transform.localScale.x; // the final rot angle is the substraction between the other sensor angle and the current sensor angle
			float finalXAxisRot = xAxisRotOtherSensor - - manager.sensorAngle;
			Quaternion rotQuat = Quaternion.Euler(new Vector3(finalXAxisRot,0f,0f));
			domainConversionMatrix.SetTRS (translationVector, rotQuat, scaleVector);
		}
	}
}
