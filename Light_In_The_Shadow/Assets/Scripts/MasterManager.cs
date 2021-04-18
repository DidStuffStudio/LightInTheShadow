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
    public Volume ppVolume;
    public int levelIndex = 1;
    public PlayerController player;
    public InventorySystem inventory;
    public SoundtrackMaster soundtrackMaster;
    public GameObject[] portals = new GameObject[4];
    public Interactor interactor;
    public bool isInFocusState;
    [SerializeField] private GameObject credits;

    public static MasterManager Instance {
        get {
            if (_instance != null) return _instance;
            var masterManagerGameObject = new GameObject();
            _instance = masterManagerGameObject.AddComponent<MasterManager>();
            masterManagerGameObject.name = typeof(MasterManager).ToString();
            return _instance;
        }
    }

    private void Start()
    {
        soundtrackMaster.MainThemeVolume(100, 2);
    }

    public void ChangeLevelAudio()
    {
        // Level 1 music is index 0, level 2 is 1 and so on
        soundtrackMaster.LevelMusicVolume(levelIndex, 100.0f, 10.0f); // increase volume of next level music
        soundtrackMaster.PlayLevelMusic(levelIndex, true); // play next level music
        soundtrackMaster.PlayLevelAmbience(levelIndex, true); // play next level ambience
        soundtrackMaster.LevelMusicVolume(levelIndex - 1, 0.0f, 10.0f); //reduce volume of previous level music
        soundtrackMaster.LevelAmbienceVolume(levelIndex - 1 , 0.0f, 0.1f);
        soundtrackMaster.LevelAmbienceVolume(levelIndex , 0.0f, 0.1f);// reduce volume of previous level ambience
    }

    public void PlayCredits()
    {
        credits.SetActive(true);
    }
    
    
    public GameObject loadingScreen;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            // make instance persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LockCursor(bool @lock){

        if (@lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        
    }
    
    
    public void StartLoadingNextScene() {
        // increase the level index to be the one after the active scene
        levelIndex++;
        if (levelIndex > 1) ChangeLevelAudio();
        StartCoroutine(LoadNextScene(levelIndex));
    }

    private IEnumerator LoadNextScene(int sceneToLoad) {
        var asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        portals[0].SetActive(false);
        portals[1].SetActive(false);
        portals[2].SetActive(true);
        portals[3].SetActive(true);
        while (!asyncOperation.isDone) yield return null;
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));
        GetComponentInChildren<Volume>().profile = levelPP[levelIndex - 1];
        UnloadPreviousScene();
    }

    public void UnloadPreviousScene() {
        if (levelIndex > 0 && levelIndex < 4) SceneManager.UnloadSceneAsync(levelIndex - 1);
    }

    public void RestartGame()
    {
        _instance = null;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    public void ToggleNeurons(bool enable) {
        loadingScreen.SetActive(enable);
        portals[0].SetActive(enable);
        portals[1].SetActive(enable);
        if (enable) return;
        portals[2].SetActive(false);
        portals[3].SetActive(false);
        player.GetComponent<PlayerController>().NewRespawnPoint();
    }

    public void Quality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
    public IEnumerator WaitToReturnMusic()
    {
        yield return new WaitForSeconds(20.0f);
        soundtrackMaster.LevelMusicVolume(0,100.0f, 20.0f);
        soundtrackMaster.MemoryMusicVolume(0.0f, 20.0f);
    }
}