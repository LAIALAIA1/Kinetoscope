using UnityEngine;
using System.Collections;

public class UpdateCameraPosition : MonoBehaviour {

	private NetworkView view = null;
	private Matrix4x4 domainConversionMatrix;
	private LoadConfigurations.Configurations configs = null;
	private Transform oldPos;

	private void Start()
	{
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
		if (null != configs) 
		{
			Vector3 translationVector = new Vector3 (configs.TranlsationX, configs.TranslationY, configs.TranslationZ);
			Quaternion rotQuat = Quaternion.Euler (new Vector3 (0.0f, configs.RotationAngle, 0f));
			domainConversionMatrix.SetTRS (translationVector, rotQuat, Vector3.one);
			Debug.Log("Matrix " + domainConversionMatrix.ToString());
		}
	}
}
