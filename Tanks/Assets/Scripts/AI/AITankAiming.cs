using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

public class AITankAiming : MonoBehaviour
{
	public float turnSpeed;
	private bool tracksEnemy;

	[SerializeField]
	private bool hasRandomNumber;
	[SerializeField]
	private bool isFinishedRotating;
	private bool moveLeft;

	
	private float turnTimer;
	private float currentTimer;
	private float randomRot;


	private Vector3 offset;
	
	private Transform turretTransform;
	private EnemyStats stats; //Going to base aiming of its intelligence
	[SerializeField]
	private GameObject enemy;

	// Use this for initialization
	void Start ()
	{
		stats = gameObject.GetComponent<EnemyStats>();
		turretTransform = stats.turretTransform;
		if (stats.tankIntelligence > Intelligence.LOW)
		{
			enemy = GameObject.FindWithTag("Player");
		}

		if (stats.tankIntelligence == Intelligence.NORMAL)
		{
			offset = new Vector3(Random.Range(-3,3),0,Random.Range(-3,3));
		}
			
	}
	
	// Update is called once per frame
	void Update () 
	{
		Aim();
	}

	private void Aim()
	{
		switch (stats.tankIntelligence)
		{
			case Intelligence.LOW: LowIntelAim();
				break;
			case Intelligence.NORMAL: NormalIntelAim();
				break;
			case Intelligence.HIGH: HighIntelAim();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private void LowIntelAim()
	{
		GetNumber();
		Rotate();
	}

	private void NormalIntelAim()
	{
		//They seem to be able to know where the enemy is but they slightly keep rotating 
		//about like 30 degrees contsantly left then back right etc.
		if (enemy != null)
		{
			Vector3 direction = enemy.transform.position - turretTransform.position;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			if (LookAtEnemyOrNot())
			{
				GetNumber();
				turnTimer /= 5;				
				Rotate();
				StartCoroutine(Wait());
			}
			else
			{
				turretTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
			}
			
		}
	}

	private void HighIntelAim()
	{
		if (enemy != null)
		{
			Vector3 direction = enemy.transform.position - turretTransform.position;
			Quaternion lookRotation = Quaternion.LookRotation(direction);
			turretTransform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
		}
	}

	private void GetNumber()
	{
		if (!hasRandomNumber)
		{
			randomRot = Random.Range(0f,1f);
			turnTimer = GetRandomTimer();
			hasRandomNumber = true;
		} 
	}

	private void Rotate()
	{
		if (!isFinishedRotating)
		{			
			//Rotation direction
			if (randomRot < 0.5f)
			{
				turretTransform.Rotate(Vector3.up,turnSpeed * Time.deltaTime);
			}
			else if(randomRot >= 0.5)
			{
				turretTransform.Rotate(Vector3.up,-turnSpeed * Time.deltaTime);
			}
			
			//Timer to rotate
			if ((currentTimer += Time.deltaTime) >= turnTimer)
			{
				currentTimer = 0;
				isFinishedRotating = true;
			}
			
			//Reset
			if (isFinishedRotating)
			{
				StartCoroutine(Reset());
			}
		}
	}
	
	private float GetRandomTimer()
	{
		var timer = Random.Range(0.5f, 1.5f);
		return timer;
	}

	private bool LookAtEnemyOrNot()
	{
		var num = Random.Range(0f,1f);
		return num < 0.5;
	}

	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(5);
	}

	private IEnumerator Reset()
	{
		yield return new WaitForSeconds(1);
		currentTimer = 0;
		isFinishedRotating = false;
		hasRandomNumber = false;
	}
	
}
