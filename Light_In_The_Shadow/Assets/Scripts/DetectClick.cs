using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Outline))]
public class DetectClick : MonoBehaviour
{
    public UnityEvent OnClick;
    public bool clickEnabled, canClick, onlyInteractOnce;
    private Outline _outline;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Click()
    {
        OnClick?.Invoke();
    }

    public void Outline(bool enable)
    {
        _outline.enabled = enable;
        canClick = enable;
    }

    public void Update()
    {
        if (!canClick || !clickEnabled || !Input.GetMouseButtonDown(0)) return;
        Click();
        if (onlyInteractOnce) clickEnabled = canClick = _outline.enabled = false;
    }
}
