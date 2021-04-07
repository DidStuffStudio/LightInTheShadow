using System;
using System.Collections;
using System.Collections.Generic;
using Puzzles;
using UnityEngine;

public class AlertAnimationObservers : MonoBehaviour
{
    
    public enum AlertWho
    {
        TVPuzzleEndCutScene,
        PhonePuzzleEndCutScene,
        TVChildAnimations,
        KitchenChildAnimations,
        BossManLogic
        
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
                    FindObjectOfType<TVPuzzle>().EndLivingRoomCutscene();
                    break;
                }
                
                case AlertWho.TVChildAnimations:
                {
                    FindObjectOfType<TVPuzzle>().SwitchChildAnimation();
                    break;
                }
                
                case AlertWho.PhonePuzzleEndCutScene:
                {
                    FindObjectOfType<PhonePuzzle>().EndKitchenCutscene();
                    break;
                }

                case AlertWho.KitchenChildAnimations:
                {
                    FindObjectOfType<PhonePuzzle>().SwitchChildAnimation();
                    break;
                }

                case AlertWho.BossManLogic:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        else BossManLogic(message);

      
    }

    void BossManLogic(string message)
    {
        
        if (message.Equals("BreathDarkness"))
        {
            StartCoroutine(FindObjectOfType<BossFight>().SpawnDarkness());
        }
        
        else if (message.Equals("SpawnMonsters"))
        {
            StartCoroutine(FindObjectOfType<BossFight>().SpawnMonsters());
        }
        
    }
}
