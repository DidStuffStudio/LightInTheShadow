using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string itemName, description;

    [HideInInspector]
    public GameObject canvas;
    private GameObject _player;
    
    
    void Start()
    {
        _player = Camera.main.gameObject;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.SetActive(false);
        gameObject.name = itemName;
    }

    private void Update()
    {
        canvas.transform.LookAt(_player.transform, Vector3.up);
    }
}
