using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class WoodenPlank : MonoBehaviour {
    [SerializeField] private BathroomPuzzle _bathroomPuzzle;
    [SerializeField] private Crowbar _crowbar;
    private DetectClick _detectClick;
    public bool isDetached; // is this wooden plank still nailed to the window?

    private Rigidbody _rigidbody;
    private Outline _outline;
    private bool mouseIsOver;

    public bool pullCrowbarUpWardsToDetach;
    public float force = 500000;
    private bool _addForce;
    private Vector3 _impactPoint;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _outline = GetComponent<Outline>();
        _detectClick = GetComponent<DetectClick>();
        if (_crowbar.pullCrowbarUpWardsToDetach) pullCrowbarUpWardsToDetach = true;
    }

    private void FixedUpdate()
    {
        if(!_addForce) return;
        _rigidbody.AddForceAtPosition(-transform.forward * force, _impactPoint, ForceMode.Impulse);
    }

    IEnumerator WaitBro()
    {
        _addForce = true;
        yield return new WaitForSeconds(2.0f);
        _addForce = false;
    }

    public void Detach(Vector3 hitPoint)
    {
        _bathroomPuzzle.crowbarIsNull = true;
        _bathroomPuzzle._isRotating = false;
        _bathroomPuzzle._rotation = 0.0f;
        _bathroomPuzzle.detachedWoodenPlanks++;
        GetComponent<DetectClick>().clickEnabled = false;
        _crowbar.gameObject.SetActive(false);
        isDetached = true;
        // set the gravity to true
        _rigidbody.useGravity = true;
        // add a force to the rigidbody so it goes back after detaching it
        _impactPoint = hitPoint;
        SetSelfSelected(false);
        _rigidbody.isKinematic = false;
        StartCoroutine(WaitBro());

    }

    public void SetSelfSelected(bool isSelected) {
        if (isSelected) {
            var hasCrowbar = false;
            foreach (var item in MasterManager.Instance.inventory.idsInInventory.Where(id => id.Contains("crowbar"))) hasCrowbar = true;
            if (hasCrowbar)
            {
                print("Setting the wooden plank and crowbar");
                _bathroomPuzzle.SetCrowbar(_crowbar.gameObject);
                _crowbar.gameObject.SetActive(true);
                _bathroomPuzzle.SelectWoodenPlank(this);
            }
            else
            {
                _bathroomPuzzle.FocusOnWoodenPlanks(false);
                MasterManager.Instance.player.helpText.text = "You need something to lever these planks from the window";
                MasterManager.Instance.player.OpenHelpMenu(true);
                
            }
           
        }
        else {
            _bathroomPuzzle.SelectWoodenPlank(null);
            _crowbar.gameObject.SetActive(false);
            _bathroomPuzzle.SetCrowbar(null);
            
        }
    }
    
    
    
}