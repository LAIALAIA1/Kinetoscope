using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using INI;

public class LoadConfigurations : MonoBehaviour {

	public Text errorText;
	public Configurations loadedConfigs;

	/***
	 * private attributes
	 ***/
	private IniFile iniConfigs; //Ini files reader/writer object
	private string INI_PATH = ""; //the ini file path called kinetoscope.ini
	private readonly string CALIBRATION_SECTION = "calibration"; //calibration section string
	private readonly string OBSERVATOR_SECTION = "observator"; //observator section string
	private readonly string NETWORK_SECTION = "network"; //network section string
	private readonly string DOMAIN_SECTION = "domainconversion"; //domain conversion section string
	private bool isCalibrationLoaded = false;
	private bool isObservatorLoaded = false;
	private bool isNetworkLoaded = true; // true because we're not forced to use network communication and there's no error if we don't want to
	private bool isDomainConversionParamsLoaded = true;
	private bool isConfigurationLoadingSuccess = false;
	private readonly float DISPLAY_TIME = 10.0f;


	// initialization
	void Start () {
		INI_PATH = Application.dataPath.ToString() + "/Configs/kinetoscope.ini";
		Camera.main.enabled = true;
		errorText.enabled = false;
		errorText.text = "";
		loadedConfigs = new Configurations (); //create empty configurations object

		//at initialisation we will read the calibrations value and set it to our scene objects
		iniConfigs = new IniFile (INI_PATH);
		LoadCalibrationConfigs ();
		LoadObservatorConfigs ();
		LoadNetworkConfigs ();
		LoadDomainConversionParams ();

		if (!isCalibrationLoaded || !isObservatorLoaded || !isNetworkLoaded || !isDomainConversionParamsLoaded) {
			StartCoroutine(ShowErrorMessages());
		} 
		else 
		{
			isConfigurationLoadingSuccess = true;
		}
	}

	private void LoadCalibrationConfigs ()
	{
		float screenWidth = 0, screenHeight = 0, screenCenterX = 0, screenCentery = 0, screenCenterZ = 0;
		if (float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenWidth.Value), out screenWidth) && 
		    float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenHeight.Value), out screenHeight) && 
		    float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterX.Value), out screenCenterX) && 
		    float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterY.Value), out screenCentery) && 
		    float.TryParse (iniConfigs.IniReadValue (CALIBRATION_SECTION, INIValues.ScreenCenterZ.Value), out screenCenterZ)) 
		{
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
		int isNetworkEnabled = 0;
		int.TryParse (iniConfigs.IniReadValue (NETWORK_SECTION, INIValues.IsNetworkEnabled.Value),out isNetworkEnabled);
		loadedConfigs.IsNetworkEnabled = isNetworkEnabled > 0;
		if (loadedConfigs.IsNetworkEnabled) 
		{
			string ipAddress;
			int port = 0, isServer = 0;
			ipAddress = iniConfigs.IniReadValue(NETWORK_SECTION, INIValues.IpAddress.Value);

			if(ipAddress.Length > 0 && int.TryParse(iniConfigs.IniReadValue(NETWORK_SECTION,INIValues.Port.Value),out port)
			   && int.TryParse(iniConfigs.IniReadValue(NETWORK_SECTION,INIValues.IsServer.Value),out isServer))
			{
				loadedConfigs.IpAddress = ipAddress;
				loadedConfigs.Port = port;
				loadedConfigs.isServer = isServer > 0;
			}
			else
			{
				errorText.text += "Error while loading Network communication configurations\n";
				isNetworkLoaded = false;
			}
		}
	}

	private void LoadDomainConversionParams ()
	{
		if (isNetworkLoaded) 
		{
			float translationX = 0f, translationY = 0f, translationZ = 0f;
			if (float.TryParse (iniConfigs.IniReadValue (DOMAIN_SECTION, INIValues.TranslationX.Value), out translationX) && 
			    float.TryParse (iniConfigs.IniReadValue (DOMAIN_SECTION, INIValues.TranslationY.Value), out translationY) && 
			    float.TryParse (iniConfigs.IniReadValue (DOMAIN_SECTION, INIValues.TranslationZ.Value), out translationZ))
			{
				loadedConfigs.TranlsationX = translationX;
				loadedConfigs.TranslationY = translationY;
				loadedConfigs.TranslationZ = translationZ;
			}
			else
			{
				errorText.text += "Error while loading domain conversion parameters";
				isDomainConversionParamsLoaded = false;
			}
		}
	}
	
	private IEnumerator ShowErrorMessages()
	{
		errorText.enabled = true;
		yield return new WaitForSeconds (DISPLAY_TIME);
		errorText.enabled = false;
		errorText.text = "";
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

		public float TranlsationX { get; set; }
		public float TranslationY { get; set; }
		public float TranslationZ { get; set; }
	}

}

