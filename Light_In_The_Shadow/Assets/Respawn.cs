using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    private Vector3 _startingPosition;
    private Quaternion _startingRotation;
    [SerializeField] private float respawnDistance = 50.0f;
    private Matress _matress;
    private void Start()
    {
        _matress = GetComponent<Matress>();
        var transform1 = transform;
        _startingPosition = transform1.position;
        _startingRotation = transform1.rotation;
        StartCoroutine(CheckForDisplacement());
    }

    IEnumerator CheckForDisplacement()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, _startingPosition) > respawnDistance)
            {
                if (_matress.pickedUp)
                {
                    MasterManager.Instance.player.helpText.text = "This matress is too heavy to carry further";
                    MasterManager.Instance.player.OpenHelpMenu(true);
                }
                else if(Vector3.Distance(transform.position, _startingPosition) > respawnDistance + 10 || transform.position.y < 1.0f)
                {
                    _matress.pickedUp = false;
                    _matress.ColliderTrigger(false);
                    MasterManager.Instance.player.AttachObjectToPlayer(gameObject, false);
                    var transform1 = transform;
                    transform1.position = _startingPosition;
                    transform1.rotation = _startingRotation;
                }
            }
                
                
        }
    }
}
