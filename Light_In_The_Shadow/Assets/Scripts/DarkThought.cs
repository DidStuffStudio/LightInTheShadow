using System;
using System.Collections;
using System.Collections.Generic;
using Puzzles;
using UnityEngine;
using UnityEngine.Rendering;

public class DarkThought : MonoBehaviour
{
    public bool hitByTorch = false, alive = true, isMonster = true;
    public int health = 100;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip explosionClip;
    private List<SkinnedMeshRenderer> _meshRenderers = new List<SkinnedMeshRenderer>();

    [SerializeField] private GameObject explosion;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        foreach (var mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            _meshRenderers.Add(mr);
        }
    }

    public IEnumerator DestroyByLight()
    {
        while (alive && health <= 100)
        {
            while (hitByTorch && alive)
            {
                yield return new WaitForSeconds(0.1f);
                health -= 5;
                if (health <= 0)
                {
                    alive = false;
                    StartCoroutine(Explode());
                }
            }

            while (health < 100 && alive)
            {
                yield return new WaitForSeconds(0.1f);
                health += 5;
            }
        }

        if (health > 100) health = 99;
    }

    private void Update()
    {
        
        if (hitByTorch || health < 99)
        {
            for (var i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material.SetFloat("_health", health);
            }
        }
    }

    IEnumerator Explode()
    {
        _audioSource.PlayOneShot(explosionClip);
        yield return new WaitForSeconds(4.0f);
        explosion.SetActive(true);
        MasterManager.Instance.interactor.ReleaseDarkThought(gameObject);
        yield return new WaitForSeconds(0.5f);
        for (var i = 0; i < _meshRenderers.Count; i++)
        {
            _meshRenderers[i].enabled = false;
        }
    
        GetComponent<PuzzleAudio>().enabled = false;
        GetComponentInChildren<Volume>().enabled = false;
        if(isMonster) GetComponent<EnemyAI>().canKill = false;
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
