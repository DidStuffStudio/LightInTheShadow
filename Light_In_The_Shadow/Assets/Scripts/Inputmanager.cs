using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    private PlayerControls playerControls;

    private static Inputmanager _instance;
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
        playerControls = new PlayerControls();

    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 getPlayerMovement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }
    public Vector2 getMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>();
    }
}
