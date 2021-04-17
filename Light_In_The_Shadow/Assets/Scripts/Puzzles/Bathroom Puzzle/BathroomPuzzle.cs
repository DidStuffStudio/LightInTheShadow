using System.Collections;
using System.Collections.Generic;
using Puzzles;
using UnityEditor;
using UnityEngine;

public class BathroomPuzzle : PuzzleMaster {
    [Space] [Header("Unique Parameters")] [SerializeField] [HideInInspector]
    public bool goingBackToOriginalPos, _isRotating;

    [HideInInspector] public Vector3 mouseDownPosition = Vector3.zero;

    private readonly float _goBackRotationSpeed = 0.2f;
    private readonly float _rotationSensitivity = 0.01f;
    public float _rotation;

    [SerializeField] private List<GameObject> woodenPlanks = new List<GameObject>();
    [SerializeField] private List<DetectClick> detectClicksOnWoodenPlanks = new List<DetectClick>();
    public GameObject crowbar;

    [SerializeField] private float deltaNeededToDetachWoodenPlank = 3.0f;
    private float deltaBetweenWoodenPlankAndCrowbar = 0.0f;

    [SerializeField] private float crowBarForce = 0.1f;

    public WoodenPlank selectedWoodenPlank;

    public int detachedWoodenPlanks = 0;
    [SerializeField] private int numberWoodenPlanks;
    private bool puzzleSolved;
    private Crowbar currentCrowbar;
    public bool crowbarIsNull = true;

    protected override void Start() {
        base.Start();
        fadeSpeedIncrement = 0.001f;
    }

    protected override void Update() {
        base.Update();
        if (puzzleSolved) return;
        if (_isRotating) {
            var mousePosition = (mouseDownPosition - Input.mousePosition);
            _rotation = mousePosition.y * _rotationSensitivity * -1;
            

            if (selectedWoodenPlank.pullCrowbarUpWardsToDetach) _rotation *= -1;
            
            
                if (deltaBetweenWoodenPlankAndCrowbar <= 0)
                {
                    crowbar.transform.RotateAround(crowbar.transform.position, crowbar.transform.forward,
                        angle: _rotation);
                    deltaBetweenWoodenPlankAndCrowbar += _rotation;
                }
                else crowbar.transform.rotation = Quaternion.Lerp(crowbar.transform.rotation, currentCrowbar.originalRot, _goBackRotationSpeed);
                
            
        }

        if (crowbarIsNull) return;
        if(currentCrowbar.atOriginalRot)deltaBetweenWoodenPlankAndCrowbar = 0.0f;
        
        /*if (deltaBetweenWoodenPlankAndCrowbar <= deltaNeededToDetachWoodenPlank &&
            !selectedWoodenPlank.pullCrowbarUpWardsToDetach) {
            CrowBarHasDetachedWoodenPlank();
        }
        
        if (deltaBetweenWoodenPlankAndCrowbar >= deltaNeededToDetachWoodenPlank &&
            selectedWoodenPlank.pullCrowbarUpWardsToDetach) {
            CrowBarHasDetachedWoodenPlank();
        }
        */
        
        

        if (detachedWoodenPlanks >= numberWoodenPlanks) PuzzleSolved();
    }

    private void CrowBarHasDetachedWoodenPlank()
    {
        /*detachedWoodenPlanks++;*/
        _isRotating = false;
        _rotation = 0;
        /*deltaBetweenWoodenPlankAndCrowbar = 0;*/
    }

    private void PuzzleSolved()
    {
        print("Solved");
        puzzleSolved = true;
        correct = true;
    }

    public void SetCrowbar(GameObject c) {
        if (crowbar != null)
        {
            crowbar.SetActive(false);
        }
        crowbar = c;
        crowbarIsNull = false;
    }


    public void SelectWoodenPlank(WoodenPlank woodenPlank) {
        selectedWoodenPlank = woodenPlank;
        
        if (crowbar.TryGetComponent(out Crowbar crowbarScript)) currentCrowbar = crowbarScript;
        
    }

    public void FaceScene(bool fadeIn) {
        if (fadeIn)
        {
            base.FadeInScene();
        }
        else base.FadeOutCutscene();
    }


    public void FocusOnWoodenPlanks(bool focus) {
        base.FocusOnPuzzleItem(focus);
        foreach (var dc in detectClicksOnWoodenPlanks) {
            dc.gameObject.GetComponent<Collider>().enabled = focus;
            dc.gameObject.GetComponent<Outline>().enabled = false;
            dc.clickEnabled = focus;
            if (!focus) crowbarIsNull = true;
        }
    }

    public void EndBathroomCutscene() => base.EndCutScene();
    public void SwitchChildAnimation() => base.BoyAnimations();
}