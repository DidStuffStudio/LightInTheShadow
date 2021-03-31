using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;

    [SerializeField] private bool isInternalLevelPortal;
    private bool _playerIsOverlapping = false;

    [SerializeField] private bool isFinalPortal = false;

    // Update is called once per frame
    void Update()
    {
        if (_playerIsOverlapping && !isFinalPortal)
        {
            Vector3 portalToPlayer = player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
            // If this is true: The player has moved across the portal - so teleport him
            print(dotProduct);
            if (dotProduct < 0f)
            {
                player.GetComponent<CharacterController>().enabled = false;
                float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
                rotationDiff += 180;
                player.Rotate(Vector3.up, rotationDiff);

                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                player.position = receiver.position + positionOffset;


                _playerIsOverlapping = false;
                player.GetComponent<CharacterController>().enabled = true;
                if (isInternalLevelPortal) return;

                MasterManager.Instance.StartLoadingNextScene();
            }
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