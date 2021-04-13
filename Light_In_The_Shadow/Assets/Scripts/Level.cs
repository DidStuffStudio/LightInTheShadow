using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    // [SerializeField] private GameObject [] 
    // memory fragments for this level
    [SerializeField] private GameObject[] memoryFragments, memorySlots;
    [SerializeField] private GameObject portalBlock;
    [SerializeField] private String[] memoryIds = new string[2];
    public int _puzzlesSolved = 0, numberOfPuzzles = 2;
    private bool _puzzlesCompleted;

    private InventorySystem _inventorySystem;
    private void Start()
    {
        _inventorySystem = MasterManager.Instance.inventory;
        
    }

    public void SetPuzzleSolved(int i) {
        // activate the appropriate fragment
        memoryFragments[i].SetActive(true);
        memorySlots[i].SetActive(false);
        _inventorySystem.RemoveItem(memoryIds[i]);
        _puzzlesSolved++;
        if (_puzzlesSolved >= numberOfPuzzles)
        {
            _puzzlesCompleted = true;
            portalBlock.GetComponent<Collider>().enabled = false;
        }
    }

    public void CheckInventoryForMemory(int i)
    {
        print("Checking the inventory");
        foreach (var id in _inventorySystem.idsInInventory.Where(id => id.Contains(memoryIds[i]))) SetPuzzleSolved(i);
    }

    void Update()
    {
        if (!_puzzlesCompleted) return;
        
        portalBlock.transform.localScale -= Vector3.one*0.002f;
        if (!(Vector3.Distance(portalBlock.transform.localScale, Vector3.zero) <= 0.1)) return;
        portalBlock.SetActive(false);
        Destroy(gameObject);
    }


    }