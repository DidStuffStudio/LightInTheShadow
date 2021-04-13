using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Collider _collider;

    [SerializeField] private int healthBoost = 5;
    [SerializeField] private float healthBoostTime = 5, respawnTime = 30.0f;
    
    

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        _meshRenderer.enabled = false;
        _collider.enabled = false;
        StartCoroutine(Respawn());
        StartCoroutine(RegenerateHealth(other.GetComponent<PlayerController>()));
    }

    private IEnumerator RegenerateHealth(PlayerController player)
    {
        player.healthRegenerationRate += healthBoost;   
        yield return new WaitForSeconds(healthBoostTime);
        player.healthRegenerationRate -= healthBoost;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }
}
