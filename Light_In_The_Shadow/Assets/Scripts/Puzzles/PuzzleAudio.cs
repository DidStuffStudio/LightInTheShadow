using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleAudio : MonoBehaviour
{
    public bool active = false;
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private float minimumWaitTime = 10.0f, maximumWaitTime = 20.0f;
    [SerializeField] private AudioSource _audioSource;
    

    public IEnumerator PlayVoiceClip()
    {
        while (active)
        {
            var t = Random.Range(minimumWaitTime, maximumWaitTime);
            var i = Random.Range(0, voiceClips.Length - 1);
            yield return new WaitForSeconds(t);
            _audioSource.PlayOneShot(voiceClips[i]);
        }
    }
}
