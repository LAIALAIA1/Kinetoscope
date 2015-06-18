using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.UpArrow)) {
			transform.position += Vector3.up * Time.deltaTime * 5.0f;
		}
	}
}
