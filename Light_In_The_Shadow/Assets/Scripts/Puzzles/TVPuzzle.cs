using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[ExecuteInEditMode]
public class TVPuzzle : PuzzleMaster
{


    [Header("Unique Parameters")] 
    [SerializeField] private Antenna antennaA;
    [SerializeField] private Antenna antennaB;
    [Space]
    [SerializeField] private Animator door;
    
    private DetectClick _doorClickInteract;

    protected override void Start()
    {
        base.Start();
        _doorClickInteract = door.GetComponentInChildren<DetectClick>();
    }

    public void CheckIfHasKey()
    {
        bool hasKey = false;
        foreach (var item in inventorySystem.itemsInInventory)
        {
            if (item.name.Contains("Key")) hasKey = true;
        }

        if (hasKey)
        {
            puzzleAudio.active = true;
            StartCoroutine(puzzleAudio.PlayVoiceClip());
            MasterManager.Instance.soundtrackMaster.PlaySoundEffect(1); // Play memory fade in sfx
            door.Play("Scene");
            MasterManager.Instance.soundtrackMaster.PlaySoundEffect(3); // Play door open sfx
            _doorClickInteract.canClick = true;
            fadeInNow = true;
        }
        else
        {
            MasterManager.Instance.soundtrackMaster.PlaySoundEffect(2); // Play door locked sfx
        }
    }

    public void EndLivngRoomCutscene()
    {
        base.EndCutScene();
    }

    public void SwitchChildAnimation()
    {
        base.BoyAnimations();
    }

    protected override void FadeOutCutscene()
    {
        base.FadeOutCutscene();
        antennaA.GetComponentInChildren<Outline>().enabled = false;
        antennaB.GetComponentInChildren<Outline>().enabled = false;
    }

    protected override void Update()
    {
        if (playerController == null)
        {
            print("It's the player controller..");
            return;
        }
        _doorClickInteract.canClick = playerController.currentTagTorchHit == "ClickInteract";
        if (antennaA.antennaCorrect && antennaB.antennaCorrect && !finished) correct = true;
        base.Update();
    }
}