using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AITankShooting : MonoBehaviour
{
	public Rigidbody bullet;
	public List<GameObject> bullets;

	private int bulletCount;
	
	//Bullet Shooting calculations
	[SerializeField]
	private float randomShootTimer;
	private float maxHoldingThreshold;
	[SerializeField]
	private bool decidedOnTime;
	
	private EnemyStats stats;
	private AITankRayCasting aiRayCast;
	
	// Use this for initialization
	void Start ()
	{
		stats = GetComponent<EnemyStats>();
		aiRayCast = GetComponent<AITankRayCasting>();
		bulletCount = stats.bulletCount;
		bullets = new List<GameObject>();
		maxHoldingThreshold = 8f; //Can hold bullet for this many seconds otherwise lets just shoot
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bullets.Count < bulletCount)
		{
			DetermineFireForAi();
			if (!aiRayCast.AimingAtItself() && (randomShootTimer -= Time.deltaTime) <= 0)
			{
				Fire();
			}
				
		}
	}

	private void Fire()
	{
		// Create an instance of the shell and store a reference to it's rigidbody.
		Rigidbody shellInstance = Instantiate(bullet, 
											  stats.turretEndTransform.position, 
											  stats.turretEndTransform.rotation) as Rigidbody;

		bullets.Add(shellInstance.gameObject);
		shellInstance.GetComponent<Bullet>().owner = gameObject;
		decidedOnTime = false;
	}

	private void SetRandomShootTimer()
	{
		if (!decidedOnTime)
		{
			randomShootTimer = Random.Range(5f, maxHoldingThreshold);
			decidedOnTime = true;
		}	
	}

	private void DetermineFireForAi()
	{
		switch (stats.tankIntelligence)
		{
			case Intelligence.PLAYER:
				break;
			case Intelligence.LOW: SetRandomShootTimer();
				break; 
			case Intelligence.NORMAL:
				break;
			case Intelligence.HIGH:
				randomShootTimer = 0;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}
