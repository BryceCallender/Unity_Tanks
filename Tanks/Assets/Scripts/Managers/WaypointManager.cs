using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
	public Transform[] wayPoints;
	
	// Use this for initialization
	void Start () 
	{
		
		wayPoints = new Transform[transform.childCount];
		
		for (int i = 0; i < transform.childCount; i++)
		{
			wayPoints[i] = transform.GetChild(i).transform;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public Transform[] GetClosestToFurthest(Transform currentTransform)
	{
		Transform[] returnArray = wayPoints;
		returnArray = wayPoints.OrderBy(wayPoint => Vector3.Distance(currentTransform.position,
																	 wayPoint.transform.position)).ToArray();
		return returnArray;
	}

	public Transform[] GetClosestToFurthest(Transform currentTransform, int amount)
	{
		Transform[] returnArray = wayPoints;
		returnArray = wayPoints.OrderBy(wayPoint => Vector3.Distance(currentTransform.position,
			wayPoint.transform.position)).ToArray();

		Transform[] temp = new Transform[amount];
		Array.Copy(returnArray,temp,amount);
		return temp;
	}

	public Collider[] GetWaypointsNear(Transform currentTransform, float range)
	{
		LayerMask waypointMask = 1 << 12;
		Collider[] collisions = Physics.OverlapSphere(currentTransform.position, range, waypointMask);
		return collisions;
	}
}
