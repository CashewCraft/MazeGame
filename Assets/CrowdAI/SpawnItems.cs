using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour {

public GameObject[] Coins;
	public Transform[] SpawnPoints;
	public float spawnTime = 1.5f;

	
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnCoins", spawnTime, spawnTime);
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	void SpawnCoins()
	{
		int spawnIndex = Random.Range(0, SpawnPoints.Length);

		int objectIndex = Random.Range(0, Coins.Length);
		Instantiate(Coins[objectIndex], SpawnPoints [spawnIndex].position, SpawnPoints [spawnIndex].rotation);
	}
}