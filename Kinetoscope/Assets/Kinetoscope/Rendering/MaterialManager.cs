using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {

	public Material standardMaterial = null; //set from editor
	public Material playbackMaterial = null; // set from editor
	public Material negativeMaterial = null; // set from editor

	private MaterialState materialState = MaterialState.Standard;

	public void SetStandardMaterial()
	{
		if (null != standardMaterial) 
		{
			GetComponent<Renderer> ().material = standardMaterial;
			Debug.Log("zbouub malhonnete");
			materialState = MaterialState.Standard;
		}
	}

	public void SetPlaybackMaterial()
	{
		if (null != playbackMaterial) 
		{
			GetComponent<Renderer> ().material = playbackMaterial;
			materialState = MaterialState.Playback;
		}
	}

	private void SetNegativeMaterial()
	{
		if (null != negativeMaterial) 
		{
			GetComponent<Renderer> ().material = negativeMaterial;
			materialState = MaterialState.Negative;
		}
	}

	private IEnumerator NegativeMaterialAnimation(float time)
	{
		SetNegativeMaterial ();
		yield return new WaitForSeconds(time);
		if (materialState == MaterialState.Playback) {
			SetPlaybackMaterial ();
		}
		else {
			SetStandardMaterial ();
		}
	}

	public void LaunchNegativeMaterialAnimation(float time)
	{
		StartCoroutine(NegativeMaterialAnimation(time));
	}

	public enum MaterialState
	{
		Standard, Negative, Playback
	}
}
