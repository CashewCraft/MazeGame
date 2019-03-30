using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour {

	public static Node Selected;
	public GameObject Prototype;
	public static float Level;
	public Transform ParentObject;
	public static bool Active = false;

	private List<GameObject> CreatedNodes = new List<GameObject>();

	/*
		HOW TO USE THIS SCRIPT:

		-Create a new node by pressing LMB
		-Select it by pressing RMB
		-Use RMB on a node while having another selected to create a link

		-Use UP and DOWN to adjust the height of the camera in half floor steps
		-Use S to remove any colliders from created nodes (do this before copying them into the main scene)

		-You can only select nodes that you are on the same floor as
	*/

	private void Start()
	{
		Active = true;
	}

	void Update ()
	{

		Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //Get the position of the mouse

		if (Input.GetMouseButtonDown(0)) //if LMB is pressed
		{
			//Create a new object, set it's level and add it to the list of created objects
			GameObject n = Instantiate(Prototype, (Vector3)MousePos + new Vector3(0, 0, FloorManager.ins.Floors.Length - Level), new Quaternion(1, 0, 0, 0), ParentObject);
			n.GetComponent<Node>().Level = Level;
			CreatedNodes.Add(n);
		}
		else if (Input.GetMouseButtonDown(1)) //if RMB is pressed
		{
			RaycastHit2D[] hit = Physics2D.RaycastAll(MousePos, Vector3.forward); //get all collisions with the mouse
			foreach (RaycastHit2D i in hit) //Go through each collision
			{
				if (i.transform.tag == "Node" && i.transform.GetComponent<Node>().Level == Level) //if it is a node and it's on our level
				{
					Node HitNode = i.transform.GetComponent<Node>(); //Save the copy of the node for easy referencing
					if (Selected == null) //if nothing was previously selected
					{
						Selected = HitNode; //make this the selected object
					}
					else if (HitNode == Selected) //if we've selected the same thing again
					{
						Selected = null;
					}
					else //if there was something selected
					{
						Selected.Connections.Add(HitNode); //make connections
						HitNode.Connections.Add(Selected);

						Selected = null; //remove whatever was selected
					}
					break; //we've found a valid node, no longer need to loop through everything else
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Level += 0.5f; //up arrow moves us up half a floor
			FloorManager.ins.SetFloor(Level);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Level -= 0.5f; //down arrow moves us down half a floor
			FloorManager.ins.SetFloor(Level);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			//S removes any colliders from the created nodes
			foreach (GameObject i in CreatedNodes)
			{
				if (i.GetComponent<Collider2D>() != null)
				{
					Destroy(i.GetComponent<Collider2D>());
				}
			}
		}
	}
}
