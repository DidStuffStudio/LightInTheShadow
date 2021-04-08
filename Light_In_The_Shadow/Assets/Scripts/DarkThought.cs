using System;
using System.Collections;
using System.Collections.Generic;
using Puzzles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class DarkThought : MonoBehaviour
{
    public bool hitByTorch = false, alive = true;
    public int health = 100, maxHealth = 100;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip explosionClip;
    private List<Material> _materials = new List<Material>();
    [SerializeField] private int healthDecayRate, healthRegenerateRate; 
    [SerializeField] private GameObject explosion;

    private protected Transform player;
    protected LayerMask _playerLayer;
    
    private protected Vector3 targetNavigationPoint;
    private protected bool navigationPointSet;
    [SerializeField] private protected float navigationPointRange;
    [SerializeField] private protected float sightRange;
    private protected bool playerInSightRange;
    private bool _canKill = true;
    private protected bool collided;
    protected int damageToHealth = 10;
    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        foreach (var mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            _materials.Add(mr.material);
        }
        if(GetComponentInChildren<MeshRenderer>()) _materials.Add(GetComponentInChildren<MeshRenderer>().material);
        
        player = MasterManager.Instance.player.transform;
        _playerLayer = LayerMask.GetMask("Player");
    }


    protected virtual void Update()
    {
        if (!alive || health > maxHealth) return;
        
        if (health > maxHealth) health = maxHealth;
        
        if (health <= 0)
        {
            alive = false;
            StartCoroutine(Explode());
        }
        
        if (hitByTorch)
        {
            health -= healthDecayRate;
            foreach (var m in _materials) m.SetFloat("_health", health);
        }

        else if (health < maxHealth)
        {
            health += healthRegenerateRate;
            foreach (var m in _materials) m.SetFloat("_health", health);
        }
    }

    protected void LateUpdate()
    {
        collided = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    
    void OnCollisionEnter(Collision other) // Damage or kill player
    {
        
        if (other.gameObject.layer == 10 && _canKill)
        {
            collided = true;
            MasterManager.Instance.player.playerHealth -= damageToHealth;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == 10)
        {
            collided = true;
        }
    }

    protected IEnumerator Explode()
    {
        if (MasterManager.Instance.levelIndex == 3)
        {
            var bossFight = FindObjectOfType<BossFight>();
            bossFight.numberOfMonsters -= 1;
        }
        MasterManager.Instance.interactor.ReleaseDarkThought(gameObject);
        _canKill = false;
        _audioSource.PlayOneShot(explosionClip);
        GetComponent<PuzzleAudio>().canPlay = false;
        var explosionFX = Instantiate(explosion, transform);
        explosionFX.transform.localScale = Vector3.one * 2;
        
        yield return new WaitForSeconds(0.5f);
        foreach (var mr in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            mr.enabled = false;
        }
        if(GetComponentInChildren<MeshRenderer>()) GetComponentInChildren<MeshRenderer>().enabled = false;
        if(GetComponentInChildren<Volume>()) GetComponentInChildren<Volume>().enabled = false;
        StartCoroutine(FadeOutSoundOnDeath());
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    IEnumerator FadeOutSoundOnDeath()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
