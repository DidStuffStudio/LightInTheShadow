using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzles
{
    public class PuzzleAudio : MonoBehaviour
    {
        public bool active = false;
        [SerializeField] private AudioClip[] voiceClips;
        [SerializeField] private float minimumWaitTime = 10.0f, maximumWaitTime = 20.0f;
        [SerializeField] private AudioSource audioSource;

        private void Start()
        {
            if (active) StartCoroutine(PlayVoiceClip());
        }

        public IEnumerator PlayVoiceClip()
        {
            while (active)
            {
                var t = Random.Range(minimumWaitTime, maximumWaitTime);
                var i = Random.Range(0, voiceClips.Length - 1);
                yield return new WaitForSeconds(t);
                audioSource.PlayOneShot(voiceClips[i]);
            }
        }
        
        
    }
}
