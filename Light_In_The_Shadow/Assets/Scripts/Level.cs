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
    [SerializeField] private bool removeItem = false;
    [SerializeField] private String itemToRemoveID;

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
        StartCoroutine(MasterManager.Instance.player.InventoryRemoveInform("a memory"));
        if (_puzzlesSolved >= numberOfPuzzles)
        {
            _puzzlesCompleted = true;
            portalBlock.GetComponent<Collider>().enabled = false;

            if (!removeItem) return;
            _inventorySystem.RemoveItem(itemToRemoveID);
        }
    }

    public void CheckInventoryForMemory(int i)
    {
        var hasMemory = false;
        
            foreach (var t in _inventorySystem.idsInInventory.Where(t => t.Contains(memoryIds[i]))) hasMemory = true;

            if(hasMemory)SetPuzzleSolved(i);
        
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