using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowbar : MonoBehaviour {
    [SerializeField] private BathroomPuzzle bathroomPuzzle;
    public bool pullCrowbarUpWardsToDetach;

    private void OnMouseDown() {
        bathroomPuzzle._isRotating = true;
        bathroomPuzzle.mouseDownPosition = Input.mousePosition;
    }

    private void OnMouseUp() {
        bathroomPuzzle._isRotating = false;
        bathroomPuzzle.UpdateWoodenPlank();
        bathroomPuzzle.goingBackToOriginalPos = true;
    }
}