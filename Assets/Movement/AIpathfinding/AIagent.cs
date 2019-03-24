using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIagent : MonoBehaviour {
	public float Speed;

	public Rigidbody2D rb;
	public Vector2 V;

	public Node Last;
	public Node Current;
	private Stack<Node> Path = new Stack<Node>();
	public Node[] ToPath;

	public float Debug;

	void GenPathTo(Node Destination)
	{
		int Deadman = 0;

		print("Begining pathfinding");

		List<PathfindingNodeInfo> Closed = new List<PathfindingNodeInfo>();
		List<PathfindingNodeInfo> Open = new List<PathfindingNodeInfo>
		{
			new PathfindingNodeInfo(0, Destination, Current, null)
		};

		PathfindingNodeInfo Target = Open[0];
		while (Target.Represent != Destination && Deadman < 20)
		{
			Deadman++;

			print("Begining to look for lowest cost open node");
			float LowestCost = Open[0].Fcost+1;
			foreach (PathfindingNodeInfo i in Open)
			{
				if (i.Fcost < LowestCost)
				{
					print("Found new lowest cost target with a value of " + i.Fcost);
					Target = i;
					LowestCost = i.Fcost;
				}
			}
			print("Done looking for target with lowest cost");

			List<Node> Connections = Target.Represent.Connections;
			for (int Edge = 0; Edge < Connections.Count; Edge++)
			{
				bool FoundMatch = false;

				foreach (PathfindingNodeInfo i in Closed)
				{
					if (i.Represent == Connections[Edge])
					{
						FoundMatch = true;
						break;
					}
				}

				if (!FoundMatch)
				{
					for (int i = 0; i < Open.Count; i++)
					{
						if (Open[i].Represent == Connections[Edge]) //if the current info is representing the current node
						{
							FoundMatch = true;
							PathfindingNodeInfo NewRoute = new PathfindingNodeInfo(Target.Hcost, Destination, Connections[Edge], Target);
							if (NewRoute.Fcost < Open[i].Fcost)
							{
								Open[i] = NewRoute;
							}
							break;
						}
					}
				}

				if (!FoundMatch)
				{
					Open.Add(new PathfindingNodeInfo(Target.Hcost, Destination, Connections[Edge], Target));
				}
			}

			Open.Remove(Target);
			Closed.Add(Target);
		}
		
		while (Target.Previous != null)
		{
			Path.Push(Target.Represent);
			Target = Target.Previous;
		}
	}

	private void Start()
	{
		//foreach (Node i in ToPath)
		//{
		//	Path.Push(i);
		//}
		GenPathTo(Last);
		Last = Current;
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

public class PathfindingNodeInfo
{
	public float Fcost;
	public float Hcost;
	public Node Represent;
	public PathfindingNodeInfo Previous;

	public PathfindingNodeInfo(float Hops, Node Target, Node This, PathfindingNodeInfo Parent)
	{
		MonoBehaviour.print("Creating new info with hops of " + Hops);
		Fcost = Hops + Vector3.Distance(This.transform.position, Target.transform.position);
		Hcost = (Parent == null) ? 0 : Hops + Vector3.Distance(This.transform.position, Parent.Represent.transform.position);
		Represent = This;
		Previous = Parent;
	}
}
