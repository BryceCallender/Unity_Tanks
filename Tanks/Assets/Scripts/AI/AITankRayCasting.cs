using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

public class AITankRayCasting : MonoBehaviour
{

	public float radius;
	
	private EnemyStats stats; //Going to base aiming of its intelligence
	private RaycastHit hitInfo;
	[SerializeField]
	private GameObject[] hitArray;

	private LayerMask tankLayerMask;
	private Vector3 reflectedVector;

	public bool aimingAtItself;
	
	// Use this for initialization
	void Start ()
	{
		stats = gameObject.GetComponent<EnemyStats>();
		tankLayerMask = (1 << 11) | (1 << 10);
		hitArray = new GameObject[stats.ricochetCount + 1];
	}
	
	// Update is called once per frame
	void Update () 
	{
		LineCastToFindEnemy();
		if (stats.tankIntelligence > Intelligence.NORMAL)
		{
			SphereCastForMinesOrBullets();
		}
	}

	//For finding enemy
	private void LineCastToFindEnemy()
	{
		RicochetRayCastRecursion(stats.turretTransform.position,
								 stats.turretTransform.forward,
								 stats.ricochetCount);
	}

	//For avoiding mines and bullets
	private void SphereCastForMinesOrBullets()
	{
		
	}

	private void RicochetRayCastRecursion(Vector3 position,Vector3 direction,int numberOfRicochetsLeft)
	{
		//Only do -1 because the ray hits and doesnt do an extra one so 1 ricochet gets
		//2 raycasts and so on
		if (numberOfRicochetsLeft == -1)
		{
			return;
		}
		else
		{
			if (Physics.Raycast(position,direction, out hitInfo, Mathf.Infinity,tankLayerMask))
			{
				reflectedVector = Vector3.Reflect(direction,hitInfo.normal);
				Debug.DrawLine(position,hitInfo.point,Color.yellow);
				//Just add it to the array from end to front just because
				hitArray[numberOfRicochetsLeft] = hitInfo.collider.gameObject;
				aimingAtItself = AimingAtItself();
				RicochetRayCastRecursion(hitInfo.point,reflectedVector,numberOfRicochetsLeft-1);
			}
		}
	}

	public bool HitEnemy()
	{
		foreach (var item in hitArray)
		{
			if (item.layer == 11)
			{
				return true;
			}
		}
		return false;
	}

	public bool AimingAtItself()
	{
		for (int i = 0; i < hitArray.Length; i++)
		{
			if (hitArray[i] == null)
				continue;
			if (hitArray[i].gameObject.name.Equals(gameObject.name))
			{
				return true;
			}
		}
		return false;
	}

//	private bool HitMineOrBullet()
//	{
//		
//	}
}
