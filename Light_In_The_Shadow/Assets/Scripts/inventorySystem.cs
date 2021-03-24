using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> itemsInInventory = new List<GameObject>();
    public GameObject highlightedItem;
    public GameObject descriptionPanel,buttonPanel,itemsholder;
    public GameObject rotatableObject;

    private void Update()
    {
        if (Input.GetMouseButton(0) && rotatableObject != null)
        {

                rotatableObject.transform.rotation = Quaternion.Euler(-Input.mousePosition.x, -Input.mousePosition.y, -Input.mousePosition.z);
        }
       
    }

    // Update is called once per frame
    public void showHightlightedItem(GameObject item)
    {
        highlightedItem = item;
        descriptionPanel.SetActive(true);
        descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = item.GetComponent<item>().itemName;
        descriptionPanel.transform.GetChild(1).GetComponent<Text>().text = item.GetComponent<item>().description;
        GameObject meme = Instantiate(item,descriptionPanel.transform.parent);
        meme.transform.localPosition = new Vector3(0,0,200);
        meme.transform.localScale *= 10;
        meme.layer = 0;
        rotatableObject = meme;
    }
    public void dropItem()
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
        

        
    }

    

    
}
