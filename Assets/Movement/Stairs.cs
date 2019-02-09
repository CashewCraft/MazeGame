using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
	//use the X axis?
	public bool useX;

	//the bounds of the actual stair part, using the axis as specified above
	public Vector2 Lowest;
	public Vector2 Highest;

	//private versions of the needed axis from above
	float Min;
	float Max;

	//the floor we're going from to the floor we're going to
	public float FromFloor;
	public float ToFloor;

	private void Start()
	{
		//get the start/end positions of the stairs from the Vector2 from the correct axis
		Min = ((useX) ? transform.position.x : transform.position.y) + ((useX) ? Lowest.x : Lowest.y);
		Max = ((useX) ? transform.position.x : transform.position.y) + ((useX) ? Highest.x : Highest.y);

		//register these stairs in the floor manager
		FloorManager.RegisterStairs(transform, Mathf.FloorToInt(FromFloor), Mathf.CeilToInt(ToFloor));
	}

	//runs while an object is inside the trigger
	void OnTriggerStay2D(Collider2D other)
	{
		//if the object is the player
		if (other.tag == "Player")
		{
			//get the exact "floor" the player is on (i.e. halfway up on stairs from floors 1 to 2 should give 1.5)
			float level = Mathf.Lerp(FromFloor, ToFloor, Percent((useX)?other.transform.position.x:other.transform.position.y, Min, Max));
			FloorManager.ins.SetFloor(level); //send the progress to the floor manager to set transparency appropriately
		}
	}

	//convenience method to get the percentage of X between A and B
	float Percent(float x, float a, float b)
	{
		return (x - a) / (b - a);
	}
}