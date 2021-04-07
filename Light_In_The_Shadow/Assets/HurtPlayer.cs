using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
   public int damageToHealth = 10;
   private bool _alreadyHurt;
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.layer == 10 && !_alreadyHurt)
      {
         MasterManager.Instance.player.playerHealth -= damageToHealth;
      }
   }
}
