using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MasterManager : MonoBehaviour {
    // instance of the Master Manager
    private static MasterManager _instance;
    public VolumeProfile[] levelPP; //TODO Make post processing volumes and fog lerp between levels
    private int _levelIndex = 0;
    [SerializeField] private playerController player;

    public static MasterManager Instance {
        get {
            if (_instance != null) return _instance;
            var masterManagerGameObject = new GameObject();
            _instance = masterManagerGameObject.AddComponent<MasterManager>();
            masterManagerGameObject.name = typeof(MasterManager).ToString();
            return _instance;
        }
    }
    
    public GameObject loadingScreen;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            // make instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }

    }

    public void StartLoadingNextScene() {
        // increase the level index to be the one after the active scene
        _levelIndex++;
        LoadNextScene(_levelIndex);
    }

    private void LoadNextScene(int sceneToLoad) {
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
    }

    public void UnloadPreviousScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_levelIndex));
        if (_levelIndex > 0 && _levelIndex < 4) SceneManager.UnloadSceneAsync(_levelIndex - 1);
        GetComponentInChildren<Volume>().profile = levelPP[_levelIndex-1];
    }

    public void ToggleNeurons(bool enable)
    {
        loadingScreen.SetActive(enable);
        if (enable) return;
        player.GetComponent<playerController>().NewRespawnPoint();
    }
    public void Quality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}