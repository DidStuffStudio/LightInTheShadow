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
    public int levelIndex = 1;
    public playerController player;
    public SoundtrackMaster soundtrackMaster;
    public GameObject[] portals = new GameObject[4];

    public static MasterManager Instance {
        get {
            if (_instance != null) return _instance;
            var masterManagerGameObject = new GameObject();
            _instance = masterManagerGameObject.AddComponent<MasterManager>();
            masterManagerGameObject.name = typeof(MasterManager).ToString();
            return _instance;
        }
    }

    public void ChangeLevelAudio()
    {
        soundtrackMaster.LevelMusicVolume(levelIndex, 0.0f, 10.0f);
        soundtrackMaster.PlayLevelMusic(2, true);
        soundtrackMaster.PlayLevelAmbience(2, true);
        soundtrackMaster.LevelMusicVolume(levelIndex + 1, 100.0f, 10.0f);
        soundtrackMaster.LevelAmbienceVolume(levelIndex, 0.0f, 2.0f);
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
        if(levelIndex > 0) ChangeLevelAudio();
        levelIndex++;
        LoadNextScene(levelIndex);
    }

    private void LoadNextScene(int sceneToLoad) {
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        portals[0].SetActive(false);
        portals[1].SetActive(false);
        portals[2].SetActive(true);
        portals[3].SetActive(true);
    }

    public void UnloadPreviousScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));
        if (levelIndex > 0 && levelIndex < 4) SceneManager.UnloadSceneAsync(levelIndex - 1);
        GetComponentInChildren<Volume>().profile = levelPP[levelIndex-1];
    }

    public void ToggleNeurons(bool enable)
    {
        loadingScreen.SetActive(enable);
        portals[0].SetActive(enable);
        portals[1].SetActive(enable);
        if (enable) return;
        portals[2].SetActive(false);
        portals[3].SetActive(false);
        player.GetComponent<playerController>().NewRespawnPoint();
    }
    public void Quality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}