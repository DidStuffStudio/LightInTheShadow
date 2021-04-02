using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class SoundtrackMaster : MonoBehaviour
{
    [Range(0,100)] public float masterVolume = 0.0f;
    
    public AudioMixer audioMixer;
    [Space]
    public AudioMixerGroup mainTheme;
    public AudioClip mainThemeClip;
    [Space]
    public AudioMixerGroup[] levelMusic;
    public AudioClip[] levelMusicClips;
    [Space]
    public AudioMixerGroup[] levelAmbience;
    public AudioClip[] levelAmbienceClips;
    [Space]
    public AudioMixerGroup[] memoryMusic;
    public AudioClip[] memoryMusicClips;
    [Space]
    public AudioMixerGroup[] portalSounds;
    public AudioClip[] portalSoundsClips;
    [Space]
    public List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        var mainThemeSrc = gameObject.AddComponent<AudioSource>();
        mainThemeSrc.playOnAwake = true;
        mainThemeSrc.loop = true;
        mainThemeSrc.clip = mainThemeClip;
        mainThemeSrc.outputAudioMixerGroup = mainTheme;
        audioSources.Add(mainThemeSrc);
        
        var i = 1;
        var mixGrpIndex = 1;
        foreach (var clip in levelMusicClips)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = src.loop = true;
            src.clip = clip;
            audioSources.Add(src);
            audioSources[i].outputAudioMixerGroup = levelMusic[mixGrpIndex];
            i++;
            mixGrpIndex++;
        }

        mixGrpIndex = 1;
        foreach (var clip in levelAmbienceClips)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = src.loop = true;
            src.clip = clip;
            audioSources.Add(src);
            audioSources[i].outputAudioMixerGroup = levelAmbience[mixGrpIndex];
            i++;
            mixGrpIndex++;
        }

        mixGrpIndex = 1;
        foreach (var clip in memoryMusicClips)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = src.loop = false;
            src.clip = clip;
            audioSources.Add(src);
            audioSources[i].outputAudioMixerGroup = memoryMusic[mixGrpIndex] ;
            i++;
            mixGrpIndex++;
        }

        mixGrpIndex = 1;
        foreach (var clip in portalSoundsClips)
        {
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = src.loop = true;
            src.clip = clip;
            audioSources.Add(src);
            audioSources[i].outputAudioMixerGroup = portalSounds[mixGrpIndex];
            i++;
            mixGrpIndex++;
        }

        audioSources[audioSources.Count - 2].loop = false;

        PlayMainTheme(true);
    }

    public void PlayLevelMusic(int index, bool play) // Level music indices: Level 1 - 1, Level 2 - 2, Level 3 - 3
    {
        
        print("Playing audiosource index from music "+index);
        if(play) audioSources[index].Play();
        else audioSources[index].Stop();
    }

    public void LevelMusicVolume(int index, float targetVolume, float overTime)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(FadeAudio("Level_Music_Volume", Map(targetVolume, 0,100,-80,0), overTime));// global music
                break;
            case 1:
                StartCoroutine(FadeAudio("Level_1_Music_Volume", Map(targetVolume, 0,100,-80,0), overTime));// level 1 music
                break;
            case 2:
                StartCoroutine(FadeAudio("Level_2_Music_Volume", Map(targetVolume, 0,100,-80,0), overTime));// level 2 music
                break;
            case 3:
                StartCoroutine(FadeAudio("Level_3_Music_Volume", Map(targetVolume, 0, 100, -80, 0), overTime));// level 3 music
                break;
            default:
                Debug.LogError("Music level index out of bounds. 0 for global music volume, 1 for level 1, 2 for level 2 and 3 for level 3");
                break;
        }
    }


    public void PlayLevelAmbience(int index, bool play) //Level ambience indices: Level 1 = 4, level 2 = 5, Level 3 = 6
    {
        index += levelMusicClips.Length;
        print("Playing audiosource index from ambience "+index);
        if(play) audioSources[index].Play();
        else audioSources[index].Stop();
    }

    public void LevelAmbienceVolume(int index, float targetVolume, float overTime)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(FadeAudio("Level_Ambience_Volume", Map(targetVolume, 0,100,-80,0), overTime));// global ambience
                break;
            case 1:
                StartCoroutine(FadeAudio("Level_1_Ambience_Volume", Map(targetVolume, 0,100,-80,0), overTime));// level 1 ambience
                break;
            case 2:
                StartCoroutine(FadeAudio("Level_2_Ambience_Volume", Map(targetVolume, 0,100,-80,0), overTime));// level 2 ambience
                break;
            case 3:
                StartCoroutine(FadeAudio("Level_3_Ambience_Volume", Map(targetVolume, 0, 100, -80, 0), overTime));// level 3 ambience
                break;
            default:
                Debug.LogError("Ambience level index out of bounds. 0 for global ambience, 1 for level 1, 2 for level 2 and 3 for level 3");
                break;
        }
    }
        
 
    
    public void PlayMemoryMusic(int index, bool play) //Memory music indices: 1 = 7, 2 = 8, 3 = 9, 4 = 10, 5 = 11
    {
        index += levelMusicClips.Length + levelAmbienceClips.Length;
        if(play) audioSources[index].Play();
        else audioSources[index].Stop();
    }
    
    public void MemoryMusicVolume(float targetVolume, float overTime) => StartCoroutine(FadeAudio("Memories_Volume", Map(targetVolume, 0,100,-80,0), overTime));
    
    
    public void PlayPortalSounds(int index, bool play) // Portal indices: Humming - 12, Passthrough - 13, Wind - 14
    {
        index += levelMusicClips.Length + levelAmbienceClips.Length + memoryMusicClips.Length;
        if(play) audioSources[index].Play();
        else audioSources[index].Stop();
    }

    public void PortalSoundsVolume(float targetVolume, float overTime) => StartCoroutine(FadeAudio("Portal_Volume", Map(targetVolume, 0,100,-80,0), overTime));
    
    public void PlayMainTheme(bool play)
    {
        if(play) audioSources[0].Play();
        else audioSources[0].Stop();
    }

    public void MainThemeVolume(float targetVolume, float overTime)
    {
        StartCoroutine(FadeAudio("Main_Theme_Volume", Map(targetVolume, 0,100,-80,0), overTime));
    }
    
    public void SetGlobalVolume(float volume) => audioMixer.SetFloat("Master_Volume", Map(volume, 0, 100, -80, 20));
    
    public void SetFootstepsVolume(float volume) => audioMixer.SetFloat("Footsteps_Volume", Map(volume, 0, 100, -80, 20));
    
    public void SetSfxVolume(float volume) => audioMixer.SetFloat("Sound_Effects_Volume", Map(volume, 0, 100, -80, 20));
    
    private IEnumerator FadeAudio(String volumeString, float targetVolume, float overTime)
    {
        audioMixer.GetFloat(volumeString, out var vol);
        for (var t = 0f; t < overTime; t += Time.deltaTime) {
            audioMixer.SetFloat(volumeString,Mathf.Lerp(vol, targetVolume, t / overTime));
            yield return new WaitForEndOfFrame();
        }
        audioMixer.SetFloat(volumeString, targetVolume);
    }
    
    
    
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    public void Update()
    {
        SetGlobalVolume(masterVolume);
    }
}
