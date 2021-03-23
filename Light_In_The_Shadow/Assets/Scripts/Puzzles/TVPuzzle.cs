using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVPuzzle : MonoBehaviour {

    [SerializeField] private Animator _animator; // open door animator
    [SerializeField] private FadeInScene _fadeInScene; // living room fad in scene
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip [] _audioClips;

    [SerializeField] private GameObject cinemachineDollyCamera;
    [SerializeField] private GameObject cinemachineTVFocusCamera;
    [SerializeField] private GameObject tv;
    private Camera mainCamera;
    public inventorySystem InventorySystem;


    public void EndTVCutScene() {
        print("The recorded animation has finished");
        
        // deactivate cinemachine camera
        cinemachineDollyCamera.SetActive(false);
        
        // go back to camera
    }

    public void CheckIfHasKey() {
        print("Checking if there's a key");
        foreach (var item in InventorySystem.itemsInInventory) {
            if (item.name == "Key") {
                print("You can use the key!");
                // doorAudioSource.clip = _audioClips[0];
                doorAudioSource.PlayOneShot(_audioClips[0]);
                _animator.Play("Scene");
                _fadeInScene.gameObject.SetActive(true);
                _fadeInScene.enabled = true;
            }
            else {
                print("You don't have the key!");
                // doorAudioSource.clip = _audioClips[1];
                doorAudioSource.PlayOneShot(_audioClips[1]);
            }
        }
    }
    
    // once you solved the TV puzzle
    public void FadeOutCutscene() {
        FocusOnTV(focus: false);
        // fade out and makes dolly movement camera --> activate dolly camera
        cinemachineDollyCamera.SetActive(true);
        doorAudioSource.PlayOneShot(_audioClips[2]);
        // TODO: shrink memory and pick it up - Instantiate memory prefab
    }

    public void FocusOnTV(bool focus) {
        print("Focused on TV!: " + focus);
        tv.GetComponent<Collider>().enabled = !focus;
        tv.GetComponent<Outline>().enabled = !focus;
        cinemachineTVFocusCamera.SetActive(focus);
    }
}