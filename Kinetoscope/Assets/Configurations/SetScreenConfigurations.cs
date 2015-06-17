using UnityEngine;
using System.Collections;

public class SetScreenConfigurations : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadConfigurations.Configurations configs = GameObject.Find ("ConfigurationsManager").GetComponent<LoadConfigurations> ().LoadedConfigs;
		if (configs != null) {
			transform.localScale = configs.ScreenScale;
			transform.position = configs.ScreenCenter;
			transform.rotation = Quaternion.Euler (270.0f, 0.0f, 0.0f); // rotate of 270° on x axis to place the screen in front of camera
		}
	}

}
