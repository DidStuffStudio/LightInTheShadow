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
            return _instance;
        }
    }

    private UserInput _userInput;
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
        var component = GetComponent<UserInput>();
        if (component == null) _userInput = gameObject.AddComponent<UserInput>();
        else _userInput = GetComponent<UserInput>();

        // loadingScreen.SetActive(false);
    }

    public void StartLoadingNextScene() {
        _levelIndex++;
        StartCoroutine(LoadNextScene(_levelIndex));
    }

    private IEnumerator LoadNextScene(int sceneToLoad) {
        // loadingScreen.SetActive(true);
        loadingScreenTransitionStarted = true;

        yield return new WaitForSeconds(0.5f);

        print("Loading next scene: " + sceneToLoad);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        // float loadProgress = loadingOperation.progress;

        while (!loadingOperation.isDone) {
            yield return null;
        }

        // yield return new WaitForSeconds(5.0f);

        // loadingScreen.SetActive(false);
    }

    public void LoadingScreenTransitionFinished() {
        print("The loading screen transition has finished");
        loadingScreen.SetActive(false);
        loadingScreenTransitionStarted = false;
    }
    

    private IEnumerator FallingDownThroughNeuronsTransition() {
        yield return new WaitForSeconds(5.0f);
    }
}