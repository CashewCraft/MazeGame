using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIagent : MonoBehaviour {
	public float Speed;

	public Rigidbody2D rb;
	public Vector2 V;

	public Node Last;
	public Node Current;

	Stack<Node> Path;
	public Node[] ToPath;

	public float Debug;

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
			Last = Current;
			Current = Path.Pop();
		}

		float Dist = Vector2.Distance(Last.transform.position, Current.transform.position);
		float level = Mathf.Lerp(Last.Level, Current.Level, ((Dist - Vector2.Distance(transform.position, Current.transform.position)) / Dist));
		Debug = level;
		transform.position = new Vector3(transform.position.x, transform.position.y, FloorManager.ins.Floors.Length - Mathf.Floor(level) - 1.1f);
		
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.g, FloorManager.ins.GetOpacity(level));
	}

	//convenience method to get the percentage of X between A and B
	float Percent(float x, float a, float b)
	{
		return (x - a) / (b - a);
	}
}
