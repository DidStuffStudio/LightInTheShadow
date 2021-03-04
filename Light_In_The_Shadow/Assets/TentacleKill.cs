using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TentacleKill : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.transform.CompareTag("Player")) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }
}
