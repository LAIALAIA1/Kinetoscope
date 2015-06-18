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
	void FixedUpdate () {
		if (null != view) 
		{
			if(!view.isMine)
			{
				if(null == oldPos)
				{
					Camera.main.transform.position = domainConversionMatrix.MultiplyVector(view.transform.position);
				}
				else
				{
					Camera.main.transform.position = Vector3.Lerp(oldPos.position, domainConversionMatrix.MultiplyVector(view.transform.position), Time.smoothDeltaTime);
				}
				oldPos = Camera.main.transform;
			}
		}
	}

	private void initMatrix()
	{
		domainConversionMatrix = Matrix4x4.identity;
		if (null != configs) 
		{
			Debug.Log(configs.RotationAngle);
			Vector3 translationVector = new Vector3 (configs.TranlsationX, configs.TranslationY, configs.TranslationZ);
			Quaternion rotQuat = Quaternion.Euler (new Vector3 (0f, configs.RotationAngle, 0f));
			domainConversionMatrix.SetTRS (translationVector, rotQuat, Vector3.one);
		}
		Debug.Log (domainConversionMatrix.ToString ());
	}
}
