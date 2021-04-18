using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Puzzles
{
    public class PuzzleMaster : MonoBehaviour
    {
        [Tooltip("Objects that should become visible when fading in scene. For example, the door in puzzle 1")]
        [SerializeField] private GameObject[] hiddenInitiatorObjects;
        [Space]
        [Tooltip("The object to focus on. For example, the phone in puzzle 2. Should have detect click script attached.")]
        [SerializeField] protected GameObject puzzleObject;
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

        [Tooltip("Puzzle 1 should be 1, puzzle 2 should be 2 and so on...")] [Space] [SerializeField]
        private int puzzleMemorySoundIndex;
        [SerializeField] private int[] otherMemorySoundIndices =  new int[3];
    
    
        private Animator _memoryLightAnimator;  
        private protected PlayerController playerController;
        private protected InventorySystem inventorySystem;
        private protected bool finished, fadeInNow;
        private protected PuzzleAudio puzzleAudio;
        private protected float fadeSpeedIncrement = 0.0008f;
        private protected DetectClick detectClick;
        private protected bool correct;
        
        private ParticleSystem _particles;
        private Volume _postProcessing;
        private FadeInScene _fadeInScene;
        private Animator[] _boyAnimators;
        private bool _fadingOut;
        private bool _focused;
        private bool _fadedIn;
        private int _boyAnimatorIndex;



        protected virtual void Start()
        {
            puzzleAudio = GetComponent<PuzzleAudio>();
            playerController = MasterManager.Instance.player;
            inventorySystem = MasterManager.Instance.inventory;
            _fadeInScene = GetComponentInChildren<FadeInScene>();
            _particles = GetComponentInChildren<ParticleSystem>();
            _postProcessing = GetComponentInChildren<Volume>();
            detectClick = puzzleObject.GetComponent<DetectClick>();
            _memoryLightAnimator = memorySpawnLocation.GetComponent<Animator>();
            _boyAnimators = new Animator[childCharacterGameObjects.Length];
            var i = 0;
            foreach (var go in childCharacterGameObjects)
            {
                _boyAnimators[i] = go.GetComponent<Animator>();
                if(i > 0)
                    foreach (var meshRender in childCharacterGameObjects[i].GetComponentsInChildren<SkinnedMeshRenderer>())
                        meshRender.enabled = false;
                i++;
            }
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
            _fadedIn = true;
            detectClick.clickEnabled = true;
            _fadeInScene.fadeInNow = true;
            //puzzleAudio.active = true;
            //StartCoroutine(puzzleAudio.PlayVoiceClip());
            MasterManager.Instance.soundtrackMaster.PlaySoundEffect(1);
            foreach (var go in hiddenInitiatorObjects)
            {
                go.layer = 0;
            }
        }

        // once you solved the TV puzzle
        protected virtual void FadeOutCutscene()
        {
            Destroy(puzzleAudio);
            puzzleObject.GetComponent<Collider>().enabled = puzzleObject.GetComponent<Outline>().enabled = false;
            puzzleObject.layer = 0;
            FocusOnPuzzleItem(focus: false);
            finished = true;
            //puzzleAudio.active = false;
            _fadeInScene.reverse = true;
            _fadeInScene.speed = fadeSpeedIncrement;
            /*var particleSystemVelocityOverLifetime = _particles.velocityOverLifetime;
            particleSystemVelocityOverLifetime.speedModifierMultiplier = -1;*/
            _fadeInScene.fadeInNow = true;
            playerController.FreezePlayerForCutScene(true);
            cutsceneCamera.SetActive(true);
            MasterManager.Instance.soundtrackMaster.LevelMusicVolume(0,0.0f, 5.0f);
            for (int i = 0; i < otherMemorySoundIndices.Length; i++)
            {
                MasterManager.Instance.soundtrackMaster.PlayMemoryMusic(i, false);
            }
            MasterManager.Instance.soundtrackMaster.PlayMemoryMusic(puzzleMemorySoundIndex, true);
            MasterManager.Instance.soundtrackMaster.MemoryMusicVolume(100.0f, 5.0f);
            _fadingOut = true;
            StartCoroutine(PlayMemoryLight());
        }

        protected virtual void BoyAnimations()
        {
            foreach (var meshRender in childCharacterGameObjects[_boyAnimatorIndex].GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                meshRender.enabled = false;
            }
            _boyAnimatorIndex++;
            if (_boyAnimatorIndex > _boyAnimators.Length - 1) _boyAnimatorIndex = 0;
            foreach (var meshRender in childCharacterGameObjects[_boyAnimatorIndex].GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                meshRender.enabled = true;
            }
            _boyAnimators[_boyAnimatorIndex].Play("Play Boy Animation", 0, 0);
        }

        protected virtual void FocusOnPuzzleItem(bool focus)
        {
            MasterManager.Instance.interactor.mouseControl = focus;
            MasterManager.Instance.LockCursor(!focus);
            _focused = focus;
            puzzleFocusCamera.SetActive(focus);
            puzzleObject.GetComponent<Collider>().enabled = !focus;
            MasterManager.Instance.isInFocusState = focus;
            if (!focus) return;
            MasterManager.Instance.interactor.DisableLastHitObject(puzzleObject);
        }

        protected virtual void Update()
        {
            if (!_fadedIn) return;
            if(MasterManager.Instance.player.playerHealth <= 0) FocusOnPuzzleItem(false);
            if((Mathf.Abs(Input.GetAxis("Horizontal")) > 0.0f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.0f) && _focused) FocusOnPuzzleItem(false);
            if (correct && !finished) {
                FadeOutCutscene();
            }
            if (!_fadingOut) return;
            var particleSystemShape = _particles.shape;
            _postProcessing.weight = Map(_fadeInScene.increment, _fadeInScene.maxDistance, _fadeInScene.minDistance, 1, 0);
            particleSystemShape.randomPositionAmount = Map(_fadeInScene.increment, _fadeInScene.maxDistance, _fadeInScene.minDistance, 5.0f, 0);
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
            StartCoroutine(MasterManager.Instance.WaitToReturnMusic());
            //StartCoroutine(WaitToDestroy());

        }

        protected virtual float Map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    
   
    
        protected virtual IEnumerator WaitToDestroy()
        {
            yield return new WaitForSeconds(2.0f);
            gameObject.SetActive(false);
        }
    
    
    
    
    
    
    }
}
