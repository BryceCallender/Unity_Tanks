using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
	public Texture2D[] crosshairs;

	private Vector2 hotSpot;
	private Vector3 currentPosition;
	private Vector3 lastPosition;

	private RaycastHit hit;

	private int index = 0;

	private bool cursorChanged;
	//Make a good amount of textures where the color is changing downwards
	//so we can get the effect of the color moving

	// Use this for initialization
	private void Awake()
	{
		hotSpot = new Vector2(crosshairs[index].width/2f,crosshairs[index].height/2f); 
		Cursor.SetCursor(crosshairs[index],hotSpot,CursorMode.Auto);
	}

//	private void Update()
//	{
//		if (!cursorChanged)
//		{
//			StartCoroutine(ChangeCursor());
//			cursorChanged = false;
//		}
//		
//	}

//	private IEnumerator ChangeCursor()
//	{
//		index = (index + 1) % crosshairs.Length;
//		Cursor.SetCursor(crosshairs[index],hotSpot,CursorMode.Auto);
//		yield return new WaitForSeconds(4);
//		cursorChanged = true;
//	}
	
}
