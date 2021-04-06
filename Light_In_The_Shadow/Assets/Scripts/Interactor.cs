using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private Camera _cam;
    public float interactionDistance = 20;
    private GameObject _lastHitObject, _hitTorchObject, _lastHitDarkThought;
    public bool inventoryItemHit;
    public GameObject inventoryItem = null;
    private int _layerMask, _torchInteractLayerMask, _darkThoughtsLayerMask;
    public bool mouseControl;
    private Ray _ray;
    private bool _isInventoryItemNull, _isTorchHitNull, _hitObjectNull, _hitDarkThoughNull;

    private void Start()
    {
        _isInventoryItemNull = inventoryItem == null;
        _isTorchHitNull = _hitTorchObject == null;
        _hitObjectNull = _lastHitObject == null;
        _hitDarkThoughNull = _lastHitDarkThought == null;
        
        _cam = Camera.main;
        _layerMask = LayerMask.GetMask("InventoryItem", "Clickable");
        _torchInteractLayerMask = LayerMask.GetMask("Hidden");
        _darkThoughtsLayerMask = LayerMask.GetMask( "DarkThoughts");
        
    }

    private void Update()
    {
        _ray = !mouseControl ? _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)) : _cam.ScreenPointToRay(Input.mousePosition);
        
        //Normal raycast
        if (Physics.Raycast(_ray, out var hit, interactionDistance, _layerMask))
        {
            if (hit.transform.gameObject.layer == 12) //If we hit an inventory item
            {
                var invItem = hit.transform.gameObject;
                
                if (_isInventoryItemNull)
                {
                    EnableInventoryItemInteraction(invItem, true);
                    _isInventoryItemNull = false;
                }

                else if (invItem != inventoryItem)
                {
                    EnableInventoryItemInteraction(inventoryItem, false);
                    EnableInventoryItemInteraction(invItem, true);
                    _isInventoryItemNull = false;
                }
            }
            
            else if(!_isInventoryItemNull) EnableInventoryItemInteraction(inventoryItem, false);
            
            if (hit.transform.gameObject.CompareTag("ClickInteract")){ //If we hit something we can click on

                var hitGameObject = hit.transform.gameObject;
                
                if (_hitObjectNull)EnableInteraction(hitGameObject, true);
                
                else if (hitGameObject != _lastHitObject)
                {
                    EnableInteraction(_lastHitObject, false);
                    EnableInteraction(hitGameObject, true);
                }
                
            }
            else if(!_hitObjectNull) DisableLastHitObject(_lastHitObject);
        }
        
        else
        {
            if (!_hitObjectNull) DisableLastHitObject(_lastHitObject);
                
            if (inventoryItemHit)
            {
                inventoryItemHit = false;
                if (inventoryItem) EnableInventoryItemInteraction(inventoryItem, false);
                
                inventoryItem = null;
                _isInventoryItemNull = true;
            }
        }
        
        //Raycast for torch
        if (MasterManager.Instance.player.holdingTorch)
        {

            var torchRay = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            Debug.DrawRay(_cam.transform.position, _cam.transform.forward, Color.red);
            if (Physics.Raycast(torchRay, out var torchHit, interactionDistance, _torchInteractLayerMask))
            {
                //If it is a Click Interact Object set its canClick state to true


                if (torchHit.transform.CompareTag("ClickInteract"))
                {
                    var torchHitObject = torchHit.transform.gameObject;
                    if (_isTorchHitNull) EnableTorchInteraction(torchHitObject, true);
                    
                    else if (torchHitObject != _hitTorchObject)
                    {
                        EnableTorchInteraction(_hitTorchObject, false);
                        EnableTorchInteraction(torchHitObject, true);
                    }
                }
                
                else if (!_isTorchHitNull) DisableTorchInteraction();
            }
            else if (!_isTorchHitNull) DisableTorchInteraction();

            if (Physics.Raycast(torchRay, out var darkThoughtHit, interactionDistance, _darkThoughtsLayerMask)) // If we hit a monster
            {
                var monster = darkThoughtHit.transform.gameObject;
                
                if(_hitDarkThoughNull) HitDarkThought(monster, true);
                
                else if(monster != _lastHitDarkThought){
                    HitDarkThought(_lastHitDarkThought, false);
                    HitDarkThought(monster, true); //If it hasn't been hit yet, hit it
                }
            }
            else if (!_hitDarkThoughNull) ReleaseDarkThought(_lastHitDarkThought); //If we didn't hit a monster and one is still being damaged, inform it the torch isn't hitting it
            
        }
        else
        {
            if (!_isTorchHitNull) DisableTorchInteraction();
            if (!_hitDarkThoughNull) ReleaseDarkThought(_lastHitDarkThought); //If anything is still being hit and we're not holding the torch, inform them
        }
    }

  

    private void EnableTorchInteraction(GameObject hitGameObject, bool enable)
    {
        var detectClick = hitGameObject.GetComponent<DetectClick>();
        if (!detectClick.clickEnabled) return;
        detectClick.Outline(enable);
        if(!enable) return;
        _hitTorchObject = hitGameObject;
        _isTorchHitNull = false;
    }

    public void DisableTorchInteraction()
    {
        _hitTorchObject.GetComponent<DetectClick>().Outline(false);
        _hitTorchObject = null;
        _isTorchHitNull = true; //If anything is still being hit and we're not holding the torch, inform them
    }

    private void EnableInteraction(GameObject hitGameObject, bool enable)
    {
      
        var detectClick = hitGameObject.GetComponent<DetectClick>();
        if (!detectClick.clickEnabled) return;
        detectClick.Outline(enable);
        if(!enable) return;
        _lastHitObject = hitGameObject;
        _hitObjectNull = false;
        
    }
    
    public void DisableLastHitObject(GameObject lastHitGameObject)
    {
        lastHitGameObject.GetComponent<DetectClick>().Outline(false);
        _lastHitObject = null;
        _hitObjectNull = true; //If anything is still being hit and we're not holding the torch, inform them
    }

    void EnableInventoryItemInteraction(GameObject item, bool enable)
    {
        item.GetComponent<item>().EnableInteraction(enable);
        inventoryItemHit = enable;
        if(enable) inventoryItem = item;
    }

    public void ReleaseInventoryItem(GameObject item)
    {
        item.GetComponent<item>().EnableInteraction(false);
        inventoryItemHit = false;
        inventoryItem = null;
        _isInventoryItemNull = true;
    }
    
    void HitDarkThought(GameObject darkThought, bool hit)
    {
        var dark = darkThought.transform.GetComponent<DarkThought>();
        dark.hitByTorch = hit;
        if (!hit) return;
        StartCoroutine(dark.DestroyByLight());
        _lastHitDarkThought = darkThought;
        _hitDarkThoughNull = false;
    }

    public void ReleaseDarkThought(GameObject darkThought)
    {
        var dark = darkThought.transform.GetComponent<DarkThought>();
        dark.hitByTorch = false;
        _lastHitDarkThought = null;
        _hitDarkThoughNull = true;
    }
}
