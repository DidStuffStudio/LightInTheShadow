using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Antenna : MonoBehaviour
{
    public GameObject[] gameObjects = new GameObject[2];
    [SerializeField] private float maxValue, sensitivity, correctWindow = 0.2f;
    private float _mouseDelta, _mouseStart, _lookAtStart;
    public bool antennaCorrect;
    public AudioMixer masterMix;
    [SerializeField] private MeshRenderer meshRenderer;
    private Material _tvMaterial;
    [SerializeField]private bool isSoundAntenna, isVisualAntenna;
    public bool canInteract;
    private float distance;
    private void Start()
    {
        _tvMaterial = meshRenderer.material;
        _lookAtStart = gameObjects[0].transform.position.z;
        transform.LookAt(gameObjects[0].transform);
    }

    private void Update()
    {
        distance = Vector3.Distance(gameObjects[0].transform.position, gameObjects[1].transform.position);
        antennaCorrect = distance < correctWindow;

        if (isVisualAntenna)_tvMaterial.SetFloat("_TVTransition", Mathf.Clamp01(distance));
        if (!isSoundAntenna) return;
        masterMix.SetFloat("whiteNoise", Map(distance, 3.8f, 0, 20, -80));
        masterMix.SetFloat("tv", Map(distance, 0, 3.8f, 20, -80));
    }

    void OnMouseDown()
    {
        if (!canInteract) return;
        _mouseStart = Input.mousePosition.x;
    }

    private void OnMouseDrag()
    {
        if (!canInteract) return;
        if (gameObjects[0].transform.position.z < _lookAtStart - maxValue)
        {
            gameObjects[0].transform.position = new Vector3( gameObjects[0].transform.position.x, gameObjects[0].transform.position.y,_lookAtStart - maxValue+ 0.1f);
            return;
        }
        if (gameObjects[0].transform.position.z > _lookAtStart + maxValue)
        {
            gameObjects[0].transform.position = new Vector3( gameObjects[0].transform.position.x, gameObjects[0].transform.position.y,_lookAtStart + maxValue - 0.01f);
            return;
        }
        transform.LookAt(gameObjects[0].transform);
        gameObjects[0].transform.position += new Vector3(0,0,_mouseDelta * sensitivity);
        _mouseDelta = Input.mousePosition.x - _mouseStart ;
        
    }
    
    
    float Map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s-a1)*(b2-b1)/(a2-a1);
    }

    
}
