using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {

	public Material standardMaterial = null; //set from editor
	public Material playbackMaterial = null; // set from editor
	public Material negativeMaterial = null; // set from editor

	private MaterialState materialState = MaterialState.Standard;
	private readonly int NB_MIN_FLICKR = 2;
	private readonly int NB_MAX_FLICKR = 3;

	public void SetStandardMaterial()
	{
		if (null != standardMaterial) 
		{
			GetComponent<Renderer> ().material = standardMaterial;
			materialState = MaterialState.Standard;
		}
	}

	public void SetStandardMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetStandardMaterial();
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

	public void SetPlaybackMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetPlaybackMaterial();
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

	public void SetNegativeMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetNegativeMaterial();
		}
	}

	private void SetRightMaterial ()
	{
		if (materialState == MaterialState.Playback) {
			SetPlaybackMaterial ();
		}
		else {
			SetStandardMaterial ();
		}
	}

	public void SetRightMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetRightMaterial();
		}
	}

	private IEnumerator NegativeMaterialAnimation(float time)
	{
		SetNegativeMaterial ();
		yield return new WaitForSeconds(time);
		SetRightMaterial ();
	}

	private IEnumerator FlickrAnimation(bool withNegative)
	{
		Random.seed = (int)Time.time; // seed the random generator
		Renderer renderer = GetComponent<Renderer> ();
		int nbOfFlickr = Random.Range (NB_MIN_FLICKR, NB_MAX_FLICKR) * 2;
		float waitTime = 0f;
		//MaterialState matState = materialState;
		bool rendererState = false;

		if (null != renderer) 
		{
			if(withNegative) SetNegativeMaterialToAll();
			for(int i = 0; i < nbOfFlickr; i++)
			{
				waitTime = Random.Range(Time.deltaTime*2,Time.deltaTime*10);
				yield return new WaitForSeconds(waitTime);
				if(Random.value > 0.20f)
				{
					rendererState = !rendererState;
				}
				renderer.enabled = rendererState;
			}
			renderer.enabled = true;
			//materialState = matState;
			SetRightMaterialToAll();
		}
	}

	private float FlickrTime(float x)
	{
		return 1.0f / (1.0f + x); // never divided by 0
	}

	private IEnumerator HideForAWhile(float time)
	{
		Renderer renderer = GetComponent<Renderer> ();
		if (null != renderer) 
		{
			renderer.enabled = false;
			yield return new WaitForSeconds (time);
			renderer.enabled = true;
		}
	}

	public void LaunchNegativeMaterialAnimation(float time)
	{
		StartCoroutine(NegativeMaterialAnimation(time));
	}

	public void LaunchFlickrAnimation(bool withNegativeEffect)
	{
		StartCoroutine (FlickrAnimation (withNegativeEffect));
	}

	public void LaunchHideForAWhile(float time)
	{
		StartCoroutine (HideForAWhile (time));
	}

	public enum MaterialState
	{
		Standard, Negative, Playback
	}
}
