using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyOutline : MonoBehaviour
{
    private Outline _outline;

    [SerializeField] private Outline outlineToCopy;
   
    void Start()
    {
        _outline = GetComponent<Outline>();
    }

    
    void Update()
    {
        _outline.enabled = outlineToCopy.enabled;
    }
}
