using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class FadeInScene : MonoBehaviour

{
  
    //Renderer rend;
    private Shader shader;
    public bool fadeInNow;
    [HideInInspector]public float increment = 0.0f;
    //public GameObject GO;
    Renderer[] children;
    public float speed;
    [SerializeField] private float maxDistance, minDistance;
    public bool reverse;


    void Start()
    {
        children = GetComponentsInChildren<Renderer>();

    }

    void Update()
    {
        
        if (!fadeInNow) return;
        
        if (!reverse)
        {
            if (increment < maxDistance)
            {
                foreach (Renderer rend in children)
                {
                    if (fadeInNow)
                    {
                        increment += speed;
                    }

                    rend.material.SetFloat("_Radius", increment);
                }
            }
            else
            {
                increment = maxDistance;
                reverse = true;
                fadeInNow = false;
            }
        }
        
        
        if (reverse)
        {
            if (increment > minDistance)
            {
                foreach (Renderer rend in children)
                {
                    if (fadeInNow)
                    {
                        increment -= speed;
                    }


                    rend.material.SetFloat("_Radius", increment);
                }
            }
            else
            {
                increment = minDistance;
                reverse = false;
                fadeInNow = false;
            }
        }
      
    }
}
