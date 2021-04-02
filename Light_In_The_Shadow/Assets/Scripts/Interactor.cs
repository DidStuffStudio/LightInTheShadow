using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    private Camera _cam;
    public float interactionDistance = 20;
    private GameObject _lastHitObject;
    public bool inventoryItemHit;
    public GameObject inventoryItem;
    private int _layerMask;
    public string currentTag = "Nothing";
    public bool mouseControl;
    private Ray _ray;
    private void Start()
    {
        _cam = Camera.main;
        _layerMask = LayerMask.GetMask("InventoryItem", "Clickable", "Hidden");
    }

    private void Update()
    {
        _ray = !mouseControl ? _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0)) : _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(_ray, out hit, interactionDistance, _layerMask))
        {
            if (hit.transform.gameObject.layer == 12)
            {
                hit.transform.gameObject.GetComponent<item>().canvas.SetActive(true);
                hit.transform.gameObject.GetComponent<Outline>().enabled = true;
                inventoryItem = hit.transform.gameObject;
                inventoryItemHit = true;
            }
            if (!hit.transform.gameObject.CompareTag("ClickInteract")) return;
            currentTag = hit.transform.gameObject.tag;
            var detectClick = hit.transform.gameObject.GetComponent<DetectClick>();
            if (detectClick == null || !detectClick.canClick) return;
            _lastHitObject = hit.transform.gameObject;
            hit.transform.GetComponent<Outline>().enabled = true;
            if (!Input.GetMouseButtonDown(0)) return;
            detectClick.Click();

        }
        else
        {
            
            currentTag = "Nothing";
            if (_lastHitObject != null && _lastHitObject.transform.gameObject.GetComponent<Outline>())
                _lastHitObject.transform.GetComponent<Outline>().enabled = false;
            

            if (!inventoryItemHit) return;
            inventoryItemHit = false;
            if (inventoryItem)
            {
                inventoryItem.GetComponent<item>().canvas.SetActive(false);
                inventoryItem.GetComponent<Outline>().enabled = false;
            }

            inventoryItem = null;

        }

    }
}
