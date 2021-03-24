using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> itemsInInventory = new List<GameObject>();
    public GameObject highlightedItem;
    public GameObject descriptionPanel,buttonPanel,itemsholder;
    public GameObject rotatableObject;
    public float _sensitivity = 0.5f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && rotatableObject)
        {
            print("mouse down");
            _isRotating = true;
            _mouseReference = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))_isRotating = false;
        if (!_isRotating) return;
        _mouseOffset.x = ( _mouseReference.x-Input.mousePosition.x );
        _mouseOffset.y = ( _mouseReference.y-Input.mousePosition.y );
        _rotation.z = _mouseOffset.x * _sensitivity;
        _rotation.x = _mouseOffset.y * _sensitivity;
        rotatableObject.transform.Rotate(_rotation, Space.World);
        //rotatableObject.transform.localRotation = quaternion.Euler(_mouseOffset * _sensitivity);
    }

    // Update is called once per frame
    public void showHightlightedItem(GameObject item)
    {
        if (item == rotatableObject) return;
        if(rotatableObject) Destroy(rotatableObject);
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<item>().itemName;
        descriptionPanel.transform.GetChild(1).GetComponent<Text>().text = item.GetComponent<item>().description;
        GameObject meme = Instantiate(item,descriptionPanel.transform.parent);
        meme.transform.localPosition = new Vector3(0,0,300);
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

