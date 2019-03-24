using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEditor : MonoBehaviour {

	public Node Selected;
	public GameObject Prototype;
	float Level;
	
	void Update ()
	{
		Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButtonDown(0))
		{
			GameObject n = Instantiate(Prototype, (Vector3)MousePos + new Vector3(0, 0, FloorManager.ins.Floors.Length - Level), new Quaternion(1, 0, 0, 0), null);
			n.GetComponent<Node>().Level = Level;
		}
		else if (Input.GetMouseButtonDown(1))
		{
			RaycastHit2D[] hit = Physics2D.RaycastAll(MousePos, Vector3.forward);
			foreach (RaycastHit2D i in hit)
			{
				print(i.transform.name);
				if (i.transform.tag == "Node")
				{
					Node HitNode = i.transform.GetComponent<Node>();
					if (Selected == null)
					{
						Selected = HitNode;
					}
					else
					{
						Selected.Connections.Add(HitNode);
						HitNode.Connections.Add(Selected);

						Selected = null;
					}
					break;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			Level += 0.5f;
			FloorManager.ins.SetFloor(Level);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			Level -= 0.5f;
			FloorManager.ins.SetFloor(Level);
		}
	}
}
