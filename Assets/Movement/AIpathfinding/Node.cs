using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public List<Node> Connections = new List<Node>();
	public float Level;

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 0.03f);

		Gizmos.color = Color.green;
		foreach (Node i in Connections)
		{
			Gizmos.DrawLine(transform.position, i.transform.position);
		}
#endif
	}
}