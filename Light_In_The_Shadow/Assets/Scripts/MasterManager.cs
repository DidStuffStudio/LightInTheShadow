using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    

    public GameObject loadingScreen;

    private void Awake() {
        if (_instance == null) _instance = this;

        _instance = this;
        // set the user input control
        var component = GetComponent<UserInput>();
        if (component == null) _userInput = gameObject.AddComponent<UserInput>();
        else _userInput = GetComponent<UserInput>();
        
        // loadingScreen.SetActive(false);
    }

    IEnumerator LoadNextScene(string sceneToLoad) {
        // loadingScreen.SetActive(true);

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        float loadProgress = loadingOperation.progress;

        while (!loadingOperation.isDone)
        {
            yield return null;
        }
        
        // loadingScreen.SetActive(false);
    }
    
    
}