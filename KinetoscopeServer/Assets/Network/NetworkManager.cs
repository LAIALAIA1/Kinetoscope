using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	private float topleftAnchorX = 0f;
	private float topLeftAnchorY = 0f;
	private int port = 5555;
	private bool isInitialized;

	// Use this for initialization
	void Start () {
		topleftAnchorX = Screen.width * 0.05f;
		topLeftAnchorY = Screen.height * 0.05f;
		Network.InitializeServer (4, port, false);
	}

	private void OnServerInitialized()
	{
		isInitialized = true;
		Debug.Log ("Server initialized");
	}

	private void OnGUI()
	{
		GUI.Label (new Rect(topleftAnchorX,topLeftAnchorY,200,20),"Server status : " + (isInitialized ? "On" : "Off"));
		GUI.Label (new Rect(topleftAnchorX,topLeftAnchorY+20,200,20),"Server IP : " + Network.player.ipAddress.ToString());
		GUI.Label (new Rect(topleftAnchorX,topLeftAnchorY+40,200,20),"Clients connected : " + Network.connections.Length);
	}
}
