using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBlock : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      print("Collided");
      if (other.gameObject.layer != 10) return;
      var playerController = other.gameObject.GetComponentInChildren<PlayerController>();
      playerController.helpText.text = "This part of the mind is still to unclear to enter yet. Something must be missing...";
      playerController.OpenHelpMenu(true);
   }
}
