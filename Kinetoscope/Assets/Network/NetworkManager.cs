using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject observatorEyes;
	public GameObject networkedObservatorEyes;

	private string ipAddress = "127.0.0.1";
	private int port = 5555;
	private bool isConnected = false;
	private bool isObservatorInstantiated = false;
	private readonly int NB_ATTEMPS = 30;
	private int currentAttempt = 0;
	private KinectManager manager;

	// Use this for initialization
	void Start () {
		manager = Camera.main.GetComponent<KinectManager> ();
		LoadConfigurations.Configurations configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
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
		while (!isConnected) {
			Network.Connect(ipAddress,port);
			Debug.Log("Trying to connect");
			yield return new WaitForSeconds(2.0f);
			currentAttempt++;
			if(currentAttempt > NB_ATTEMPS) break;
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
	
	// Update is called once per frame
	void Update () {

	}
}
