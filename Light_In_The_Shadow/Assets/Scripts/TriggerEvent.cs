using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
   public UnityEvent OnTrigger;
   public bool onlyForPlayer;

   private void OnTriggerEnter(Collider other)
   {
      if (onlyForPlayer)
      {
         if (other.gameObject.CompareTag("Player"))
         {
            OnTrigger?.Invoke();
         }
      }
      else
      {
         OnTrigger?.Invoke();
      }
   }
}
