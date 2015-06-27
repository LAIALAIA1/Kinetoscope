using UnityEngine;
using System.Collections;

/// <summary>
/// Material manager. Class that manage switches between materials and shader for rendering effects
/// </summary>
public class MaterialManager : MonoBehaviour {

	public Material standardMaterial = null; // standard material and shader for skeleton : set from editor
	public Material playbackMaterial = null; // materail used during playback animation : set from editor
	public Material negativeMaterial = null; // negative colors material for new detected user effect : set from editor

	private MaterialState materialState = MaterialState.Standard;
	private readonly int NB_MIN_FLICKR = 2; // minimum number of blinking for the flicker
	private readonly int NB_MAX_FLICKR = 3; // maximum number of blinking for the flicker
	private float minBlinkingTime = 0f;
	private float maxBlinkingTime = 0f;

	//use this for initialization
	private void Start()
	{
		minBlinkingTime = Time.deltaTime * 2; //minimum blinking time is more or less two frames
		maxBlinkingTime = Time.deltaTime * 10; //maximum blinking time (flickr) is more or less 10 frames
	}

	/// <summary>
	/// Sets the standard material as renderer.
	/// </summary>
	public void SetStandardMaterial()
	{
		if (null != standardMaterial) 
		{
			GetComponent<Renderer> ().material = standardMaterial;
			materialState = MaterialState.Standard; // refresh the actual state of material
		}
	}

	/// <summary>
	/// Sets the standard material to all.
	/// </summary>
	public void SetStandardMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetStandardMaterial();
		}
	}

	/// <summary>
	/// Sets the playback material.
	/// </summary>
	public void SetPlaybackMaterial()
	{
		if (null != playbackMaterial) 
		{
			GetComponent<Renderer> ().material = playbackMaterial;
			materialState = MaterialState.Playback;
		}
	}

	/// <summary>
	/// Sets the playback material to all.
	/// </summary>
	public void SetPlaybackMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetPlaybackMaterial();
		}
	}

	/// <summary>
	/// Sets the negative material.
	/// </summary>
	private void SetNegativeMaterial()
	{
		if (null != negativeMaterial) 
		{
			GetComponent<Renderer> ().material = negativeMaterial;
			materialState = MaterialState.Negative;
		}
	}

	/// <summary>
	/// Sets the negative material to all.
	/// </summary>
	public void SetNegativeMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetNegativeMaterial();
		}
	}

	/// <summary>
	/// Sets the right material according to the current state (use this function for reset after a custom rendering effect).
	/// </summary>
	private void SetRightMaterial ()
	{
		if (materialState == MaterialState.Playback) {
			SetPlaybackMaterial ();
		}
		else {
			SetStandardMaterial ();
		}
	}

	/// <summary>
	/// Sets the right material to all.
	/// </summary>
	public void SetRightMaterialToAll()
	{
		MaterialManager[] allManagers = FindObjectsOfType<MaterialManager> () as MaterialManager[];
		foreach (MaterialManager manager in allManagers) 
		{
			manager.SetRightMaterial();
		}
	}

	/// <summary>
	/// Set the negative material, wait for time and then reset the right material
	/// </summary>
	/// <param name="time">Time.</param>
	private IEnumerator NegativeMaterialAnimation(float time)
	{
		SetNegativeMaterial ();
		yield return new WaitForSeconds(time);
		SetRightMaterial ();
	}

	/// <summary>
	/// Flickr animation
	/// </summary>
	/// <returns>The animation.</returns>
	/// <param name="withNegative">If set to <c>true</c> with negative effect during flickr.</param>
	private IEnumerator FlickrAnimation(bool withNegative)
	{
		Random.seed = (int)Time.time; // seed the random generator
		Renderer renderer = GetComponent<Renderer> (); // get the current renderer
		int nbOfFlickr = Random.Range (NB_MIN_FLICKR, NB_MAX_FLICKR) * 2; // get a random flickr number (*2 because it's always turned invisible then visible)
		float waitTime = 0f;
		bool rendererState = false; //the boolean that say if the renderer should draw to screen or not

		//if renderer has been found on current object
		if (null != renderer) 
		{
			if(withNegative) SetNegativeMaterialToAll(); //if negative animation wanted then set negative material to all

			// flickering loop
			for(int i = 0; i < nbOfFlickr; i++)
			{
				waitTime = Random.Range(minBlinkingTime,maxBlinkingTime); // random waiting time between boundaries

				//80% of chance to switch between turned on and turned off, 20% chance to stay in the actual state
				if(Random.value > 0.20f)
				{
					rendererState = !rendererState; // renderer state inversion
				}
				renderer.enabled = rendererState; //set new renderer state
				yield return new WaitForSeconds(waitTime); // wait for some random time
			}
			renderer.enabled = true; //at the end of the loop we want the model to be visible for sure
			SetRightMaterialToAll(); // reset materials
		}
	}

	/// <summary>
	/// Hides object for A while.
	/// </summary>
	/// <param name="time">Time to hide.</param>
	private IEnumerator HideForAWhile(float time)
	{
		Renderer renderer = GetComponent<Renderer> ();
		if (null != renderer) 
		{
			renderer.enabled = false; // hide
			yield return new WaitForSeconds (time);// wait
			renderer.enabled = true; //show
		}
	}

	/// <summary>
	/// Launchs the negative material animation.
	/// </summary>
	/// <param name="time">Time to negative.</param>
	public void LaunchNegativeMaterialAnimation(float time)
	{
		StartCoroutine(NegativeMaterialAnimation(time));
	}

	/// <summary>
	/// Launchs the flickr animation.
	/// </summary>
	/// <param name="withNegativeEffect">If set to <c>true</c> with negative effect.</param>
	public void LaunchFlickrAnimation(bool withNegativeEffect)
	{
		StartCoroutine (FlickrAnimation (withNegativeEffect));
	}

	/// <summary>
	/// Launchs the hide for A while animation.
	/// </summary>
	/// <param name="time">Time to hide.</param>
	public void LaunchHideForAWhile(float time)
	{
		StartCoroutine (HideForAWhile (time));
	}

	///
	/// the actual material set on.
	///
	public enum MaterialState
	{
		Standard, Negative, Playback
	}
}
