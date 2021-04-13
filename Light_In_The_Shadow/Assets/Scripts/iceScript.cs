using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iceScript : MonoBehaviour
{
    public int counterForIce;
    public int thresholdForIce;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (counterForIce >thresholdForIce)
        {
            transform.position -= new Vector3(0,-2,0);
        }
       
    }
}
