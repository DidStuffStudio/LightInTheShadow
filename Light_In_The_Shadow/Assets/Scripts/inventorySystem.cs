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



    // Update is called once per frame
    void Update()
    {
        if (highlightedItem == null)
        {
            descriptionPanel.SetActive(false);
            
        }
        else
        {
            descriptionPanel.SetActive(true);
            descriptionPanel.transform.GetChild(0).GetComponent<Text>().text = highlightedItem.GetComponent<item>().itemName;
            descriptionPanel.transform.GetChild(1).GetComponent<Text>().text = highlightedItem.GetComponent<item>().description;
        }
        
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
            }
        }
        

        
    }

    

    
}
