using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flicker : MonoBehaviour
{

    public GameObject lightCone;
    public Light spotlight;
    
    
    void Update() 
    {
        if ( Random.value > 0.9 ) //a random chance
        {
            if ( lightCone.activeSelf) //if the light is on...
            {
                lightCone.SetActive(false);
                spotlight.intensity = 4.0f; //turn it off
            }
            else
            {
                lightCone.SetActive(true);
                spotlight.intensity = 15.0f;//turn it on
            }
        }
    }
}
