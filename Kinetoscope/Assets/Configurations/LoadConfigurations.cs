using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using INI;

public class LoadConfigurations : MonoBehaviour {

	public Text errorText;
	public Configurations loadedConfigs;
	public bool enableNetworkCommunication = false;

	/***
	 * private attributes
	 ***/
	private IniFile iniConfigs; //Ini files reader/writer object
	private readonly string INI_PATH = @"C:/kinetoscope.ini"; //the ini file path called kinetoscope.ini
	private readonly string CALIBRATION_SECTION = "calibration"; //calibration section string
	private readonly string OBSERVATOR_SECTION = "observator"; //observator section string
	private readonly string NETWORK_SECTION = "network";
	private bool isCalibrationLoaded = false;
	private bool isObservatorLoaded = false;
	private bool isNetworkLoaded = true; // true because we're not forced to use network communication
	private bool isConfigurationLoadingSuccess = false;
	private float time = 0.0f;
	private readonly float DISPLAY_TIME = 10.0f;


	// initialization
	void Start () {

		Camera.main.enabled = true;
		errorText.enabled = false;
		errorText.text = "";
		loadedConfigs = new Configurations ();

		//at initialisation we will read the calibrations value and set it to our scene objects
		iniConfigs = new IniFile (INI_PATH);
		LoadCalibrationConfigs ();
		LoadObservatorConfigs ();
		LoadNetworkConfigs ();

		if (!isCalibrationLoaded || !isObservatorLoaded || !isNetworkLoaded) {
			errorText.enabled = true;
			time = Time.time;
		} 
		else 
		{
			isConfigurationLoadingSuccess = true;
		}
	}

	private void LoadCalibrationConfigs ()
	{
		float screenWidth = 0, screenHeight = 0, screenCenterX = 0, screenCentery = 0, screenCenterZ = 0;
		if (float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenWidth.Value), out screenWidth) && float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenHeight.Value), out screenHeight) && float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterX.Value), out screenCenterX) && float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterY.Value), out screenCentery) && float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterZ.Value), out screenCenterZ)) {
			loadedConfigs.ScreenCenter = new Vector3 (screenCenterX, screenCentery, screenCenterZ);
			loadedConfigs.ScreenScale = new Vector3 (screenWidth / 10.0f, 1, screenHeight / 10.0f); // we divide by 10.0 because a plane object is a 10 per 10 meters object by default and its actually a scaling factor
			isCalibrationLoaded = true;
		}
		else 
		{
			errorText.text += "Error while loading calibrations configurations\n";
		}
	}

	private void LoadObservatorConfigs ()
	{
		float observatorX = 0, observatorY = 0, observatorZ = 0;
		if (float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorX.Value), out observatorX) && float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorY.Value), out observatorY) && float.TryParse (iniConfigs.IniReadValue (OBSERVATOR_SECTION, INIValues.ObservatorZ.Value), out observatorZ)) {
			loadedConfigs.ObservatorPosition = new Vector3 (observatorX, observatorY, observatorZ);
			isObservatorLoaded = true;
		}
		else 
		{
			errorText.text += "Error while loading observator position\n";
		}
	}

	private void LoadNetworkConfigs()
	{
		if (enableNetworkCommunication) 
		{
			string ipAddress;
			int port = 0, isServer = 0;
			ipAddress = iniConfigs.IniReadValue(NETWORK_SECTION, INIValues.IpAddress.Value);

			if(ipAddress.Length > 0 && int.TryParse(iniConfigs.IniReadValue(NETWORK_SECTION,INIValues.Port.Value),out port)
			   && int.TryParse(iniConfigs.IniReadValue(NETWORK_SECTION,INIValues.Port.Value),out isServer))
			{
				loadedConfigs.IpAddress = ipAddress;
				loadedConfigs.Port = port;
				loadedConfigs.isServer = isServer > 0;
			}
			else
			{
				errorText.text += "Error while loading Network communication configurations";
				isNetworkLoaded = false;
			}
		}
		loadedConfigs.IsNetworkEnabled = enableNetworkCommunication;
	}

	void Update()
	{
		// if the message has been displayed DISPLAY_TIME seconds
		if (Time.time - time > DISPLAY_TIME && errorText.enabled) {
			errorText.enabled = false; //hide text message
		}
	}

	public Configurations LoadedConfigs
	{
		get{
			if(isConfigurationLoadingSuccess)
			{
				return loadedConfigs;
			}
			else
			{
				return null;
			}
		}
		private set
		{
			loadedConfigs = value;
		}
	}


	public class Configurations
	{
		public Vector3 ObservatorPosition { get; set; }
		public Vector3 ScreenCenter { get; set; }
		public Vector3 ScreenScale { get; set; }
		public bool IsNetworkEnabled { get; set; }
		public int Port { get; set; }
		public string IpAddress { get; set; }
		public bool isServer { get; set; }
	}

}

