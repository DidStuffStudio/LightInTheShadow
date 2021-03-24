using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassToTVPuzzle : MonoBehaviour
{
    [SerializeField] private TVPuzzle _tvPuzzle;
    [SerializeField] private PhonePuzzle _phonePuzzle;

    public void EndTVCutScene()
    {
        _tvPuzzle.EndTVCutScene();
    }

    public void EndKitchenCutScene()
    {
        _phonePuzzle.EndKitchenCutScene();
    }
    
}
