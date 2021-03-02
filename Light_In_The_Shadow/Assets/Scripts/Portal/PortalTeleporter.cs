using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour {

	public Transform player;
	public Transform receiver;

	private bool playerIsOverlapping = false;

	// Update is called once per frame
	void Update () {
		if (playerIsOverlapping)
		{
			Vector3 portalToPlayer = player.position - transform.position;
			
			float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				print("teleporting");
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = receiver.position + positionOffset;

				playerIsOverlapping = false;
			}
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.CompareTag("Player"))
		{
			print(playerIsOverlapping);
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}
