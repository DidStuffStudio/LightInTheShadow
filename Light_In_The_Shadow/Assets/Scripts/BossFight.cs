using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;

public class BossFight : MonoBehaviour
{

    private Camera _cam;
    public float interactionDistance = 100.0f;
    public float torchStrength = 20.0f;
    private int bossLayerMask;
    public float health = 100.0f, healthDecayRate = 10, updateTime = 0.2f;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool _canHurtBigBossMan = true;
    public bool alive = true;
    [SerializeField] private GameObject bigBossMan;
    [SerializeField] private GameObject hitEffect;
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private GameObject darknessVFX, bigBossMouth;
    [SerializeField] private GameObject flyingBean;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int numberOfFlyingBeans, flyingBeansIncreaseRate = 2;
    [SerializeField] private float breathForce = 50;

    [SerializeField] private Material skybox, ice;
    private Color _fogStartColor;
    [SerializeField] private Color fogEndColor;
    [SerializeField] private GameObject bossManExplosion, trees, cutSceneCam;
    [SerializeField] private Volume nicePP;
    [SerializeField] private TerrainChange terrainChange;
    public int numberOfMonsters;
    
    
    void Start()
    {
        _fogStartColor = RenderSettings.fogColor;
        //numberOfMonsters = FindObjectsOfType<DarkThoughtWalking>().Length;
        bossLayerMask = LayerMask.GetMask("BigBossMan");
        skinnedMeshRenderer = bigBossMan.GetComponentInChildren<SkinnedMeshRenderer>();
        _cam = Camera.main;
        _player = MasterManager.Instance.player.gameObject;
        _animator = bigBossMan.GetComponent<Animator>();
        BreathDarkness(); // Change to when we want attack to begin

    }

    // Update is called once per frame

    private void Update()
    {
        
        if (alive)
        {
            if (MasterManager.Instance.player.holdingTorch)
            {
                var torchRay = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                if (Physics.Raycast(torchRay, out var hit, interactionDistance, bossLayerMask) && _canHurtBigBossMan)
                {
                    StartCoroutine(HurtBigBossMan(hit.point));
                }
            }
        }
        if (numberOfMonsters > 0)
        {
            _canHurtBigBossMan = false;
            if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("Pain"))_animator.Play("Pain", 0, 0);
        }
        else if(alive)
        {
            BreathDarkness();
            _canHurtBigBossMan = true;
        }

        var position = _player.transform.position;
        if (alive)
        {
            bigBossMan.transform.LookAt(new Vector3(position.x, bigBossMan.transform.position.y,
                position.z));
        }

        if (health < 0 && alive)
        {
            alive = false;
            MasterManager.Instance.player.playerHealth = 100;
            StartCutscene();
            
        }
    }

    public void RestartLevel()
    {
        health = 100.0f;
        foreach (var flyingBean in FindObjectsOfType<FlyingBeanSpider>())
        {
            MasterManager.Instance.interactor.ReleaseDarkThought(flyingBean.gameObject);
            Destroy(flyingBean);
        }
    }

    IEnumerator HurtBigBossMan(Vector3 hitPoint)
    {
        _canHurtBigBossMan = false;
        var effect = Instantiate(hitEffect, hitPoint, Quaternion.identity);
        effect.transform.localScale = Vector3.one*5.0f;
        health -= healthDecayRate;
        skinnedMeshRenderer.material.SetFloat("_health", health);
        _animator.Play("Pain", 0, 0);
        StartCoroutine(SpawnMonsters());
        yield return new WaitForSeconds(10.0f);
        Destroy(effect);
    }

    public void BreathDarkness()
    {
        if (!alive) return;
        _canHurtBigBossMan = true;
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("BreathDarkness"))_animator.Play("BreathDarkness", 0, 0);
    }

    public IEnumerator SpawnDarkness()
    {
        if (alive)
        {
            var position = bigBossMouth.transform.position;
            var vfx = Instantiate(darknessVFX, position, bigBossMouth.transform.rotation);
            var rb = vfx.GetComponent<Rigidbody>();
            rb.AddForce((_player.transform.position - position) * breathForce, ForceMode.Force);
            yield return new WaitForSeconds(10.0f);
            Destroy(vfx);
        }
    }
    public IEnumerator SpawnMonsters()
    {
        if(alive)
        {
          
        for (int i = 0; i < numberOfFlyingBeans; i++)
        {
            numberOfMonsters += 1;
            Instantiate(flyingBean, spawnPoint.position, quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }

        
        numberOfFlyingBeans += flyingBeansIncreaseRate;
        }
    }

    private void StartCutscene()
    {
        MasterManager.Instance.player.FreezePlayerForCutScene(true);
        cutSceneCam.SetActive(true);
    }
    public void EndCutscene()
    {
        MasterManager.Instance.player.FreezePlayerForCutScene(false);
        cutSceneCam.SetActive(false);
        StartCoroutine(EndCredits());
    }

    public IEnumerator KillBigBossMan() //Set sky box float "Fog Intensity" set ice float "frozen" set fog density of environment to 0.01 and change colour.
    {
        yield return new WaitForSeconds(1.0f);
        //Spawn explosion
        bossManExplosion.SetActive(true);
        Destroy(trees);
        terrainChange.change = true;
        bigBossMan.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        var value = 100.0f;
        var fogIntensity = RenderSettings.fogDensity;
       
        MasterManager.Instance.soundtrackMaster.LevelAmbienceVolume(3,0,10.0f);
        MasterManager.Instance.soundtrackMaster.LevelAmbienceVolume(1,100,10.0f);
        MasterManager.Instance.soundtrackMaster.LevelMusicVolume(0,0,10.0f);
        MasterManager.Instance.soundtrackMaster.MainThemeVolume(100,10.0f);
        
        while (value > 0)
        {
            value -= 0.1f;
            ice.SetFloat("frozen", value);   
            
            RenderSettings.fogDensity = Map(value, 100, 0, fogIntensity, 0.0003f);
            RenderSettings.fogColor = Color.Lerp(_fogStartColor,fogEndColor, 1.0f);
            skybox.SetFloat("Fog Intensity", Map(value, 100,0,1.0f,0.1f));
            MasterManager.Instance.ppVolume.weight = Map(value, 100, 0, 1, 0);
            nicePP.weight = Map(value, 100, 0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
        MasterManager.Instance.soundtrackMaster.PlaySoundEffect(0);
        StartCoroutine(terrainChange.SpawnInTreesAndGrass());
        Destroy(bigBossMan);
        print("Game Ended");
    }

    IEnumerator EndCredits()
    {
        
        yield return new WaitForSeconds(10.0f);
        //Show credits
    }
    
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }


    private void OnEnable()
    {
        ice.SetFloat("frozen", 100.0f);  
    }
}

