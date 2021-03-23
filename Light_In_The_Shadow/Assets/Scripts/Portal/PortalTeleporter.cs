using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {
    public Transform player;
    public Transform receiver;

    private bool _playerIsOverlapping = false;

    [SerializeField] private bool isFinalPortal = false;

    // Update is called once per frame
    void Update() {
        if (_playerIsOverlapping && !isFinalPortal) {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
            // If this is true: The player has moved across the portal - so teleport him
            if (dotProduct < 0f) {
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;

                _playerIsOverlapping = false;
                if (!MasterManager.Instance.loadingScreenTransitionStarted)
                    MasterManager.Instance.StartLoadingNextScene();
                else {
                    MasterManager.Instance.LoadingScreenTransitionFinished();
                    player.GetComponent<playerController>().NewRespawnPoint(); // set the new respawning point to the player
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) _playerIsOverlapping = true;
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) _playerIsOverlapping = false;
    }
}