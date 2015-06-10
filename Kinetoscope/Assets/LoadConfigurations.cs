using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using INI;

public class LoadConfigurations : MonoBehaviour {

	public GameObject projectionScreen; // the projection screen gameobject on scene
	public Text errorText;

	/***
	 * private attributes
	 ***/
	private IniFile iniConfigs; //Ini files reader/writer object
	private readonly string INI_PATH = @"C:/kinetoscope.ini"; //the ini file path called kinetoscope.ini
	private readonly string CALIBRATION_SECTION = "calibration"; //calibration section string
	private readonly string OBSERVATOR_SECTION = "observator"; //observator section string
	private bool isCalibrationLoaded = false;
	private bool isObservatorLoaded = false;
	private float time = 0.0f;
	private readonly float DISPLAY_TIME = 10.0f;


	// initialization
	void Start () {

		Camera.main.enabled = true;
		errorText.enabled = false;
		errorText.text = "";

		Debug.Log (INI_PATH);
		//at initialisation we will read the calibrations value and set it to our scene objects
		iniConfigs = new IniFile (INI_PATH);
		float screenWidth = 0, screenHeight = 0, screenCenterX = 0, screenCentery = 0, screenCenterZ = 0;
		float observatorX = 0, observatorY = 0, observatorZ = 0;

		if (float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenWidth.Value), out screenWidth)
			&& float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenHeight.Value), out screenHeight)
			&& float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterX.Value), out screenCenterX)
			&& float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterY.Value), out screenCentery)
			&& float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterZ.Value), out screenCenterZ)) {

			if(null != projectionScreen)
			{
				projectionScreen.transform.localScale = new Vector3(screenWidth/10.0f, 1, screenHeight/10.0f);
				projectionScreen.transform.position = new Vector3(screenCenterX,screenCentery, screenCenterZ);
				projectionScreen.transform.rotation = Quaternion.Euler(270.0f, 0.0f, 0.0f);
				isCalibrationLoaded = true;
			}
		} 
		else 
		{
			errorText.text += "Error while loading calibrations configurations\n";
		}

		if (float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorX.Value), out observatorX)
		    && float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorY.Value), out observatorY)
		    && float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorZ.Value), out observatorZ)) {
			Camera.main.transform.position = new Vector3(observatorX,observatorY,observatorZ);
			isObservatorLoaded = true;
		} 
		else 
		{
			errorText.text += "Error while loading observator position";
		}

		if (!isCalibrationLoaded || !isObservatorLoaded) 
		{
			errorText.enabled = true;
			time = Time.time;
		}
	}

	void Update()
	{
		// if the message has been displayed DISPLAY_TIME seconds
		if (Time.time - time > DISPLAY_TIME) {
			errorText.enabled = false; //hide text message
			this.enabled = false; //kill script
		}
	}

}

