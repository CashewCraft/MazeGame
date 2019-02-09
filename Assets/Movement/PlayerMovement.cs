using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//the rigidbody of the player, used for quick reference
	public Rigidbody2D rb;

	//a multiplier applied to the speed of the player
	public float SpeedMult = 1;

	void Update ()
	{
		//set the position of the main camera
		Camera.main.transform.position = transform.position + new Vector3(0, 0, -20);

		//Make the player move towards the direction the controller is pointing
		rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		//get the difference between the mouse position and our position in screenspace
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

		//use atan2 to get the angle between the two
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		//set the rotation to be the calculated angle rotated around the z axis
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
