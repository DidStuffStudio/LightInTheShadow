using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FadeInScene : MonoBehaviour

{
  
    //Renderer rend;
    private Shader shader;
    public bool fadeInNow;
    private float increment;
    //public GameObject GO;
    Renderer[] children;
    public float speed;



    void Start()
    {
        children = GetComponentsInChildren<Renderer>();

    }

    void Update()
    {
        
        foreach (Renderer rend in children)
        {
            if (fadeInNow)
            {
                for (int i = 0; i < 1; i++)
                {
                    increment += speed;
                }
            }
            else
            {
                increment = 0;
            }
            rend.material.SetFloat("_Radius", increment);
        }
    }
}
