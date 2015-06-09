using UnityEngine;
using System.Collections;
using INI;

public class LoadConfigurations : MonoBehaviour {

	private IniFile iniConfigs;
	private readonly string INI_PATH = @"C:/kinetoscope.ini"; //the ini file path called kinetoscope.ini
	private readonly string CALIBRATION_SECTION = "calibration"; //calibration section string
	private readonly string OBSERVATOR_SECTION = "observator"; //observator section string


	// initialization
	void Start () {

		Debug.Log (INI_PATH);
		//at initialisation we will read the calibrations value and set it to our scene objects
		iniConfigs = new IniFile (INI_PATH);
		float screenWidth = 0, screenHeight = 0, screenCenterX = 0, screenCentery = 0, screenCenterZ = 0;
		float observatorX = 0, observatorY = 0, observatorZ = 0;

		if(float.TryParse(iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenWidth.Value), out screenWidth)
		   && float.TryParse(iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenHeight.Value), out screenHeight)
		   && float.TryParse(iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterX.Value), out screenCenterX)
		   && float.TryParse(iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterY.Value), out screenCentery)
		   && float.TryParse(iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterZ.Value), out screenCenterZ))
		{
			Debug.Log("CONFIGURATION LOADING SUCCESS");
		}
	}

}

