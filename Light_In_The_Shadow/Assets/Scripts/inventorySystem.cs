using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventorySystem : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> itemsInInventory = new List<GameObject>();
    public GameObject highlightedItem;
    public GameObject descriptionPanel;
    private PlayerControls playerControls;
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerControls.Player.PickUp.started += _ => highlightObject();
    }

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

    void highlightObject()
    {
        Debug.Log("Mouse is down");

        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit)
        {
            Debug.Log("Hit " + hitInfo.transform.gameObject.name);
        }
    }
}
