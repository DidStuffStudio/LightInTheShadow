using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MasterManager : MonoBehaviour {
    // instance of the Master Manager
    private static MasterManager _instance;

    public VolumeProfile[] levelPP;

    public Portal loadingPortal;
    public Camera loadingPortalCam;
    public Transform recievingCollider;

    public static MasterManager Instance {
        get {
            if (_instance != null) return _instance;
            var masterManagerGameObject = new GameObject();
            _instance = masterManagerGameObject.AddComponent<MasterManager>();
            masterManagerGameObject.name = typeof(MasterManager).ToString();
            return _instance;
        }
    }

    //private UserInput _userInput;
    private int _levelIndex = 0;
    public bool loadingScreenTransitionStarted = false;

    public GameObject loadingScreen;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            // make instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }

        // set the user input control
        //var component = GetComponent<UserInput>();
        
        //if (component == null) _userInput = gameObject.AddComponent<UserInput>();
        //else _userInput = GetComponent<UserInput>();
    }

    public void StartLoadingNextScene() {
        // increase the level index to be the one after the active scene
        _levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        StartCoroutine(LoadNextScene(_levelIndex));
    }

    private IEnumerator LoadNextScene(int sceneToLoad) {
        // loadingScreen.SetActive(true);
        loadingScreenTransitionStarted = true;

        yield return new WaitForSeconds(0.1f);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        // float loadProgress = loadingOperation.progress;
        while (!loadingOperation.isDone) {
            yield return null;
        }
    }

    public void LoadingScreenTransitionFinished() {
        if(_levelIndex > 0) SceneManager.UnloadSceneAsync(_levelIndex - 1);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(_levelIndex));
        print("The loading screen transition has finished");
        loadingScreen.SetActive(false);
        loadingScreenTransitionStarted = false;
        GetComponentInChildren<Volume>().profile = levelPP[_levelIndex];
        if (_levelIndex < levelPP.Length) NewPortalSetup();
    }

    private void NewPortalSetup() {
        print("Setting up new portal");
        var portalGameObject = GameObject.FindWithTag("LevelPortal");
        Portal portal = portalGameObject.GetComponent<Portal>();
        PortalTeleporter teleporter = portalGameObject.GetComponent<PortalTeleporter>();
        portal.linkedPortal = loadingPortal;
        portal.portalCam = loadingPortalCam;
        teleporter.player = GameObject.FindWithTag("Player").transform;
        teleporter.receiver = recievingCollider;
    }


    private IEnumerator FallingDownThroughNeuronsTransition() {
        yield return new WaitForSeconds(5.0f);
    }
}