using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
	public bool useX;

	public Vector2 Lowest;
	public Vector2 Highest;

	float Min;
	float Max;

	public float FromFloor;
	public float ToFloor;

	private void Start()
	{
		Min = ((useX) ? transform.position.x : transform.position.y) + ((useX) ? Lowest.x : Lowest.y);
		Max = ((useX) ? transform.position.x : transform.position.y) + ((useX) ? Highest.x : Highest.y);

		FloorManager.RegisterStairs(transform, Mathf.FloorToInt(FromFloor), Mathf.CeilToInt(ToFloor));
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			float level = Mathf.Lerp(FromFloor, ToFloor, Percent((useX)?other.transform.position.x:other.transform.position.y, Min, Max));
			FloorManager.ins.SetFloor(level);
		}
	}

	float Percent(float x, float a, float b)
	{
		return (x - a) / (b - a);
	}
}