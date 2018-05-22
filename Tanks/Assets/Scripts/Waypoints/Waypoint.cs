using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public float radiusOfInfluence;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position,radiusOfInfluence);
	}

	private void OnCollisionEnter(Collision other)
	{
		other.collider.enabled = false;
	}

	private void OnCollisionExit(Collision other)
	{
		other.collider.enabled = true;
	}
}
