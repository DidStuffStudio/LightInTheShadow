using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;
    private bool _playerIsOverlapping = false;
    [Space]
    [SerializeField] private int portalNumber;



    // Update is called once per frame
    void Update()
    {
        if (!_playerIsOverlapping) return;
        if (portalNumber == 2 || portalNumber == 4) return;
        var portalToPlayer = player.position - transform.position;
        var dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
        // If this is true: The player has moved across the portal - so teleport him
        if (!(dotProduct < 0f)) return;
        player.GetComponent<CharacterController>().enabled = false;
        var rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
        rotationDiff += 180;
        player.Rotate(Vector3.up, rotationDiff);

        var positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
        player.position = receiver.position + positionOffset;

                    
        _playerIsOverlapping = false;
        player.GetComponent<CharacterController>().enabled = true;
                    

        if(portalNumber == 1){

            MasterManager.Instance.StartLoadingNextScene();
            MasterManager.Instance.soundtrackMaster.PortalSoundsVolume(100.0f,3.0f);
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(2, true);
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(1, true); //Play swoosh
        }
        else
        {
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(1, true); //Play swoosh
            MasterManager.Instance.soundtrackMaster.PortalSoundsVolume(0.0f, 10.0f);
            MasterManager.Instance.soundtrackMaster.LevelAmbienceVolume(MasterManager.Instance.levelIndex, 100.0f, 2.0f);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _playerIsOverlapping = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _playerIsOverlapping = false;
    }
}