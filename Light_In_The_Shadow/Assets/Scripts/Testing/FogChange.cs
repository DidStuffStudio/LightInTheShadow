using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FogChange : MonoBehaviour
{
    private Color targetColor;
    private float timeLeft;
    public Light sun;
    private void Update()
    {
        
            
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            RenderSettings.fogColor = targetColor;
            sun.color = targetColor;
 
            // start a new transition
            targetColor = new Color(Random.value, Random.value, Random.value);
            timeLeft = 30.0f;
        }
        else
        {
            // transition in progress
            // calculate interpolated color
            RenderSettings.fogColor  = Color.Lerp(RenderSettings.fogColor, targetColor, Time.deltaTime / timeLeft);
            sun.color  = Color.Lerp(sun.color, targetColor, Time.deltaTime / timeLeft);
 
            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }
}
