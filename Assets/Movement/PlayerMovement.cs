using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//the rigidbody of the player, used for quick reference
	public Rigidbody2D rb;

	public Animator Model;

	//a multiplier applied to the speed of the player
	public float SpeedMult = 1;

	void Update ()
	{
		//set the position of the main camera
		Camera.main.transform.position = transform.position + new Vector3(0, 0, -20);

		//Make the player move towards the direction the controller is pointing
		rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * SpeedMult;
      
		if (Input.GetAxis("Vertical") > 0|| Input.GetAxis("Vertical") < 0 || Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
		{
			//Model.SetBool("Walking", true);

            if(Input.GetKey("space") == true)
            {
                Model.SetBool("Walking", false);
                Model.SetBool("Running", true);
                Model.SetBool("Stairs", false);

            }
            if (Input.GetKey(KeyCode.LeftShift) == true)
            {
                Model.SetBool("Walking", false);
                Model.SetBool("Running", false);
                Model.SetBool("Stairs", true);

            }
            if((Input.GetKey(KeyCode.LeftShift) == false) && (Input.GetKey("space") == false))
            {
                Model.SetBool("Walking", true);
                Model.SetBool("Running", false);
                Model.SetBool("Stairs", false);

            }
		}
		else
		{
			Model.SetBool("Walking", false);
            Model.SetBool("Running", false);
            Model.SetBool("Stairs", false);
        }

		//get the difference between the mouse position and our position in screenspace
		Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

		//use atan2 to get the angle between the two
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

		//set the rotation to be the calculated angle rotated around the z axis
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		if (rb.velocity.magnitude > 0)
		{
			//Put animation stuff in here
		}
	}

	float GetTimeToNode(Node Start, Node End, float UnitsPerSecond)
	{
		//Create new lists for the open and closed nodes and put the current node into the open nodes
		List<PathfindingNodeInfo> Closed = new List<PathfindingNodeInfo>();
		List<PathfindingNodeInfo> Open = new List<PathfindingNodeInfo>
		{
			new PathfindingNodeInfo(End, Start, null)
		};

		//Set the target as the current node so it can be accessed when the loop ends
		PathfindingNodeInfo Target = Open[0];

		//Keep looping until we find the destingation
		while (Target.Represent != End)
		{
			if (Open.Count == 0) //if there are not more open nodes
			{
				print("Could not find destination in the graph");
				return Mathf.Infinity; //end the function since the desination is not reachable from the current node
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
							PathfindingNodeInfo NewRoute = new PathfindingNodeInfo(End, Connections[Edge], Target);

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
					Open.Add(new PathfindingNodeInfo(End, Connections[Edge], Target));
				}
			}

			Open.Remove(Target); //move the target from open into closed
			Closed.Add(Target);
		}

		//return the distance between the start and end divided by our speed
		return Target.Hcost / UnitsPerSecond;
	}
}
