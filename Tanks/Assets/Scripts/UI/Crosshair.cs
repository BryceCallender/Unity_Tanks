using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
	public Texture2D crosshair;

	private Vector2 hotSpot;
	private Vector3 currentPosition;
	private Vector3 lastPosition;

	private RaycastHit hit;
	
	//Make a good amount of textures where the color is changing downwards
	//so we can get the effect of the color moving

	// Use this for initialization
	private void Awake()
	{
		hotSpot = new Vector2(crosshair.width/2f,crosshair.height/2f); 
		Cursor.SetCursor(crosshair,hotSpot,CursorMode.Auto);
	}

//	private void Update()
//	{
//		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//	
//	}
	
}
