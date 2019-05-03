using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class backgroundMusicController : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] backgroundSong;
    bool once = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(backgroundSong[0]);
        audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!once)
        {
            float percentage = ((CountDownTimer.ins.timeLeft / CountDownTimer.ins.maxTime) * 100);
            Debug.Log(CountDownTimer.ins.timeLeft % 7.385 == 0);
            if (percentage <= 40 && CountDownTimer.ins.timeLeft % 7.385 == 0)
            {
                audioSource.Stop();
                AudioClip clip = backgroundSong[backgroundSong.Length - 1];
                audioSource.PlayOneShot(clip);
                audioSource.loop = true;
                once = true;
            }
        }

    }
}
