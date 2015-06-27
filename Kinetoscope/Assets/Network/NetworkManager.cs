using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Network manager. Manage the network connection between the two apps and the observator point of view exchange
/// </summary>
public class NetworkManager : MonoBehaviour {

	public GameObject observatorEyes; //observator object : set from editor
	public GameObject networkedObservatorEyes { get; set; } // the network instance of observator eyes
	public Text errorText; // Error text GUI display : set from editor

	private LoadConfigurations.Configurations configs = null; // app configurations object
	private string ipAddress = "127.0.0.1"; // ipAdress to connect (or listen if server) default is localhost
	private int port = 5555; // port
	private bool isConnected = false; // is the app already connected ?
	private bool isObservatorInstantiated = false; // is the observator eye's object already instantiate on network
	private readonly int NB_ATTEMPS = 50; //number of connection attemps between give up
	private int currentAttempt = 0; // current number of attempt
	private KinectManager manager; // the kinect manager to get the observator eyes positions

	// Use this for initialization
	void Start () {
		// gets the manager and the app configuration object
		manager = Camera.main.GetComponent<KinectManager> ();
		configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
		if (null != configs) {
			// Check if the network should be enabled
			if(configs.IsNetworkEnabled)
			{
				// get ip adress and port from config
				ipAddress = configs.IpAddress;
				port = configs.Port;
				// if the app is actually a client then connect
				if(!configs.isServer)
					StartCoroutine (ConnectionRoutine ());
				else
					Network.InitializeServer(4,port,false); //else start server on port without NAT
			}
		}
	}

	/// <summary>
	/// Connection routine. Tries to connect a certain number of time to server
	/// </summary>
	private IEnumerator ConnectionRoutine()
	{
		currentAttempt = 0;
		//while not connected and number of attempts lesser than authorized
		while (!isConnected && currentAttempt < NB_ATTEMPS) {
			Network.Connect(ipAddress,port); //will call a callback OnConnectedToServer if success
			Debug.Log("Trying to connect");
			yield return new WaitForSeconds(3.0f); // wait 3 seconds
			currentAttempt++; //inc the current attemps counter
		}

		// if we reach the authorized nb of attempt and we're still not connected
		if (currentAttempt >= NB_ATTEMPS && !isConnected) 
		{
			StartCoroutine(ShowNetworkClientErrorMessage()); //show error message
		}
	}
	

	/// <summary>
	/// Raises the connected to server event.
	/// </summary>
	private void OnConnectedToServer()
	{
		//a client just joigned the server
		isConnected = true;
		InstanciateObservatorEyesGameObject (); // instantiate the observator eye's on network
	}

	/// <summary>
	/// Raises the server initialized event.
	/// </summary>
	private void OnServerInitialized()
	{
		isConnected = true; //as soon as the server is initialized we consider that we0re connected (we wait client's connection)
		InstanciateObservatorEyesGameObject (); // instantiate observator eye's object (object instiated on server will be instantiated on all connected clients)
		Debug.Log ("Server initialized");
	}

	/// <summary>
	/// Raises the disconnected to server event.
	/// </summary>
	private void OnDisconnectedToServer()
	{
		//The connection has been lost or closed
		isConnected = false;
		Destroy (networkedObservatorEyes); //destroy network instantiated observator eyes object
		isObservatorInstantiated = false;
		//if not a server, tries to reconnect
		if(!configs.isServer)
			StartCoroutine (ConnectionRoutine ()); // try to connect again if client
	}

	/// <summary>
	/// Instanciates the observator eyes game object on network.
	/// </summary>
	private void InstanciateObservatorEyesGameObject ()
	{
		if (!isObservatorInstantiated) 
		{
			networkedObservatorEyes = Network.Instantiate (observatorEyes, manager.GetObservatorPointOfView (), Quaternion.identity, 0) as GameObject;
			networkedObservatorEyes.AddComponent<UpdateEyesPosition>(); // add a script to update the pos of instanciated object

			// for debug purpose -> rename instanciated object
			if (Network.isServer)
				networkedObservatorEyes.name = "serverObservator";
			else
				networkedObservatorEyes.name = "clientObservator";
			isObservatorInstantiated = true;
		}
	}

	/// <summary>
	/// Shows the network client error message.
	/// </summary>
	private IEnumerator ShowNetworkClientErrorMessage()
	{
		errorText.enabled = true;
		errorText.text = "Connexion to server failed ! ";
		yield return new WaitForSeconds(10);
		errorText.enabled = false;
	}

}
