using UnityEngine;

public class FirstPersonAudio : MonoBehaviour
{
    public playerController character;
    //public GroundCheck groundCheck;

    [Header("Step")]
    public AudioSource stepAudio;
    [Tooltip("Minimum velocity for the step audio to play")]
    public float velocityThreshold = .01f;

    

    
    void FixedUpdate()
    {
        if (character.controller.velocity.magnitude >= velocityThreshold && character.controller.isGrounded)
        {
           stepAudio.Play();
        }
        else
        {
            stepAudio.Stop();
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
