using UnityEngine;
using System.Collections;

public class LogoDisplayer : MonoBehaviour {

	public Texture HEArcTexture = null; //the logo texture : set from editor

	private void OnGUI()
	{
		// if texture has been set from editor (not null)
		if (null != HEArcTexture) 
		{
			//draw it on left bottom corner of screen
			GUI.DrawTexture(new Rect(10,Screen.height - HEArcTexture.height - 10,HEArcTexture.width,HEArcTexture.height),HEArcTexture);
		}
	}
}
