using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour {
    // instance of the Master Manager
    private static MasterManager _instance;

    public static MasterManager Instance {
        get {
            if (_instance != null) return _instance;
            var masterManagerGameObject = new GameObject();
            _instance = masterManagerGameObject.AddComponent<MasterManager>();
            masterManagerGameObject.name = typeof(MasterManager).ToString();

            // make instance persistent across scenes
            DontDestroyOnLoad(masterManagerGameObject);

            return _instance;
        }
    }

    private UserInput _userInput;
    public Level currentLevel;


    private void Awake() {
        // set the user input control
        var component = GetComponent<UserInput>();
        if (component == null) _userInput = gameObject.AddComponent<UserInput>();
        else _userInput = GetComponent<UserInput>();
    }

    public void SetNextLevel(int index) {
        // load a specific scene
    }
}