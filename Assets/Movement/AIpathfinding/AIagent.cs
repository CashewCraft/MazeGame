using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIagent : MonoBehaviour {
	public float Speed;

	public Rigidbody2D rb;

	public Node Last;
	public Node Current;
	private Stack<Node> Path = new Stack<Node>();
	public Node[] ToPath;

	public float ReachDistance = 0.01f;
	public float AvoidDist = 1;

	public float Level = 0;

	bool CanCollide = true;
	bool Initalising = true;

	List<Transform> CurrentCollided = new List<Transform>();

	public bool GenPathTo(Node Destination)
	{
		print("Trying to generate a path from " + Destination.name + " to " + Current.name);

		//Create new lists for the open and closed nodes and put the current node into the open nodes
		List<PathfindingNodeInfo> Closed = new List<PathfindingNodeInfo>();
		List<PathfindingNodeInfo> Open = new List<PathfindingNodeInfo>
		{
			new PathfindingNodeInfo(Destination, Current, null)
		};

		//Set the target as the current node so it can be accessed when the loop ends
		PathfindingNodeInfo Target = Open[0];

		//Keep looping until we find the destingation
		while (Target.Represent != Destination)
		{
			if (Open.Count == 0) //if there are not more open nodes
			{
				print("Could not find destination in the graph");
				return false; //end the function since the desination is not reachable from the current node
			}

			//set the lowest cost as the first element's cost so we don't end up repeating closed nodes
			Target = Open[0];
			float LowestCost = Open[0].Fcost;

			//Go through every open node
			foreach (PathfindingNodeInfo i in Open)
			{
				if (i.Fcost < LowestCost) //if the current node's cost is lower than the previous lowest
				{
					Target = i; //set this as the new lowest
					LowestCost = i.Fcost;
				}
			}

			//Get all connected nodes and check through them
			List<Node> Connections = Target.Represent.Connections;
			for (int Edge = 0; Edge < Connections.Count; Edge++)
			{
				//Create a variable to check if we found a match
				bool FoundMatch = false;

				//Check through all the closed nodes
				foreach (PathfindingNodeInfo i in Closed)
				{
					//if the current connection is in closed
					if (i.Represent == Connections[Edge])
					{
						FoundMatch = true;
						break; //Flag that there was a match and stop looping
					}
				}

				if (!FoundMatch) //if there wasn't a match in the closed nodes
				{
					for (int i = 0; i < Open.Count; i++) //check through the open nodes
					{
						if (Open[i].Represent == Connections[Edge]) //if the current info is representing the current node
						{
							FoundMatch = true; //flag that there was a match

							//create a new info to check the weights
							PathfindingNodeInfo NewRoute = new PathfindingNodeInfo(Destination, Connections[Edge], Target);

							if (NewRoute.Fcost < Open[i].Fcost) //if the new cost is less than the old cost
							{
								Open[i] = NewRoute; //set the new info as the one representing this node
							}
							break; //stop looping
						}
					}
				}

				if (!FoundMatch && !Connections[Edge].Blocked) //if neither of the last two steps found a match and the connection isn't blocked
				{
					//add this node to the open nodes
					Open.Add(new PathfindingNodeInfo(Destination, Connections[Edge], Target));
				}
			}

			Open.Remove(Target); //move the target from open into closed
			Closed.Add(Target);
		}
		
		//loop until there is no previous node (we reached the begining)
		while (Target.Previous != null)
		{
			//add this node to the path and go back to it's parent
			Path.Push(Target.Represent);
			Target = Target.Previous;
		}
		return true;
	}

	private void Update()
	{
		Vector2 SteerDirection = new Vector2();

		try
		{
			//set the intial velocity to be towards the target
			SteerDirection = ((Vector2)(Current.gameObject.transform.position - transform.position)).normalized * Speed;
		}
		catch
		{
			Destroy(gameObject);
			return;
		}

		//Apply any steering needed for collision avoidance
		SteerDirection += AvoidDanger();

		Vector2 SteerForce = Vector2.ClampMagnitude(SteerDirection, 0.1f);

		//Apply Steering force to the velocity and clamp to speed
		rb.velocity = Vector2.ClampMagnitude(rb.velocity + SteerForce, Speed);

		//Look towards target
		Vector2 MyPos = transform.position;
		Vector2 Target = rb.position + rb.velocity;
		float angleRad = Mathf.Atan2(MyPos.y - Target.y, MyPos.x - Target.x);

		transform.rotation = Quaternion.AngleAxis((angleRad * Mathf.Rad2Deg) + 90, Vector3.forward);

		//if we have reached the current node and the path isn't empty
		if (Vector2.Distance(Current.gameObject.transform.position, transform.position) < ReachDistance && Path.Count > 0)
		{
			Last = Current; //Make the current into the last
			Current = Path.Pop(); //get the next step in the path
		}
		else if (Vector2.Distance(Current.gameObject.transform.position, transform.position) < ReachDistance && Path.Count == 0 && !Initalising)
		{
			Destroy(gameObject);
			return;
		}
		else if (Initalising)
		{
			Initalising = false;
			GenPathTo(Last);
			Last = Current;
		}

		//Get the distance between the last and the current node
		float Dist = Vector2.Distance(Last.transform.position, Current.transform.position);

		//interpolate the current level based on the level difference between the two nodes and how far we are between them
		Level = Mathf.Lerp(Last.Level, Current.Level, ((Dist - Vector2.Distance(transform.position, Current.transform.position)) / Dist));

		if (Mathf.Abs(FloorManager.ins.level - Level) > 0.5f && CanCollide)
		{
			CanCollide = false;
			foreach (Collider2D Object in GetComponents<Collider2D>())
			{
				Object.enabled = false;
			}
		}
		else if (Mathf.Abs(FloorManager.ins.level - Level) <= 0.5f && !CanCollide)
		{
			CanCollide = true;
			foreach (Collider2D Object in GetComponents<Collider2D>())
			{
				Object.enabled = true;
			}
		}

		//set the Z of the current position to one above the current level
		transform.position = new Vector3(transform.position.x, transform.position.y, FloorManager.ins.Floors.Length - Mathf.Floor(Level) - 0.6f);

		//hide if the player shouldn't be able to see us
		if (Mathf.Abs(FloorManager.ins.level - Level) >= 0.8f)
		{
			transform.GetChild(0).gameObject.SetActive(false);
		}
		else
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}

	Vector2 AvoidDanger()
	{
		Debug.DrawRay(transform.position, rb.velocity * 2);

		Transform hit = null;
		foreach (Transform H in CurrentCollided)
		{
			if ((hit == null || Vector2.Distance(transform.position, H.position) < Vector2.Distance(transform.position, hit.position)))
			{
				hit = H;
			}
		}

		if (hit != null)
		{
			Vector2 ray = (rb.velocity - rb.position).normalized * Vector2.Dot(hit.transform.position - transform.position, (rb.velocity - rb.position).normalized);
			Debug.DrawLine(transform.position, (Vector2)transform.position + ray, Color.green);

			Vector2 Avoid = ((Vector2)transform.position + ray) - (Vector2)hit.transform.position;
			Debug.DrawLine(hit.transform.position, (Vector2)hit.transform.position + (Avoid.normalized * AvoidDist), Color.magenta);

			return Avoid.normalized * AvoidDist;
		}

		return Vector2.zero;
	}

	//convenience method to get the percentage of X between A and B
	float Percent(float x, float a, float b)
	{
		return (x - a) / (b - a);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" || collision.tag == "Enemy")
		{
			print("Adding "+collision.name);
			CurrentCollided.Add(collision.transform);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player" || collision.tag == "Enemy")
		{
			CurrentCollided.RemoveAll(c => (c == collision.transform));
			print("Removing " + collision.name);
		}
	}

	void SetVisibilty(Transform Object, float to)
	{
		if (Object.GetComponent<MeshRenderer>() != null)
		{
			Material old = Object.GetComponent<MeshRenderer>().material;
			old.color = new Color(old.color.r, old.color.g, old.color.b, to);
		}

		//go through all children
		for (int i = 0; i < Object.childCount; i++)
		{
			//set their transparency to the same
			SetVisibilty(Object.GetChild(i), to);
		}
	}
}

//Class to store information about known graph nodes when pathfinding
public class PathfindingNodeInfo
{
	public float Fcost; //F cost
	public float Hcost; //H cost
	public Node Represent; //The node this is representing
	public PathfindingNodeInfo Previous; //The previous node in the sequence

	public PathfindingNodeInfo(Node Target, Node This, PathfindingNodeInfo Parent)
	{
		Fcost = ((Parent == null) ? 0 : Parent.Hcost) + Vector3.Distance(This.transform.position, Target.transform.position);
		Hcost = (Parent == null) ? 0 : Parent.Hcost + Vector3.Distance(This.transform.position, Parent.Represent.transform.position);
		Represent = This;
		Previous = Parent;
	}
}
