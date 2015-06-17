﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private string ipAddress = "127.0.0.1";
	private int port = 5555;
	private bool isConnected = false;

	// Use this for initialization
	void Start () {
		LoadConfigurations.Configurations configs = GameObject.Find("ConfigurationsManager").GetComponent<LoadConfigurations>().LoadedConfigs;
		if (null != configs) {
			ipAddress = configs.IpAddress;
			port = configs.Port;
			Debug.Log (ipAddress + " - " + configs.Port);
			StartCoroutine (ConnectionRoutine ());
		}
	}

	private IEnumerator ConnectionRoutine()
	{
		while (!isConnected) {
			Network.Connect(ipAddress,port);
			Debug.Log("Trying to connect");
			yield return new WaitForSeconds(2.0f);
		}
	}

	private void OnConnectedToServer()
	{
		//a client just joigned the server
		isConnected = true;
	}

	private void OnDisconnectedToServer()
	{
		//The connection has been lost or closed
		isConnected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isConnected) {

		}
	}
}
