using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFight : MonoBehaviour
{

    private Camera _cam;
    public float interactionDistance = 100.0f;
    public float torchStrength = 20.0f;
    private int bossLayerMask;
    public float health = 100.0f, healthDecayRate = 1, updateTime = 0.2f;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private bool _canHurtBigBossMan = true;
    public bool alive = true;
    [SerializeField] private GameObject bigBossMan;
    [SerializeField] private GameObject hitEffect;
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private GameObject darknessVFX, bigBossMouth;
    
    void Start()
    {
        bossLayerMask = LayerMask.GetMask("BigBossMan");
        skinnedMeshRenderer = bigBossMan.GetComponentInChildren<SkinnedMeshRenderer>();
        _cam = Camera.main;
        _player = MasterManager.Instance.player.gameObject;
        _animator = bigBossMan.GetComponent<Animator>();
        StartCoroutine(RaycastUpdate());

    }

    // Update is called once per frame
    IEnumerator RaycastUpdate()
    {
        while (alive)
        {
            if (MasterManager.Instance.player.holdingTorch)
            {
                var torchRay = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                if (Physics.Raycast(torchRay, out var hit, interactionDistance, bossLayerMask) && _canHurtBigBossMan)
                {
                    
                    _canHurtBigBossMan = false;
                    StartCoroutine(HurtBigBossMan(hit.point));
                }
            }

            yield return new WaitForSeconds(updateTime);
        }
    }

    private void Update()
    {
        
        bigBossMan.transform.LookAt(new Vector3(_player.transform.position.x,bigBossMan.transform.position.y, _player.transform.position.z));
        if (!(health < 0)) return;
        alive = false;
    }

    IEnumerator HurtBigBossMan(Vector3 hitPoint)
    {
        var effect = Instantiate(hitEffect, hitPoint, Quaternion.identity);
        effect.transform.localScale = Vector3.one*5.0f;
        health -= healthDecayRate;
        skinnedMeshRenderer.material.SetFloat("_health", health);
        _animator.Play("Pain");
        yield return new WaitForSeconds(10.0f);
        Destroy(effect);
        SpawnMonsters();
        _canHurtBigBossMan = true;
    }

    public IEnumerator BreathDarkness()
    {
        print("BreahDarkness");
        yield return new WaitForSeconds(1.0f);
        /*var vfx = Instantiate(darknessVFX, bigBossMouth.transform.position, bigBossMouth.transform.rotation);
        _canHurtBigBossMan = false;
        yield return new WaitForSeconds(10.0f);
        _canHurtBigBossMan = true;
        Destroy(vfx);*/
    }
    public void SpawnMonsters()
    {
        print("More Monsters!!!!!!!!!!!!!!!!");
        
    }
}

