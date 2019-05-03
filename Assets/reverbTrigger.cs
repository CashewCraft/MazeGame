using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reverbTrigger : MonoBehaviour
{
    public reverbControl revControl;
    public int reverbID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player has passed through " + gameObject.name);
        revControl.blendSnapshot(reverbID);
    }
}
