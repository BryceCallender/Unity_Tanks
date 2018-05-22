using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AITankMovement : MonoBehaviour
{
	private EnemyStats stats;
	[SerializeField]
	private WaypointManager waypointManager;
	[SerializeField]
	private GameObject waypointToGoTo;

	private NavMeshAgent agent;
	
	// Use this for initialization
	private void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		waypointManager = WaypointManager.Instance;
		stats = gameObject.GetComponent<EnemyStats>();

		agent.autoBraking = false;

		//If the tank is immobile then make sure it doesnt move
		if (stats.tankSpeed == TankSpeed.IMMOBILE)
		{
			agent.speed = 0;
		}
	}
	
	// Update is called once per frame
	private void Update ()
	{
		LowIntelMovement();
	}

	private void LowIntelMovement()
	{
		if (IsAgentAtDestination())
		{
			waypointToGoTo = RandomWaypointInRadius();
			Vector3 randomDestination = waypointToGoTo.transform.position + (Random.insideUnitSphere * stats.rangeForWaypointGrabbing);
			randomDestination.y = 0;
			agent.destination = randomDestination;
		}
		Debug.DrawLine(gameObject.transform.position,agent.destination,Color.magenta);
	}

	private GameObject RandomWaypointInRadius()
	{
		Collider[] hitWayPoints = waypointManager.GetWaypointsNear(gameObject.transform, stats.rangeForWaypointGrabbing);
		int randomChoice = Random.Range(0, hitWayPoints.Length);
		return hitWayPoints[randomChoice].gameObject;
	}

	private bool IsAgentAtDestination()
	{
		return agent.remainingDistance < 0.1;
	}

	//Maybe implement where it turns its base in the direction of the way it is going then proceed to move in that 
	//direction
	public IEnumerator WaitTillRotatedBase()
	{
		yield return null;
	}
	
	
}
