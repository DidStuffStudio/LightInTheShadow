using System;
using UnityEngine;

public class PlayerColliderTrigger: MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag(GameConstantStrings.Tags.LoadingScreenTrigger)) {
            MasterManager.Instance.loadingScreen.SetActive(true);
        }
    }
}