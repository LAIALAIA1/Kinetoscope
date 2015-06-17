using UnityEngine;
using System.Collections;

public class SetCameraConfigurations : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadConfigurations.Configurations configs = GameObject.Find ("ConfigurationsManager").GetComponent<LoadConfigurations> ().LoadedConfigs;
		if (configs != null) {
			transform.position = configs.ObservatorPosition;
		}
	}

}
