using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Transform camera;

	public Rigidbody2D rb;
	public float SpeedMult = 1;

	void Update () {
		camera.position = transform.position + new Vector3(0, 0, -20);

		rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
