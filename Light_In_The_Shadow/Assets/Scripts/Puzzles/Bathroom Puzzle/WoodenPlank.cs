using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenPlank : MonoBehaviour {
    [SerializeField] private BathroomPuzzle _bathroomPuzzle;
    [SerializeField] private Crowbar _crowbar;
    public bool isDetached; // is this wooden plank still nailed to the window?

    private Rigidbody _rigidbody;
    private Outline _outline;
    private bool mouseIsOver;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
    }

    private void OnMouseOver() {
        mouseIsOver = true;
        _outline.enabled = true;
    }

    private void OnMouseExit() {
        mouseIsOver = false;
        _outline.enabled = false;
    }

    private void OnMouseDown() {
        if(mouseIsOver) SetSelfSelected(true);
    }

    public void Detach() {
        print("Detached " + this.gameObject.name);
        isDetached = true;
        // set the gravity to true
        _rigidbody.useGravity = true;
        // add a force to the rigidbody so it goes back after detaching it
        AddForce(20.0f);
        SetSelfSelected(false);
        _bathroomPuzzle.detachedWoodenPlanks++;
        _rigidbody.isKinematic = false;
    }

    public void AddForce(float force) {
        Vector3 direction = _crowbar.transform.position - transform.position;
        _rigidbody.AddForceAtPosition(direction.normalized * force, transform.position);
    }

    public void SetSelfSelected(bool isSelected) {
        if (isSelected) {
            print("Setting the wooden plank and crowbar");
            _bathroomPuzzle.SetCrowbar(_crowbar.gameObject);
            _crowbar.gameObject.SetActive(true);
            _bathroomPuzzle.SelectWoodenPlank(this);
        }
        else {
            _crowbar.gameObject.SetActive(false);
            _bathroomPuzzle.SetCrowbar(null);
            _bathroomPuzzle.SelectWoodenPlank(null);
        }
    }
    
    
    
}