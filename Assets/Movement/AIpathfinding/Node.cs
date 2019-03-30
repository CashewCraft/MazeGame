using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public List<Node> Connections = new List<Node>();
	public float Level;

	public void Start()
	{
		if (!NodeEditor.Active)
		{
			GetComponent<Collider2D>().enabled = false;
		}
	}

	private void OnDrawGizmos()
	{
#if UNITY_EDITOR
		if (Level == NodeEditor.Level)
		{
			Gizmos.color = (NodeEditor.Selected == this)?Color.yellow:Color.red;
			Gizmos.DrawSphere(transform.position, (NodeEditor.Selected == this) ? 0.05f : 0.03f);

			Gizmos.color = Color.green;
			foreach (Node i in Connections)
			{
				Gizmos.DrawLine(transform.position, i.transform.position);
			}
		}
#endif
	}
}