using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIagent : MonoBehaviour {
	public float Speed;

	public Rigidbody2D rb;
	public Vector2 V;

	public Node Current;

	Stack<Node> Path;
	public Node[] ToPath;

	void GenPathTo(Node Destination)
	{
		//PlaceHolder
	}

	private void Start()
	{
		Path = new Stack<Node>();
		foreach (Node i in ToPath)
		{
			Path.Push(i);
		}
	}

	private void Update()
	{
		rb.velocity = ((Vector2)(Current.gameObject.transform.position - transform.position)).normalized * Speed;
		V = ((Vector2)(Current.gameObject.transform.position - transform.position)).normalized * Speed;

		if (Vector2.Distance(Current.gameObject.transform.position, transform.position) < 0.01f && Path.Count > 0)
		{
			Current = Path.Pop();
		}
	}
}
