using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
	static Dictionary<int, List<Transform>> StairList= new Dictionary<int, List<Transform>>();

	public static FloorManager ins;

	public Transform[] Floors;
	public int index = 0;

	public float debug;

	private void Start()
	{
		ins = this;
		Init();
	}

	public void SetFloor(float Level)
	{

		debug = Level;
		if (Mathf.FloorToInt(Level) != index)
		{
			index = Mathf.FloorToInt(Level);
			//PlayerMovement.Player.position = new Vector3(PlayerMovement.Player.position.x, PlayerMovement.Player.position.y, -(index+1));
		}
		if (Level < Floors.Length-1) //if we aren't on the top floor
		{
			if (Mathf.Floor(Level) == Level) //if the level is a whole number
			{
				ValidateCollidable();

				SetOpacity(Floors[Mathf.FloorToInt(Level)], 1);
				SetOpacity(Floors[Mathf.FloorToInt(Level) + 1], 0);

				
			}
			else
			{
				SetOpacity(Floors[Mathf.FloorToInt(Level) + 1], 1 - (Mathf.Ceil(Level) - Level));
			}
		}
		else //if we *are* on the top floor
		{
			if (Mathf.Floor(Level) == Level) //if the level is a whole number
			{
				ValidateCollidable();

				SetOpacity(Floors[Mathf.FloorToInt(Level)], 1);
				//there's no need to force the opacity of the floor above because there isn't one
			}
			else
			{
				SetOpacity(Floors[Mathf.FloorToInt(Level) + 1], 1 - (Mathf.Ceil(Level) - Level));
			}
		}
	}

	void MakeAllTransparent()
	{
		for (int i = 0; i < Floors.Length; i++)
		{
			SetOpacity(Floors[i],0);
		}
	}

	void ValidateCollidable()
	{
		for (int i = 0; i < Floors.Length; i++)
		{
			if (i == index)
			{
				SetColliders(Floors[i], true);
				continue;
			}
			SetColliders(Floors[i], false);
		}

		if (StairList.ContainsKey(index))
		{
			foreach (Transform i in StairList[index])
			{
				Collider2D[] Checking = i.GetComponents<Collider2D>();
				if (Checking != null)
				{
					foreach (Collider2D j in Checking)
					{
						j.enabled = true;
					}
				}
			}
		}
	}

	void Init()
	{
		for (int i = 0; i < Floors.Length; i++)
		{
			Floors[i].position = new Vector3(Floors[i].position.x, Floors[i].position.y, Floors.Length - i);
		}
		MakeAllTransparent();
		ValidateCollidable();
		for (int i = 0; i < index+1; i++)
		{
			SetOpacity(Floors[i], 1);
		}
	}

	void SetOpacity(Transform Object, float to)
	{
		Object.GetComponent<SpriteRenderer>().color = new Color(1,1,1,to);
		for (int i = 0; i < Object.childCount; i++)
		{
			SetOpacity(Object.GetChild(i), to);
		}
	}

	void SetColliders(Transform Object, bool to)
	{
		Collider2D[] Checking = Object.GetComponents<Collider2D>();
		if (Checking != null)
		{
			foreach (Collider2D i in Checking)
			{
				i.enabled = to;
			}
		}
		for (int i = 0; i < Object.childCount; i++)
		{
			SetColliders(Object.GetChild(i), to);
		}
	}

	public static void RegisterStairs(Transform a, int f1, int f2)
	{
		if (!StairList.ContainsKey(f1))
		{
			StairList.Add(f1, new List<Transform>());
		}
		if (!StairList.ContainsKey(f2))
		{
			StairList.Add(f2, new List<Transform>());
		}
		StairList[f1].Add(a);
		StairList[f2].Add(a);
	}
}
