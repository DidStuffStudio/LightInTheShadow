using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterAudio : MonoBehaviour {
    [SerializeField] private AudioClip[] voiceClips;
    [SerializeField] private AudioSource audioSource;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            print("Monster " + transform.name + " has collided with the player, so play one shot the audio");
            var i = Random.Range(0, voiceClips.Length);
            audioSource.PlayOneShot(voiceClips[i]);
        }
    }
}