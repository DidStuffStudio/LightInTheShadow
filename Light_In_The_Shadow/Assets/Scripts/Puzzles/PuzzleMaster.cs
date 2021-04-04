using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleMaster : MonoBehaviour
{
    [Tooltip("Objects that should become visible when fading in scene. For example, the door in puzzle 1")]
    [SerializeField] private GameObject[] hiddenInitiatorObjects;
    [Space]
    [Tooltip("The object to focus on. For example, the phone in puzzle 2. Should have detect click script attached.")]
    [SerializeField] private GameObject puzzleObject;
    [Space]
    [SerializeField] private GameObject puzzleFocusCamera;
    [SerializeField] private GameObject cutsceneCamera;
    [Space]
    [Tooltip("The memory to spawn")]
    [SerializeField] private GameObject memoryPrefab;
    [SerializeField] private Transform memorySpawnLocation;
    [Space]
    [Tooltip("Child character game object with animations. Place in order of which they should activate and deactivate")]
    [SerializeField] private GameObject[] childCharacterGameObjects;
    [Tooltip("Puzzle 1 should be 1, puzzle 2 should be 2 and so on...")]
    [Space]
    [SerializeField] private int puzzleMemorySoundIndex;
    
    
    private Animator _memoryLightAnimator;  
    private protected playerController playerController;
    private protected inventorySystem inventorySystem;
    private protected bool finished, fadeInNow;
    private protected PuzzleAudio puzzleAudio;
   
    
    private protected bool correct;
    private ParticleSystem _particles;
    private Volume _postProcessing;
    private FadeInScene _fadeInScene;
    private Animator[] _boyAnimators;
    private bool _fadingOut, _fadedIn;
    private bool _focused;
    private int _boyAnimatorIndex;
    private DetectClick _detectClick;
    
    
    
    protected virtual void Start()
    {
        puzzleAudio = GetComponent<PuzzleAudio>();
        playerController = MasterManager.Instance.player;
        inventorySystem = MasterManager.Instance.inventory;
        _fadeInScene = GetComponentInChildren<FadeInScene>();
        _particles = GetComponentInChildren<ParticleSystem>();
        _postProcessing = GetComponentInChildren<Volume>();
        _detectClick = puzzleObject.GetComponent<DetectClick>();
        _memoryLightAnimator = memorySpawnLocation.GetComponent<Animator>();
        _boyAnimators = new Animator[childCharacterGameObjects.Length];
        var i = 0;
        foreach (var go in childCharacterGameObjects)
        {
            _boyAnimators[i] = go.GetComponent<Animator>();
            childCharacterGameObjects[i].SetActive(false);
            i++;
        }
        childCharacterGameObjects[0].SetActive(true);
        _boyAnimators[0].Play("Play Boy Animation");
    }

    protected virtual void EndCutScene()
    {
        // deactivate cinemachine camera
        cutsceneCamera.SetActive(false);
        playerController.FreezePlayerForCutScene(false);
        // go back to camera
        StartCoroutine(MasterManager.Instance.WaitToReturnMusic());
    }

    protected virtual void FadeInScene()
    {
        if(_fadedIn) return;
        _fadeInScene.fadeInNow = true;
        StartCoroutine(puzzleAudio.PlayVoiceClip());
        foreach (var go in hiddenInitiatorObjects)
        {
            go.layer = 0;
        }
        
    }

    // once you solved the TV puzzle
    protected virtual void FadeOutCutscene()
    {
        puzzleObject.GetComponent<Collider>().enabled = puzzleObject.GetComponent<Outline>().enabled = false;
        FocusOnPuzzleItem(focus: false);
        finished = true;
        puzzleAudio.active = false;
        _fadeInScene.reverse = true;
        _fadeInScene.speed = 0.0008f;
        var particleSystemVelocityOverLifetime = _particles.velocityOverLifetime;
        particleSystemVelocityOverLifetime.speedModifierMultiplier = -1;
        _fadeInScene.fadeInNow = true;
        playerController.FreezePlayerForCutScene(true);
        cutsceneCamera.SetActive(true);
        MasterManager.Instance.soundtrackMaster.LevelMusicVolume(0,0.0f, 5.0f);
        MasterManager.Instance.soundtrackMaster.PlayMemoryMusic(2, false);
        MasterManager.Instance.soundtrackMaster.PlayMemoryMusic(puzzleMemorySoundIndex, true);
        MasterManager.Instance.soundtrackMaster.MemoryMusicVolume(100.0f, 5.0f);
        _fadingOut = true;
        StartCoroutine(PlayMemoryLight());
    }

    protected virtual void BoyAnimations()
    {
        childCharacterGameObjects[_boyAnimatorIndex].SetActive(false);
        _boyAnimatorIndex++;
        if (_boyAnimatorIndex > _boyAnimators.Length - 1) _boyAnimatorIndex = 0;
        childCharacterGameObjects[_boyAnimatorIndex].SetActive(true);
        _boyAnimators[_boyAnimatorIndex].Play("Play Boy Animation");
    }

    protected virtual void FocusOnPuzzleItem(bool focus)
    {
        MasterManager.Instance.interactor.mouseControl = focus;
        MasterManager.Instance.LockCursor(!focus);
        _focused = focus;
        puzzleObject.GetComponent<Collider>().enabled = !focus;
        puzzleObject.GetComponent<Outline>().enabled = !focus;
        puzzleFocusCamera.SetActive(focus);
    }

    protected virtual void Update()
    {
        if (!fadeInNow && !_fadedIn) return;
        FadeInScene();
        _fadedIn = true;
        if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f && _focused) FocusOnPuzzleItem(false);
        _detectClick.canClick = playerController.currentTagTorchHit == "ClickInteract";
        if (correct && !finished) FadeOutCutscene();
        if (!_fadingOut) return;
        var particleSystemShape = _particles.shape;
        _postProcessing.weight = Map(_fadeInScene.increment, 8, 0, 1, 0);
        particleSystemShape.radius = Map(_fadeInScene.increment, 8, 0, 4.3f, 0);
    }

    protected virtual IEnumerator PlayMemoryLight()
    {
        yield return new WaitForSeconds(15.0f);
        _memoryLightAnimator.Play("MemoryLightAnimation");
        yield return new WaitForSeconds(2.0f);
        SpawnMemory();
    }

    protected virtual void SpawnMemory()
    {
        _particles.gameObject.SetActive(false);
        Instantiate(memoryPrefab, memorySpawnLocation.position, memorySpawnLocation.rotation);
        MasterManager.Instance.soundtrackMaster.PlaySoundEffect(0);
        StartCoroutine(WaitToDestroy());

    }

    protected virtual float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    
   
    
    protected virtual IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
    
    
    
    
    
    
}
