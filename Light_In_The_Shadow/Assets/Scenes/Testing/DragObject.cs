using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 mOffset;
    public GameObject antennaBase;
    private float mZCoord;
    void OnMouseDown()

    {

        mZCoord = Camera.main.WorldToScreenPoint(
            
            gameObject.transform.position).z;
        
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }



    private Vector3 GetMouseAsWorldPoint()

    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;
        
        return Camera.main.ScreenToWorldPoint(mousePoint);

    }



    void OnMouseDrag()

    {
        transform.RotateAround(antennaBase.transform.position,Vector3.left,GetMouseAsWorldPoint().y + mOffset.y);
    }

}
