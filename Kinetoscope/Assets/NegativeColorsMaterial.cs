using UnityEngine;
using System.Collections;

public class NegativeColorsMaterial : MonoBehaviour {
	
	public float timeToKeepColorsInverted = 3.0f;
	private bool colorsAlreadyInverted = false;

	public IEnumerator InvertColorsForAWhileRoutine(float time)
	{
		if (!colorsAlreadyInverted) 
		{
			InvertAllMaterialsColor (); // set colors to negative
			colorsAlreadyInverted = true;
			yield return new WaitForSeconds (time);
			colorsAlreadyInverted = false;
			InvertAllMaterialsColor (); // set colors back to real colors
		}
	}

	public void InvertColorsForAWhile()
	{
		StartCoroutine(InvertColorsForAWhileRoutine(timeToKeepColorsInverted));
	}

	public void InvertColorsForAWhile(float time)
	{
		StartCoroutine(InvertColorsForAWhileRoutine(time));
	}

	/// <summary>
	/// Inverts the color but keep the alpha intact.
	/// </summary>
	/// <returns>The inverted color.</returns>
	/// <param name="color">Color to invert</param>
	private Color InvertColor(Color color)
	{
		return new Color (1.0f - color.r, 1.0f - color.g, 1.0f - color.b, color.a);
	}

	/// <summary>
	/// Inverts the color of the all materials in the scene
	/// </summary>
	private void InvertAllMaterialsColor()
	{
		Renderer[] renderers = FindObjectsOfType (typeof(Renderer)) as Renderer[];
		foreach (Renderer render in renderers) 
		{
			if (render.material.HasProperty("_Color")) 
			{
				render.material.color = InvertColor (render.material.color);
			}
		}
	}
}
