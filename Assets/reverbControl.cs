using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class reverbControl : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioMixerSnapshot[] snapshots;
    public float[] weights;

    public void blendSnapshot(int triggerID)
    {
        switch(triggerID)
        {
            case 1:
                weights[0] = 0.9f;
                weights[1] = 0.1f;
                weights[2] = 0.0f;
                mixer.TransitionToSnapshots(snapshots, weights, 2.0f);
                break;
            case 2:
                weights[0] = 0.75f;
                weights[1] = 0.25f;
                weights[2] = 0.0f;
                mixer.TransitionToSnapshots(snapshots, weights, 0.5f);
                break;
            case 3:
                weights[0] = 0.0f;
                weights[1] = 0.1f;
                weights[2] = 0.9f;
                mixer.TransitionToSnapshots(snapshots, weights, 2.0f);
                break;
        }
    }
}
