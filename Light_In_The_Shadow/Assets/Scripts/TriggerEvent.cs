using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
   public UnityEvent OnTrigger;
   public bool onlyForPlayer;
   public bool isNeuronTrigger, enable;


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

      if (isNeuronTrigger && other.gameObject.CompareTag("Player"))
      {
         MasterManager.Instance.ToggleNeurons(enable);
      }
   }
}
