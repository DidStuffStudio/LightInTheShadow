using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    public string itemName, description;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = itemName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
