using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public string Name;
	public TextMesh Text;
	
    void Start()
    {
		Text.text = Name;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<PlayerMovement>().CurrentRoom = Name;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			collision.GetComponent<PlayerMovement>().CurrentRoom = "";
		}
	}

	public void SetVisible(bool to)
	{
		Text.color = (to)? new Color(0, 0, 0, 1): new Color(1, 1, 1, 0);
	}
}
