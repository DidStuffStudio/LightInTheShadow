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
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    public bool alive = true;
    
    void Start()
    {
        bossLayerMask = LayerMask.GetMask("BigBossMan");
     
        _cam = Camera.main;
        StartCoroutine(RaycastUpdate());

    }

    // Update is called once per frame
    IEnumerator RaycastUpdate()
    {
        while (alive)
        {
            if (MasterManager.Instance.player.holdingTorch)
            {
                print("Holding torch and stuff");
                var torchRay = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                if (Physics.Raycast(torchRay, out var hit, interactionDistance, bossLayerMask))
                {
                    health -= healthDecayRate;
                    skinnedMeshRenderer.material.SetFloat("_health", health);
                }
            }

            yield return new WaitForSeconds(updateTime);
        }
    }

    private void Update()
    {
        if (!(health < 0)) return;
        print("DEAD MOTHERFUCKA");
        alive = false;
    }
}

