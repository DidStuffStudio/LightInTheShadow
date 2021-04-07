using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

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
    [SerializeField] private int numberOfFlyingBeans;
    [SerializeField] private float breathForce = 50;

    public int numberOfMonsters;
    
    void Start()
    {
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
        else
        {
            BreathDarkness();
            _canHurtBigBossMan = true;
        }

        var position = _player.transform.position;
        bigBossMan.transform.LookAt(new Vector3(position.x, bigBossMan.transform.position.y,
            position.z));
        if (health < 0 && alive)
        {
            alive = false;
            StartCoroutine(KillBigBossMan());
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
        _canHurtBigBossMan = true;
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsName("BreathDarkness"))_animator.Play("BreathDarkness", 0, 0);
    }

    public IEnumerator SpawnDarkness()
    {
        var position = bigBossMouth.transform.position;
        var vfx = Instantiate(darknessVFX, position, bigBossMouth.transform.rotation);
        var rb = vfx.GetComponent<Rigidbody>();
        rb.AddForce((_player.transform.position - position)*breathForce, ForceMode.Force);
        yield return new WaitForSeconds(10.0f);
        Destroy(vfx);
    }
    public IEnumerator SpawnMonsters()
    {
        for (int i = 0; i < numberOfFlyingBeans; i++)
        {
            numberOfMonsters += 1;
            Instantiate(flyingBean, spawnPoint.position, quaternion.identity);
            yield return new WaitForSeconds(1.0f);
        }

        numberOfFlyingBeans += 2;
    }

    IEnumerator KillBigBossMan()
    {
        yield return new WaitForSeconds(0.1f);
        bigBossMan.SetActive(false);

        print("Ding Dong the witch is dead");
    }


}

