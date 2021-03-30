using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    
    public List<GameObject> itemsInInventory = new List<GameObject>();
    public List<String> idsInInventory = new List<string>();
    public GameObject descriptionPanel,rotatableObject,buttonPanel,itemsholder;
    [SerializeField] private GameObject rotatePivot;
    [SerializeField] private  float _sensitivity = 0.5f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    [SerializeField] private Camera _uiCamera;

    private Vector3 _mousePreviousPosition = Vector3.zero, _mouseDeltaPostion = Vector3.zero;
    private void Update()
    {

        
        
        if(!rotatableObject || !descriptionPanel.activeSelf) return;
        if (Input.GetMouseButton(0))
        {
            var cameraRightVector = _uiCamera.transform.right;
            _mouseDeltaPostion = Input.mousePosition - _mousePreviousPosition;
            if (Vector3.Dot(rotatePivot.transform.up, Vector3.up) >= 0)
            {
                rotatableObject.transform.Rotate(rotatePivot.transform.up, Vector3.Dot(_mouseDeltaPostion, cameraRightVector), Space.World);
            }

            else
            {
                rotatableObject.transform.Rotate(rotatePivot.transform.up, -Vector3.Dot(_mouseDeltaPostion, cameraRightVector), Space.World);
            }
        
            rotatableObject.transform.Rotate(cameraRightVector, Vector3.Dot(_mouseDeltaPostion, _uiCamera.transform.up), Space.World);
        }

        _mousePreviousPosition = Input.mousePosition;
        
    }
    
    public void showHightlightedItem(GameObject item)
    {
        if (item == rotatableObject) return;
        if(rotatableObject) Destroy(rotatableObject);
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<item>().itemName;
        descriptionPanel.transform.GetChild(1).GetComponent<Text>().text = item.GetComponent<item>().description;
        GameObject meme = Instantiate(item,rotatePivot.transform);
        meme.transform.localPosition = Vector3.zero;
        meme.transform.localScale *= 10;
        meme.layer = 0;
        rotatableObject = meme;
    }
    
    
    /*public void dropItem()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            if (itemsInInventory[i].name == highlightedItem.name)
            {
                GameObject removeItem = itemsInInventory[i];
                GameObject insceneItem = GameObject.Find(removeItem.name);
                Destroy(highlightedItem);
                removeItem.SetActive(true);
                removeItem.transform.position = new Vector3(player.position.x+1, player.position.y, player.position.z+1);
                itemsInInventory.Remove(removeItem);
                highlightedItem = null;
                descriptionPanel.SetActive(false);
            }
        }
   
    }*/
    
 
     
}

