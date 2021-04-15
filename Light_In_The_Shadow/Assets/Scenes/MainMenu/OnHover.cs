using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GO;
/*
    public void OnMouseOver()
    {
        GO.SetActive(true);
    }

    public void OnMouseExit()
    {
        GO.SetActive(false);
    }
*/
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        GO.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GO.SetActive(false);
    }
}
