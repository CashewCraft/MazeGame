using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
	//static list of all stair objects and which floor objects the connect two - the same stair object will appear twice
	static Dictionary<int, List<Transform>> StairList = new Dictionary<int, List<Transform>>();

	//a static pointer to the current active tracker for use by other scripts
	public static FloorManager ins;

	//a list of all the floor parent objects
	public Transform[] Floors;

	//the current floor we're on
	public int index = 0;

	//debug value to see the current float value of the floor
	public float level;

	private void Start()
	{
		//set this as the active tracker and initialise the floors
		ins = this;
	}

	private void Awake()
	{
		print("Yikes");
		Time.timeScale = 1;
		Init();
	}

	//method to sort the transparency levels of floors and also the floor index based on a given floor
	public void SetFloor(float Level)
	{
		//output the level for debug reasons, since it doesn't play nice with prints for some reason
		level = Level;

		if (Mathf.FloorToInt(Level) != index) //if we're on a different floor the the current one
		{
			//set the current floor to that
			index = Mathf.FloorToInt(Level);
		}
		if (Level < Floors.Length-1) //if we aren't on the top floor
		{
			if (Mathf.Floor(Level) == Level) //and if the level is a whole number
			{
				//set colliders to appropriate values
				ValidateCollidable();

				//set this floor to fully opaque
				SetOpacity(Floors[Mathf.FloorToInt(Level)], 1);

				//and the one above the fully transparent to ensure we can see the current floor
				SetOpacity(Floors[Mathf.FloorToInt(Level) + 1], 0);
			}
			else //if we're between floors (i.e. on stairs)
			{
				//set the opacity of the floor above us to the appropriate fade level
				SetOpacity(Floors[Mathf.FloorToInt(Level) + 1], 1 - (Mathf.Ceil(Level) - Level));
			}
		}
		else //if we *are* on the top floor
		{
			if (Mathf.Floor(Level) == Level) //if the level is a whole number
			{
				//set colliders to appropriate values
				ValidateCollidable();

				//set this floor to fully opaque
				SetOpacity(Floors[Mathf.FloorToInt(Level)], 1);
				//(there's no need to force the opacity of the floor above because there isn't one)
			}
			else //if we're between floors
			{
				//we are climbing stairs to a non-existent floor, what?
				print("What the heck! (player is climbing a stairway to heaven)");
			}
		}
	}

	//convenience method to make all floors transparent, code is self explanitory
	void MakeAllTransparent()
	{
		for (int i = 0; i < Floors.Length; i++)
		{
			SetOpacity(Floors[i],0);
		}
	}

	//disable all colliders on floors other than the current, baring those of stairs connected to this floor
	void ValidateCollidable()
	{
		//go through each floor
		for (int i = 0; i < Floors.Length; i++)
		{
			if (i == index) //if the collider is on the current floor
			{
				SetColliders(Floors[i], true); //set colliders to active
				continue;
			}
			SetColliders(Floors[i], false); //else set colliders to false
		}

		if (StairList.ContainsKey(index)) //if there are stairs connected to the current floor
		{
			foreach (Transform i in StairList[index]) //go through them
			{
				if (i != null)
				{
				Collider2D[] Checking = i.GetComponents<Collider2D>(); //try and get all colliders on the object
				if (Checking != null && (i.GetComponent<Stairs>().ToFloor == index || i.GetComponent<Stairs>().ToFloor < index + 1)) //if there was a collider
				{
					foreach (Collider2D j in Checking)
					{
						j.enabled = true; //enable it
					}
				}
				}
			}
		}
	}

	//initialise the floors to a default state
	void Init()
	{
		// go through all floors
		for (int i = 0; i < Floors.Length; i++)
		{
			//set the position on the Z axis to ensure they're overlapping properly
			Floors[i].position = new Vector3(Floors[i].position.x, Floors[i].position.y, Floors.Length - i);
		}
		MakeAllTransparent(); //make them all transparent
		ValidateCollidable(); //disable all colliders that aren't supposed to be on

		//go though all floors beneath and including the current one
		for (int i = 0; i < index+1; i++)
		{
			SetOpacity(Floors[i], 1); //make them fully visible
		}
	}

	//method to recursively set the transparency of an object and it's children
	void SetOpacity(Transform Object, float to)
	{
        if (Object.GetComponent<MeshRenderer>() != null)
        {
            Object.GetComponent<MeshRenderer>().material.color = new Color(to, to, to, to);
        }

		//go through all children
		for (int i = 0; i < Object.childCount; i++)
		{
			//set their transparency to the same
			SetOpacity(Object.GetChild(i), to);
		}
	}

	//method to recursively enable/disable the colliders of an object and it's children
	void SetColliders(Transform Object, bool to)
	{
		//try and get all attached colliders
		Collider2D[] Checking = Object.GetComponents<Collider2D>();

		//check if they exist
		if (Checking != null)
		{
			foreach (Collider2D i in Checking)
			{
				//go through them and enable/disable them based on argument
				i.enabled = to;
			}
		}
		if (Object.tag == "Door")
		{
			Object.GetComponent<Door>().SetVisible(to);
		}
		if (Object.tag == "Audio")
		{
			Object.gameObject.SetActive(to);
		}

		//go through children
		for (int i = 0; i < Object.childCount; i++)
		{
			//run method on them
			SetColliders(Object.GetChild(i), to);
		}
	}

	//public method for stairs to register themselves
	public static void RegisterStairs(Transform a, int f1, int f2)
	{
		//static variables don't make new lists automatically, so do that here if needed
		if (!StairList.ContainsKey(f1))
		{
			StairList.Add(f1, new List<Transform>());
		}
		if (!StairList.ContainsKey(f2))
		{
			StairList.Add(f2, new List<Transform>());
		}

		//add the stairs to the list for the two floors it's connecting
		StairList[f1].Add(a);
		StairList[f2].Add(a);
	}

	public float GetOpacity(float Pos)
	{
		if (Pos - level >= 1)
		{
			return 0;
		}
		else if (Pos - level <= 0)
		{
			return 1;
		}
		else
		{
			return 1 - ((Pos - level) / 1);
		}
	}
}
