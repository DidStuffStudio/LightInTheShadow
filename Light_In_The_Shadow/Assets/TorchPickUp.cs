using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TorchPickUp : MonoBehaviour
{
   
    [HideInInspector]
    public GameObject canvas;
    private GameObject _playerCam;
    private bool _inRange;
    private PlayerControls _playerControls;
    private PlayerController _playerController;

    private Action func; 

    void Start()
    {

        _playerController = FindObjectOfType<PlayerController>();
        _playerControls = _playerController.playerControls;
        _playerCam = Camera.main.gameObject;
        canvas = GetComponentInChildren<Canvas>().gameObject;
        canvas.SetActive(false);
        //_playerControls.Player.PickUp.started += _ => PickupTorch();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 10) return;
        canvas.SetActive(true);
        _inRange = true;

    }

    private void OnTriggerExit(Collider other)
    {
        canvas.SetActive(false);
        _inRange = false;
    }

    void PickupTorch()
    {
        if(_inRange) _playerController.hasTorch = true;
        _playerController.helpText.text = "You picked up a torch! Equip it by holding the right mouse button.";
        _playerController.OpenHelpMenu(true);
        
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!_inRange) return;
        canvas.transform.LookAt(_playerCam.transform);
        if(Input.GetKeyDown(KeyCode.E)) PickupTorch();
    }

    private void OnEnable()
    {
        _playerControls = new PlayerControls();
        _playerControls.Player.PickUp.started += _ => PickupTorch();
    }

    private void OnDisable()
    {
        _playerControls.Player.PickUp.started -= _ => PickupTorch();
    }
}
