using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class PhonePuzzle : MonoBehaviour {

    private Camera _camera;
    
    // reference to the phone game object to rotate around
    [SerializeField] private GameObject rotaryPhone;
    [SerializeField] private float goBackRotationSpeed = 1;
    public bool _isRotating = false;
    public float _rotation;
    [SerializeField] private float rotationSensitivity = 0.1f;
    public Vector3 _mouseDownPosition;
    public bool goingBackToOriginalPos;
    
    [SerializeField] private GameObject kitchenCamera, phoneCam, memoryPrefab, numbersCanvas;
    [SerializeField] private Transform memorySpawnLocation;
    [SerializeField] private FadeInScene sceneFader;
    [SerializeField] private Animator _memoryLightAnimator;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Volume postProcessing;
    [SerializeField] private AudioSource kitchenAudioSource;
    [SerializeField] private AudioClip[] audioClips;
    private playerController player;

    [SerializeField] private DetectClick _lightSwitchClicker;
    private bool fadingOut;
    
    public string currentNumber;
    private string [] possibleNumbersToCall = {"911", "112"};
    public string _dialedNumber = "";
    private Quaternion originalRotation;

    private void Start() {
        _camera = Camera.main;
        originalRotation = rotaryPhone.transform.rotation;
        player = MasterManager.Instance.player;

    }

    private void Update() {
        if (_isRotating) {
            _rotation = rotaryPhone.transform.localRotation.eulerAngles.x;
            var mousePosition = Input.mousePosition - _mouseDownPosition;
            // var rotationValue = (mouseOffset > 0) ? -Input.mousePosition.x - _mouseDownPosition.x : Input.mousePosition.x - _mouseDownPosition.x;
            _rotation = Mathf.Abs(Vector3.Magnitude(mousePosition) * rotationSensitivity);
            // print(_rotation);
            rotaryPhone.transform.RotateAround(rotaryPhone.transform.position, rotaryPhone.transform.forward, angle: _rotation);
            
            
        }
        else {
            rotaryPhone.transform.rotation = Quaternion.Lerp(rotaryPhone.transform.rotation, originalRotation, goBackRotationSpeed);
        }

        _lightSwitchClicker.canClick = player.currentTagTorchHit == "ClickInteract";
        
        if (!fadingOut) return;
        var particleSystemShape = particles.shape;
        postProcessing.weight = Map(sceneFader.increment, 8, 0, 1, 0);
        particleSystemShape.radius = Map(sceneFader.increment, 8, 0, 4.3f, 0);
        
    }
    
    public void UpdateDialNumber() {
        // check if any of the possible numbers to call contains the currently dialed number
        var aux = _dialedNumber + currentNumber;
        foreach (var number in possibleNumbersToCall) {
            if (number == aux) {
                // the user starts calling the possible number
                _dialedNumber = aux;
                CallNumber();
                break;
            }
            // if the already dialed number + this number is part of the possible numbers, add it to the dialed number
            if (number.StartsWith(aux)) {
                _dialedNumber = aux;
            }
        }
    }

    private void CallNumber() {
        
        _dialedNumber = "";
        StartKitchenCutScene();
    }
    
    

    public void FocusOnPhone(bool focus)
    {
        rotaryPhone.GetComponent<Collider>().enabled = !focus;
        rotaryPhone.GetComponent<Outline>().enabled = !focus;
        phoneCam.SetActive(focus);
    }
    
    public void FadeInScene()
    {
        numbersCanvas.SetActive(true);
        sceneFader.fadeInNow = true;
        kitchenAudioSource.PlayOneShot(audioClips[0]);
        rotaryPhone.GetComponent<DetectClick>().canClick = true; //Play memory opening
    }
    
    public void StartKitchenCutScene()
    {
        FocusOnPhone(focus: false);
        player.FreezePlayerForCutScene(true);
        rotaryPhone.GetComponent<Collider>().enabled = rotaryPhone.GetComponent<Outline>().enabled = false;
        sceneFader.speed = 0.002f;
        numbersCanvas.SetActive(false);
        var particleSystemVelocityOverLifetime = particles.velocityOverLifetime;
        particleSystemVelocityOverLifetime.speedModifierMultiplier = -1;
        kitchenAudioSource.PlayOneShot(audioClips[1]);
        kitchenCamera.SetActive(true);
        sceneFader.reverse = sceneFader.fadeInNow = fadingOut =true;
        StartCoroutine(PlayMemoryLight());
    }
    public void EndKitchenCutScene()
    {
        player.FreezePlayerForCutScene(false);
        kitchenCamera.SetActive(false);
    }

    IEnumerator PlayMemoryLight()
    {
        yield return new WaitForSeconds(15.0f);
        _memoryLightAnimator.Play("MemoryLightAnimation");
        yield return new WaitForSeconds(2.0f);
        SpawnMemory();
    }

    void SpawnMemory()
    {
        particles.gameObject.SetActive(false);
        Instantiate(memoryPrefab, memorySpawnLocation.position, memorySpawnLocation.rotation);
        kitchenAudioSource.PlayOneShot(audioClips[2]);
    }

    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}