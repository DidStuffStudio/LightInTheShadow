using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    public int counterForIce;
    public int thresholdForIce;
    private int counterWas;
    public bool canHit = true;
    [SerializeField] private BeforeBossLevel3 logic;

    [SerializeField] private Transform pickaxeSpawn;
    [SerializeField] private GameObject animatedPickaxe;
    [SerializeField] private float spawnOffset = 0.5f;
    [SerializeField] private GameObject[] iceCracks = new GameObject[4];
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip iceCracking, iceBreak, rumble;
    [SerializeField] private GameObject bigBossMan, health;
    [SerializeField] private float targetAmplitude, targetFrequency;

    private CinemachineBasicMultiChannelPerlin _cameraNoise;

    private void Start()
    {
        _cameraNoise = MasterManager.Instance.player.playerCam
            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (counterForIce != counterWas) StartCoroutine(SpawnPickAxe());

        counterWas = counterForIce;
    }

    public void HitIce()
    {
        var hasPickaxe = false;
        for (var i = 0; i < MasterManager.Instance.inventory.itemsInInventory.Count; i++)
        {
            if (MasterManager.Instance.inventory.idsInInventory[i].Contains("pickaxe"))
            {
                hasPickaxe = true;
            }
        }

        if(hasPickaxe)
        {
            if (canHit)
            {
                counterForIce++;
                StartCoroutine(ChangeIce());
            }
                
        }
        else
        {
                MasterManager.Instance.player.helpText.text = "You are missing something to break the ice.";
                MasterManager.Instance.player.OpenHelpMenu(true);
        }
    }

    private IEnumerator ChangeIce()
    {
        yield return new WaitForSeconds(0.5f);
        if (counterForIce > 0)
        {
            iceCracks[counterForIce - 1].SetActive(true);
            _audioSource.PlayOneShot(iceCracking);
        }
        if (counterForIce > 1)iceCracks[counterForIce - 2].SetActive(false);
        if (counterForIce >= thresholdForIce)
        {
            GetComponent<DetectClick>().clickEnabled = false;
            _audioSource.PlayOneShot(iceBreak);
            yield return new WaitForSeconds(3.0f);
            logic.PlayDadFall();
            _audioSource.PlayOneShot(rumble);
            MasterManager.Instance.soundtrackMaster.PlayLevelMusic(4, true);
            MasterManager.Instance.soundtrackMaster.LevelMusicVolume(3,0,10);
            MasterManager.Instance.soundtrackMaster.LevelMusicVolume(4,100,10);
            MasterManager.Instance.player.waitingForBossMan = true;
                
            StartCoroutine(BuildCameraShake(targetAmplitude, targetFrequency, 0.01f));
            yield return new WaitForSeconds(20.0f);
            bigBossMan.SetActive(true);//Switch to animation of boss man breaking up through the ice and camera shake
            health.SetActive(true);
            MasterManager.Instance.player.waitingForBossMan = false;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(DecreaseCameraShake(1, 1, 0.1f));
            yield return new WaitForSeconds(10.0f);
            
        }
        
    }
    IEnumerator SpawnPickAxe()
    {
        canHit = false;
        pickaxeSpawn.transform.position = new Vector3(MasterManager.Instance.player.transform.position.x + spawnOffset,0,MasterManager.Instance.player.transform.position.z);
        pickaxeSpawn.transform.LookAt(new Vector3(transform.position.x, pickaxeSpawn.position.y, transform.position.z));
        var pick = Instantiate(animatedPickaxe, pickaxeSpawn);
        pick.transform.localPosition = Vector3.zero;
        yield return new WaitForSeconds(1.5f);
        Destroy(pick);
        canHit = true;
    }

    IEnumerator BuildCameraShake(float targetAmp, float targetFreq, float step)
    {
        while (!bigBossMan.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
            _cameraNoise.m_FrequencyGain =
                Mathf.Lerp(_cameraNoise.m_FrequencyGain, targetFreq, step);
            _cameraNoise.m_AmplitudeGain =
                Mathf.Lerp(_cameraNoise.m_AmplitudeGain, targetAmp, step);
        }
    }
    
    IEnumerator DecreaseCameraShake(float targetAmp, float targetFreq, float step)
    {
        while (Math.Abs(_cameraNoise.m_FrequencyGain - targetFreq) > 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            _cameraNoise.m_FrequencyGain =
                Mathf.Lerp(_cameraNoise.m_FrequencyGain, targetFreq, step);
            _cameraNoise.m_AmplitudeGain =
                Mathf.Lerp(_cameraNoise.m_AmplitudeGain, targetAmp, step);
        }
    }
}
