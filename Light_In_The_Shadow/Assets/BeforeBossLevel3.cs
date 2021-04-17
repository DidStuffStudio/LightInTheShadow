using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeBossLevel3 : MonoBehaviour
{
    [SerializeField] GameObject[] childObjects = new GameObject[4];
    [SerializeField] GameObject[] dadObjects = new GameObject[4];
    [SerializeField] private DetectClick detectClick;
    
    public void PlayJumping()
    {
        childObjects[0].GetComponent<Animator>().Play("ChildJump");
    }

    public void PlayChase()
    {
        childObjects[0].SetActive(false);
        childObjects[1].SetActive(true);
        dadObjects[0].SetActive(true);
    }

    public void PlayStrangle()
    {
        childObjects[1].SetActive(false);
        dadObjects[0].SetActive(false);
        //childObjects[2].SetActive(true);
        dadObjects[1].SetActive(true); //Just one object as of now
        detectClick.clickEnabled = true;
    }

    public void PlayDadFall()
    {
        foreach (var go in childObjects)
        {
            go.SetActive(false);
        }
        foreach (var go in dadObjects)
        {
            go.SetActive(false);
        }
    }
}
