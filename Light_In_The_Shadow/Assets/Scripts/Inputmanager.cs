using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    private PlayerControls _playerControls;

    private static Inputmanager _instance;
    
    //TODO Add all inputs here so we can reference them properly from other scripts as well as functions to detect what the input is so we can change UI hints
    
    
    public static Inputmanager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
       if (_instance != null && _instance != this)
       {
           Destroy(this.gameObject);
       }
       else
       {
           _instance = this;
       }
       _playerControls = new PlayerControls();

    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _playerControls.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }
}
