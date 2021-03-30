using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectClick : MonoBehaviour
{
    public UnityEvent OnClick;
    public bool canClick;
    public void Click()
    {
        OnClick?.Invoke();
    }
    
}
