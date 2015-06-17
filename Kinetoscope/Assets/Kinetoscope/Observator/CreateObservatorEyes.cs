using UnityEngine;
using System.Collections;

public class CreateObservatorEyes : MonoBehaviour {

	public GameObject observatorEyes;
	// Use this for initialization
	void Start () {
		Instantiate (observatorEyes,new Vector3(0f,0f,0f),Quaternion.identity);
	}
}
