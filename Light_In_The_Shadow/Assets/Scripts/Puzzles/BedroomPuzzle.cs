using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Puzzles;
using UnityEngine;

public class BedroomPuzzle : PuzzleMaster
{
    [SerializeField] private DetectClick doorDetectClick;
    [SerializeField] private AudioClip doorLockClip;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {

        base.Update();
    }

    public void CheckIfHasKey()
    {
        var hasKey = false;
        foreach (var item in inventorySystem.idsInInventory.Where(id => id.Contains("BedroomDoorKey"))) hasKey = true;
        if (hasKey)
        {
            
            GetComponent<AudioSource>().PlayOneShot(doorLockClip); // Play door open sfx
            inventorySystem.RemoveItem("BedroomDoorKey");
            correct = true;
            doorDetectClick.clickEnabled = false;
        }
        else
        {
            MasterManager.Instance.player.helpText.text = "Maybe I should lock this door to keep the child safe";
            MasterManager.Instance.player.OpenHelpMenu(true);
        }
    }
    
       public void FaceScene(bool fadeIn) {
           if (fadeIn)
           {
               doorDetectClick.clickEnabled = true;
               base.FadeInScene();
           }
            else base.FadeOutCutscene();
        }
    public void EndBedroomCutscene() => base.EndCutScene();
    public void SwitchChildAnimation() => base.BoyAnimations();
}
