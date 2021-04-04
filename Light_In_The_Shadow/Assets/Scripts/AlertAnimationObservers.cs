using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertAnimationObservers : MonoBehaviour
{
    
    public enum AlertWho
    {
        TVPuzzleEndCutScene,
        PhonePuzzleEndCutScene,
        TVChildAnimations,
        KitchenChildAnimations
        
    }

    public AlertWho alertWho;
    
    public void AlertObservers(string message)
    {
        if (message.Equals("AnimationComplete"))
        {
            switch (alertWho)
            {
                case AlertWho.TVPuzzleEndCutScene:
                {
                    FindObjectOfType<TVPuzzle>().EndLivngRoomCutscene();
                    break;
                }
                
                case AlertWho.TVChildAnimations:
                {
                    FindObjectOfType<TVPuzzle>().SwitchChildAnimation();
                    break;
                }
                
                case AlertWho.PhonePuzzleEndCutScene:
                {
                    //FindObjectOfType<PhonePuzzle>().EndCutScene();
                    break;
                }

                case AlertWho.KitchenChildAnimations:
                {
                    //FindObjectOfType<TVPuzzle>().EndCutScene();
                    break;
                }
            }
        }
    }
}
