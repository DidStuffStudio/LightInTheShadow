using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string itemName, description, id;

    [HideInInspector]
    public GameObject canvas;
    private GameObject _player;
    public bool inInventory;
    
    void Start()
    {
        if(inInventory) return;
        _player = Camera.main.gameObject;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.SetActive(false);
        gameObject.name = itemName;
    }

    private void Update()
    {
        if (inInventory) return;
        canvas.transform.LookAt(_player.transform);
    }
}
