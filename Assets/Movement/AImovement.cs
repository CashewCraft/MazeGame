using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AImovement : MonoBehaviour
{
	float EventTimer = 0;
	public float TimeBetweenMin = 5;
	public float TimeBetweenMax = 10;

	public GameObject Agent;

	public List<DoorLinks> SpawnPoint;

	public static AImovement ins;

	private void Start()
	{
		ins = this;
	}

	void Update()
    {
		if (EventTimer <= 0)
		{
			EventTimer = Random.Range(TimeBetweenMin, TimeBetweenMax);
			Node From = SpawnPoint[Random.Range(0, SpawnPoint.Count)].n;
			Node To = SpawnPoint[Random.Range(0, SpawnPoint.Count)].n;

			GameObject NewAgent = Instantiate(Agent, transform);
			Agent.transform.position = From.transform.position;
			Agent.GetComponent<AIagent>().Current = From;
			Agent.GetComponent<AIagent>().Last = To;
		}
		else
		{
			EventTimer -= Time.deltaTime;
		}
    }
}

[System.Serializable]
public class DoorLinks
{
	public Node n;
	public Door Linked;
}
