using UnityEngine;
using System.Collections;

public class MaterialManager : MonoBehaviour {

	public Material standardMaterial = null; //set from editor
	public Material playbackMaterial = null; // set from editor
	public Material negativeMaterial = null; // set from editor

	private MaterialState materialState = MaterialState.Standard;
	private readonly int NB_MIN_FLICKR = 8;
	private readonly int NB_MAX_FLICKR = 16;
	private readonly int LOOP_START = 5;

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
		float waitTime = 1e10f;
		int loopCount = LOOP_START;
		MaterialState matState = materialState;
		bool rendererState = false;

		if (null != renderer) 
		{
			if(withNegative) SetNegativeMaterialToAll();
			while(waitTime > Time.deltaTime && loopCount < nbOfFlickr)
			{
				waitTime = Random.Range(FlickrTime(loopCount+1),FlickrTime(loopCount));
				loopCount++;
				yield return new WaitForSeconds(waitTime);
				if(Random.value > 0.20f)
				{
					rendererState = !rendererState;
				}
				renderer.enabled = rendererState;
			}
			renderer.enabled = true;
			materialState = matState;
			SetRightMaterialToAll();
		}
	}

	private float FlickrTime(float x)
	{
		return 1.0f / (x + 1E-3f); // never divided by 0
	}

	public void LaunchNegativeMaterialAnimation(float time)
	{
		StartCoroutine(NegativeMaterialAnimation(time));
	}

	public void LaunchFlickrAnimation(bool withNegativeEffect)
	{
		StartCoroutine (FlickrAnimation (withNegativeEffect));
	}

	public enum MaterialState
	{
		Standard, Negative, Playback
	}
}
