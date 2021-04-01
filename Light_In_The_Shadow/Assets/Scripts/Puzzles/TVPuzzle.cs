using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TVPuzzle : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator, _memoryLightAnimator; // open door animator
    [SerializeField] private FadeInScene _fadeInScene; // living room fad in scene
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip[] _audioClips;

    [SerializeField] private GameObject cinemachineDollyCamera;
    [SerializeField] private GameObject cinemachineTVFocusCamera;
    [SerializeField] private GameObject tv;
    [SerializeField] private Antenna antennaA, antennaB;
    private inventorySystem InventorySystem;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Volume _postProcessing;
    private playerController _playerController;
    [SerializeField] private GameObject memoryPrefab;
    [SerializeField] private Transform memorySpawnLocation;
    private bool finished, fadingOut;
    private DetectClick _doorClicker;


    private void Start()
    {
        _doorClicker = _doorAnimator.GetComponentInChildren<DetectClick>();
        _playerController = MasterManager.Instance.player;
        InventorySystem = _playerController.GetComponent<inventorySystem>();

    }

    public void EndTVCutScene()
    {
        print("The recorded animation has finished");

        // deactivate cinemachine camera
        cinemachineDollyCamera.SetActive(false);
        _playerController.FreezePlayerForCutScene(false);
        // go back to camera
        StartCoroutine(WaitToReturnMusic());
    }

    public void CheckIfHasKey()
    {
        bool hasKey = false;
        foreach (var item in InventorySystem.itemsInInventory)
        {
            if (item.name.Contains("Key")) hasKey = true;
        }

        if (hasKey)
        {
            doorAudioSource.PlayOneShot(_audioClips[0]);
            _doorAnimator.Play("Scene");
            _fadeInScene.fadeInNow = true;
            doorAudioSource.PlayOneShot(_audioClips[3]);
            _doorAnimator.gameObject.layer = 0;
            tv.GetComponent<DetectClick>().canClick = true;
            for (int i = 0; i < _doorAnimator.transform.childCount; i++)
            {
                _doorAnimator.transform.GetChild(i).gameObject.layer = 0;
            }

            
        }
        else
        {
            doorAudioSource.PlayOneShot(_audioClips[1]);
        }
    }

    // once you solved the TV puzzle
    public void FadeOutCutscene()
    {
        FocusOnTV(focus: false);
        finished = true;
        // fade out and makes dolly movement camera --> activate dolly camera
        tv.GetComponent<Collider>().enabled = tv.GetComponent<Outline>().enabled = false;
        _fadeInScene.reverse = true;
        _fadeInScene.speed = 0.0008f;
        var particleSystemVelocityOverLifetime = _particleSystem.velocityOverLifetime;
        particleSystemVelocityOverLifetime.speedModifierMultiplier = -1;
        _fadeInScene.fadeInNow = true;
        _playerController.FreezePlayerForCutScene(true);
        cinemachineDollyCamera.SetActive(true);
        MasterManager.Instance.soundtrackMaster.LevelMusicVolume(0,0.0f, 1.0f);
        MasterManager.Instance.soundtrackMaster.PlayMemoryMusic(0, true);
        MasterManager.Instance.soundtrackMaster.MemoryMusicVolume(100.0f, 1.0f);
        fadingOut = true;
        StartCoroutine(PlayMemoryLight());
    }

    public void FocusOnTV(bool focus)
    {
        print("Focused on TV!: " + focus);
        tv.GetComponent<Collider>().enabled = !focus;
        tv.GetComponent<Outline>().enabled = !focus;
        cinemachineTVFocusCamera.SetActive(focus);
    }

    private void Update()
    {    
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f) FocusOnTV(false);
        _doorClicker.canClick = _playerController.currentTagTorchHit == "ClickInteract";
        if (antennaA.antennaCorrect && antennaB.antennaCorrect && !finished) FadeOutCutscene();
        if (!fadingOut) return;
        var particleSystemShape = _particleSystem.shape;
        _postProcessing.weight = Map(_fadeInScene.increment, 8, 0, 1, 0);
        particleSystemShape.radius = Map(_fadeInScene.increment, 8, 0, 4.3f, 0);
    }

    IEnumerator PlayMemoryLight()
    {
        yield return new WaitForSeconds(15.0f);
        _memoryLightAnimator.Play("MemoryLightAnimation");
        yield return new WaitForSeconds(2.0f);
        SpawnMemory();
    }

    void SpawnMemory()
    {
        _particleSystem.gameObject.SetActive(false);
        Instantiate(memoryPrefab, memorySpawnLocation.position, memorySpawnLocation.rotation);
        doorAudioSource.PlayOneShot(_audioClips[4]);
        tv.gameObject.SetActive(false);
    }

    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    
    IEnumerator WaitToReturnMusic()
    {
        yield return new WaitForSeconds(10.0f);
        MasterManager.Instance.soundtrackMaster.LevelMusicVolume(0,100.0f, 5.0f);
        MasterManager.Instance.soundtrackMaster.MemoryMusicVolume(0.0f, 5.0f);
    }
}