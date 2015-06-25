using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	public GameObject observatorEyes;
	public GameObject networkedObservatorEyes { get; set; }
	public Text errorText;

	private LoadConfigurations.Configurations configs = null;
	private string ipAddress = "127.0.0.1";
	private int port = 5555;
	private bool isConnected = false;
	private bool isObservatorInstantiated = false;
	private readonly int NB_ATTEMPS = 10;
	private int currentAttempt = 0;
	private KinectManager manager;

	// Use this for initialization
	void Start () {
		manager = Camera.main.GetComponent<KinectManager> ();
		configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
		if (null != configs) {
			if(configs.IsNetworkEnabled)
			{
				ipAddress = configs.IpAddress;
				port = configs.Port;
				if(!configs.isServer)
					StartCoroutine (ConnectionRoutine ());
				else
					Network.InitializeServer(4,port,false);
			}
		}
	}

	private IEnumerator ConnectionRoutine()
	{
		currentAttempt = 0;
		while (!isConnected && currentAttempt < NB_ATTEMPS) {
			Network.Connect(ipAddress,port);
			Debug.Log("Trying to connect");
			yield return new WaitForSeconds(3.0f);
			currentAttempt++;
		}

		if (currentAttempt >= NB_ATTEMPS) 
		{
			StartCoroutine(ShowNetworkClientErrorMessage());
		}
	}
	

	private void OnConnectedToServer()
	{
		//a client just joigned the server
		isConnected = true;
		InstanciateObservatorEyesGameObject ();
	}

	private void OnServerInitialized()
	{
		isConnected = true;
		InstanciateObservatorEyesGameObject ();
		Debug.Log ("Server initialized");
	}

	private void OnDisconnectedToServer()
	{
		//The connection has been lost or closed
		isConnected = false;
		Destroy (observatorEyes);
		isObservatorInstantiated = false;
		if(!configs.isServer)
			StartCoroutine (ConnectionRoutine ()); // try to connect again if client
	}

	private void InstanciateObservatorEyesGameObject ()
	{
		if (!isObservatorInstantiated) 
		{
			networkedObservatorEyes = Network.Instantiate (observatorEyes, manager.GetObservatorPointOfView (), Quaternion.identity, 0) as GameObject;
			networkedObservatorEyes.AddComponent<UpdateEyesPosition>();
			if (Network.isServer)
				networkedObservatorEyes.name = "serverObservator";
			else
				networkedObservatorEyes.name = "clientObservator";
			isObservatorInstantiated = true;
		}
	}

	private IEnumerator ShowNetworkClientErrorMessage()
	{
		errorText.enabled = true;
		errorText.text = "Connexion to server failed ! ";
		yield return new WaitForSeconds(10);
		errorText.enabled = false;
	}

}
