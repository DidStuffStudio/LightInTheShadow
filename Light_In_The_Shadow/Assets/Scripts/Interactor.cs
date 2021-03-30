using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Camera _cam;
    public float interactionDistance = 20;
    private GameObject _lastHitObject;
    public bool inventoryItemHit;
    public GameObject inventoryItem;
    private int _layerMask;
    public string currentTag = "Nothing";
    private void Start()
    {
        _cam = Camera.main;
        _layerMask = LayerMask.GetMask("InventoryItem", "Clickable", "Hidden");
    }

    private void Update()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, _layerMask))
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
            print(detectClick);
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
