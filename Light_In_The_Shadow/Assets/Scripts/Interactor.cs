using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Camera cam;
    public float interactionDistance = 20;
    private GameObject _lastHitObject;
    public bool inventoryItemHit;
    public GameObject inventoryItem;
    private int layerMask = 13;
    private void Start()
    {
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Torch", "Player", "UI");
    }

    private void Update()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        
        if (!Physics.Raycast(ray, out hit,layerMask)) return;

        if (hit.transform.gameObject.layer == 12 && hit.distance < interactionDistance)
        {
            hit.transform.gameObject.GetComponent<item>().canvas.SetActive(true);
            hit.transform.gameObject.GetComponent<Outline>().enabled = true;
            inventoryItem = hit.transform.gameObject;
            inventoryItemHit = true;
        }
        else if(inventoryItemHit)
        {
            inventoryItemHit = false;
            if (inventoryItem)
            {
                inventoryItem.GetComponent<item>().canvas.SetActive(false);
                inventoryItem.GetComponent<Outline>().enabled = false;
            }
            inventoryItem = null;
        }
        
        if (hit.transform.gameObject.CompareTag("ClickInteract") && hit.distance < interactionDistance)
        {
            _lastHitObject = hit.transform.gameObject;
            hit.transform.GetComponent<Outline>().enabled = true;
            if (!Input.GetMouseButtonDown(0)) return;
            if (hit.transform.gameObject.GetComponent<DetectClick>() == null) return;
            hit.transform.gameObject.GetComponent<DetectClick>().Click();
        }
        else
        {
            if (_lastHitObject == null) return;
            if (_lastHitObject.transform.gameObject.GetComponent<Outline>() == null) return;
            _lastHitObject.transform.GetComponent<Outline>().enabled = false;
        }



    }
}
