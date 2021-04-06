using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstPersonAudio : MonoBehaviour
{
    public playerController character;
    //public GroundCheck groundCheck;

    [Header("Step")]
    public AudioSource stepAudio;
    [Tooltip("Minimum velocity for the step audio to play")]
    public float velocityThreshold = .01f;


    private void Start()
    {
        StartCoroutine(SlowUpdate());
    }

    IEnumerator SlowUpdate()
    {
        while (true)
        {
            if (character.controller.velocity.magnitude >= velocityThreshold && character.controller.isGrounded)
            {
                stepAudio.volume = Mathf.Clamp01(character.controller.velocity.magnitude);
            }
            else
            {
                stepAudio.volume = 0.0f;
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
    void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0)
            return;

        // Get a random clip. If possible, make sure that it's not the same as the clip that is already on the audiosource.
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        // Play the clip.
        audio.clip = clip;
        audio.Play();
    }
}
