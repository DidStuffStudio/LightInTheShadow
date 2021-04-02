using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {
    public Transform player;
    public Transform receiver;
    private bool _playerIsOverlapping = false;
    [Space] [SerializeField] private int portalNumber;


    // Update is called once per frame
    void Update() {
        if (!_playerIsOverlapping) return;
        if (portalNumber == 2 || portalNumber == 4) return;
        var characterController = player.GetComponent<CharacterController>();
        var portalToPlayer = player.position - transform.position;
        characterController.enabled = false;
        var rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
        rotationDiff += 180;
        player.Rotate(Vector3.up, rotationDiff);
        var positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
        player.position = receiver.position + positionOffset;
        _playerIsOverlapping = false;
        characterController.enabled = true;


        if (portalNumber == 1) {
            MasterManager.Instance.StartLoadingNextScene();
            MasterManager.Instance.soundtrackMaster.PortalSoundsVolume(100.0f, 3.0f);
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(2, true);
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(3, true); //Play swoosh
        }
        else if(portalNumber == 3) {
            MasterManager.Instance.ToggleNeurons(false);
            MasterManager.Instance.soundtrackMaster.PlayPortalSounds(2, true); //Play swoosh
            MasterManager.Instance.soundtrackMaster.PortalSoundsVolume(0.0f, 10.0f);
            MasterManager.Instance.soundtrackMaster.LevelAmbienceVolume(MasterManager.Instance.levelIndex, 100.0f,
                0.1f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) _playerIsOverlapping = true;
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) _playerIsOverlapping = false;
    }
}