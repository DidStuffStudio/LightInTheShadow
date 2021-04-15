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
        BathroomPuzzleEndCutScene,
        BedroomPuzzleEndCutscene,
        TVChildAnimations,
        KitchenChildAnimations,
        BathroomChildAnimations,
        BedroomChildAnimations,
        FinalLevelChildAnimations,
        BossManLogic
        
    }

    public AlertWho alertWho;
    [SerializeField] private int finalAnimationIndex = 0;
    
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

                case AlertWho.BathroomPuzzleEndCutScene:
                {
                    FindObjectOfType<BathroomPuzzle>().EndBathroomCutscene();
                    break;
                }
                
                case AlertWho.BathroomChildAnimations:
                {
                    FindObjectOfType<BathroomPuzzle>().SwitchChildAnimation();
                    break;
                }
                
                case AlertWho.BedroomPuzzleEndCutscene:
                {
                    FindObjectOfType<BedroomPuzzle>().EndBedroomCutscene();
                    break;
                }
                
                case AlertWho.BedroomChildAnimations:
                {
                    FindObjectOfType<BedroomPuzzle>().SwitchChildAnimation();
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

                case AlertWho.FinalLevelChildAnimations:
                {
                    switch (finalAnimationIndex)
                    {
                        case 1:
                            FindObjectOfType<BeforeBossLevel3>().PlayChase();
                            break;
                        case 2:
                            FindObjectOfType<BeforeBossLevel3>().PlayStrangle();
                            break;
                    }

                    break;
                }
                
                
                case AlertWho.BossManLogic:
                {
                    FindObjectOfType<BossFight>().EndCutscene();
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
        
        if (message.Equals("DestroyBigBossMan"))
        {
            StartCoroutine(FindObjectOfType<BossFight>().KillBigBossMan());
        }
        
        else if (message.Equals("SpawnMonsters"))
        {
            StartCoroutine(FindObjectOfType<BossFight>().SpawnMonsters());
        }
        
    }
}
