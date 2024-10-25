using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://andrewmushel.com/articles/sound-effect-variation-in-unity/

public class TeleportSound : MonoBehaviour
{
    public AudioClip[] clipArray;
    public AudioSource effectSource;
    private int clipIndex;
    public float pitchMin, pitchMax, volumeMin, volumeMax;

    public void PlayRoundRobin() {
    if (clipIndex < clipArray.Length) {
        effectSource.PlayOneShot(clipArray[clipIndex]);
        clipIndex++;
    }

    else {
        clipIndex = 0;
        effectSource.PlayOneShot(clipArray[clipIndex]);
        clipIndex++;
    }
}

    // public void PlayRandom() 
    // {
    //     effectSource.pitch = Random.Range(pitchMin, pitchMax);
    //     effectSource.volume = Random.Range(volumeMin, volumeMax);

    //     clipIndex = RepeatCheck(clipIndex, clipArray.Length);
    //     effectSource.PlayOneShot(clipArray[clipIndex]);
    // }

    // private int RepeatCheck(int previousIndex, int range) 
    // {
    //     int index = Random.Range(0, range);
    //     while(index == previousIndex) {
    //         index = Random.Range(0, range);
    //     }
    //     return index;
    // }
}
