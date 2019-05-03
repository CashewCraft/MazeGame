using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class stepCalling : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] steps;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Step()
    {
        Debug.Log("I HAVE STEPPED");
        AudioClip clip = steps[Random.Range(0, steps.Length-1)];
        audioSource.PlayOneShot(clip);
    }
}
