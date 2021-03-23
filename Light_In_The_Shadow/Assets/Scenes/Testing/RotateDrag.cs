using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class RotateDrag : MonoBehaviour
{
    
     
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    public GameObject antennaBase;
    public GameObject antenna;
    public GameObject screen;
    public AudioMixer masterMix;

    private float rotationAngle;
    private float quatRotation;
    private Vector3 _previousMouseOffset;
    Renderer rend;

    private bool rightAntennaIsAtCorrectAngle = false;
    private bool leftAntennaIsAtCorrectAngle = false;

    [SerializeField] private TVPuzzle _tvPuzzle;
     
    void Start ()
    {
        _sensitivity = 0.1f;
        _rotation = Vector3.zero;
        rend = screen.GetComponent<Renderer>();
        masterMix.SetFloat("whiteNoise", -10);
        masterMix.SetFloat("tv", -80);
    }
     
    void Update() {
        CheckIfTVPuzzleSolved();
        if(_isRotating)
        {

            rotationAngle=Map(rotationAngle, 0, 360, 0, 20f);
            if (antenna.name == "LeftAntenna")
            {
                quatRotation = antenna.transform.rotation.eulerAngles.x;
                rotationAngle = antenna.transform.rotation.eulerAngles.x;

                _mouseOffset = (Input.mousePosition - _mouseReference);

                _rotation.x = -(_mouseOffset.x) * _sensitivity;

                if (quatRotation > 85 && _mouseOffset.x <= _previousMouseOffset.x)
                {
                    return;
                }

                if (quatRotation < 5 && _mouseOffset.x >= _previousMouseOffset.x) {
                    leftAntennaIsAtCorrectAngle = true;
                    return;
                }
                leftAntennaIsAtCorrectAngle = false;
                _previousMouseOffset = _mouseOffset;
                rotationAngle -= 15;

                //Debug.Log(screen.GetComponent<MeshRenderer>().material.GetFloat("Vector1_c2ed4abf816445a0a71419d361dcdacf"));

                screen.GetComponent<MeshRenderer>().material.SetFloat("Vector1_c2ed4abf816445a0a71419d361dcdacf", rotationAngle);

                transform.RotateAround(antennaBase.transform.position, Vector3.right, _rotation.x);

                // store mouse
                _mouseReference = Input.mousePosition;
            }
            if (antenna.name == "RightAntenna")
            {
                quatRotation = antenna.transform.rotation.eulerAngles.x;
                rotationAngle = antenna.transform.rotation.eulerAngles.x;

                _mouseOffset = (Input.mousePosition - _mouseReference);

                _rotation.x = -(_mouseOffset.x) * _sensitivity;

                if (quatRotation > 340  && quatRotation < 350)
                {
                    masterMix.SetFloat("whiteNoise", -80);
                    masterMix.SetFloat("tv", 0);
                    rightAntennaIsAtCorrectAngle = true;
                }
                else
                {
                    masterMix.SetFloat("whiteNoise",-10);
                    masterMix.SetFloat("tv", -80);
                    rightAntennaIsAtCorrectAngle = false;
                }
                _previousMouseOffset = _mouseOffset;
                rotationAngle -= 15;


                transform.RotateAround(antennaBase.transform.position, Vector3.right, _rotation.x);

                // store mouse
                _mouseReference = Input.mousePosition;
            }




        }


    }

    private void CheckIfTVPuzzleSolved() {
        if(leftAntennaIsAtCorrectAngle && rightAntennaIsAtCorrectAngle) {
            print("TV Puzzle solved, fade out");
            _tvPuzzle.FadeOutCutscene();
        }
    }

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;
         
        // store mouse
        _mouseReference = Input.mousePosition;
    }
     
    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
    
    private float Map(float value, float min1, float max1, float min2, float max2) {

        float r = min2 + (max2 - min2) * ((value - min1) / (max1-min1));

        return r;

    }
     
}
