using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tenticle : MonoBehaviour
{
   public Transform player;
   public GameObject ikTarget, ikPole;

   public float amplitude = 1.0f;
   public float frequency = 1.0f;

   private bool attack, hasPoint;
   private Vector3 restPosition;
   private Vector3 target;
   public float roamRange = 5.0f;

   public GameObject transformCheck;
   private void Start()
   {
      restPosition = ikTarget.transform.position;
   }

   private void Update()
   {
      if (attack) Attack();
      else Patrolling();
   }
   
   private void Patrolling()
   {
      if (hasPoint) 
         ikTarget.transform.position = Vector3.MoveTowards(ikTarget.transform.position, target, 0.1f);
      else SearchPoint();


      Vector3 distanceToTarget = ikTarget.transform.position - target;

      if (distanceToTarget.magnitude < 1)
      {
         hasPoint = false;
      }
   }



    void SearchPoint()
    {
       target = Random.insideUnitSphere;
       target += new Vector3(0,2,0);
       target *= roamRange;
       target += transform.position;
       transformCheck.transform.position = target;
       hasPoint = true;
    }

    void Attack()
    {
       target = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);
       var noise =  new Vector3(
          Mathf.PerlinNoise(1, Time.time * frequency) * 2 - 1,
          Mathf.PerlinNoise(981 + 1, Time.time * frequency) * 2 - 1,
          Mathf.PerlinNoise(109 + 2, Time.time * frequency) * 2 - 1
       ) * amplitude;
       ikPole.transform.position += noise;

       ikTarget.transform.position = Vector3.MoveTowards(ikTarget.transform.position, target, 0.1f);
    }

   private void OnTriggerEnter(Collider other)
   {
      attack = true;
   }

   private void OnTriggerExit(Collider other)
   {
      attack = false;
   }
}
