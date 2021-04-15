using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceScript : MonoBehaviour
{
    public int counterForIce;
    public int thresholdForIce;
   
    void Update()
    {
        if (counterForIce > thresholdForIce)
        {
            //Break Ice
        }
    }
}
