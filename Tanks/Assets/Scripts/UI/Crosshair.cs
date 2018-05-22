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


	private TrailRenderer trail;
	
	// Use this for initialization
	private void Awake()
	{
		hotSpot = new Vector2(crosshairs[index].width/2f,crosshairs[index].height/2f); 
		Cursor.SetCursor(crosshairs[index],hotSpot,CursorMode.Auto);
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity))
		{
			transform.position = hit.point;
		}
	}

//	private IEnumerator ChangeCursor()
//	{
//		index = (index + 1) % crosshairs.Length;
//		Cursor.SetCursor(crosshairs[index],hotSpot,CursorMode.Auto);
//		yield return new WaitForSeconds(4);
//		cursorChanged = true;
//	}
	
	
	
}
