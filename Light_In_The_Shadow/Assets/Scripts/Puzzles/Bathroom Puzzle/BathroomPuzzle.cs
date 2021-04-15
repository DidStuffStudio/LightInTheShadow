using System.Collections;
using System.Collections.Generic;
using Puzzles;
using UnityEditor;
using UnityEngine;

public class BathroomPuzzle : PuzzleMaster {
    [Space] [Header("Unique Parameters")] [SerializeField]

    [HideInInspector] public bool goingBackToOriginalPos, _isRotating;
    [HideInInspector] public Vector3 mouseDownPosition = Vector3.zero;

    private readonly float _goBackRotationSpeed = 0.2f;
    private readonly float _rotationSensitivity = 0.01f;
    private Quaternion _originalRotation;
    private float _rotation;
    
    [SerializeField] private List<GameObject> woodenPlanks = new List<GameObject>();
    public GameObject crowbar;

    [SerializeField] private float deltaNeededToDetachWoodenPlank = 3.0f;
    private float deltaBetweenWoodenPlankAndCrowbar;

    [SerializeField] private float crowBarForce = 0.1f;

    public WoodenPlank selectedWoodenPlank;
    
    public int detachedWoodenPlanks = 0;
    [SerializeField] private int numberWoodenPlanks;
    private bool puzzleSolved;
    private GameObject originalPuzzleObject;

    protected override void Start() {
        base.Start();
        originalPuzzleObject = puzzleObject;
        crowbar = puzzleObject;
        fadeSpeedIncrement = 0.001f;
        _originalRotation = puzzleObject.transform.rotation;
    }

    protected override void Update() {
        base.Update();
        if(puzzleSolved) return;
        if (_isRotating) {
            // _rotation = puzzleObject.transform.localRotation.eulerAngles.y;
            var mousePosition = (mouseDownPosition - Input.mousePosition);
            _rotation = mousePosition.y * _rotationSensitivity * -1;
            // _rotation *= -1;
            puzzleObject.transform.RotateAround(puzzleObject.transform.position, puzzleObject.transform.forward,
                angle: _rotation);
            deltaBetweenWoodenPlankAndCrowbar += _rotation;
            // print(deltaBetweenWoodenPlankAndCrowbar + "   |   " + deltaNeededToDetachWoodenPlank);
            selectedWoodenPlank.AddForce(crowBarForce);
        }
        else if(puzzleObject)
            puzzleObject.transform.rotation =
                Quaternion.Lerp(puzzleObject.transform.rotation, _originalRotation, _goBackRotationSpeed);
        
        if(Mathf.Abs(deltaBetweenWoodenPlankAndCrowbar) >= deltaNeededToDetachWoodenPlank) {
            _isRotating = false;
            _rotation = 0;
            deltaBetweenWoodenPlankAndCrowbar = 0;
            selectedWoodenPlank.Detach();
        }
        if (detachedWoodenPlanks >= numberWoodenPlanks) PuzzleSolved();
    }

    private void PuzzleSolved() {
        puzzleSolved = true;
        correct = true;
        puzzleObject = originalPuzzleObject;
    }

    public void UpdateWoodenPlank() {
        // update the wooden plank which is currently selected
        // var go = woodenPlanks.Find(item => item == woodenPlank);
    }

    public void SetCrowbar(GameObject c) => puzzleObject = c;
    

    public void SelectWoodenPlank(WoodenPlank woodenPlank) {
        // TODO: instantiate crowbar in
        // TODO: change the crowbar's position and rotation corresponding to the wooden plank
        selectedWoodenPlank = woodenPlank;

        // check for the rotation values, and modify it according to the crowbar's position

    }

    public void FaceScene(bool fadeIn) {
        if(fadeIn) base.FadeInScene();
        else base.FadeOutCutscene();
    }
    

    public void FocusOnWoodenPlanks(bool focus) => base.FocusOnPuzzleItem(focus);
    public void EndBathroomCutscene() => base.EndCutScene();
    public void SwitchChildAnimation() => base.BoyAnimations();
}