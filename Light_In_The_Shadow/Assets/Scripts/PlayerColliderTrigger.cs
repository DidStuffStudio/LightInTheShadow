using System;
using UnityEngine;

public class PlayerColliderTrigger: MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hit something");
        if (other.transform.CompareTag(GameConstantStrings.Tags.LoadingScreenTrigger)) {
            Debug.Log("Player collided with " + other.transform.tag);
            MasterManager.Instance.loadingScreen.SetActive(true);
        }
    }
}