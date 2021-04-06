using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string itemName, description, id;

    
    private GameObject _canvas;
    private Outline _outline;
    private GameObject _player;
    public bool inInventory, shouldPlaySpawnEffect;
    [SerializeField] private GameObject spawnEffect;
    public float spawnWaitTime = 1.0f;
    
    
    void Start()
    {
        if(inInventory) return;
        if (shouldPlaySpawnEffect)
        {
            foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
            {
                meshRenderer.enabled = false;
            }
            foreach (var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                meshRenderer.enabled = false;
            }

            StartCoroutine(WaitForEffect());
            spawnEffect.SetActive(true);
        }
        _player = Camera.main.gameObject;
        _canvas = GetComponentInChildren<Canvas>().gameObject;
        _canvas.SetActive(false);
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        gameObject.name = itemName;
    }

    private void Update()
    {
        if (inInventory) return;
        _canvas.transform.LookAt(_player.transform);
    }
    
    public void EnableInteraction(bool enable)
    {
        _canvas.SetActive(enable);
        _outline.enabled = enable;
    }

    IEnumerator WaitForEffect()
    {
        yield return new WaitForSeconds(spawnWaitTime);
        
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
        
        foreach (var meshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            meshRenderer.enabled = true;
        }
    }
}
