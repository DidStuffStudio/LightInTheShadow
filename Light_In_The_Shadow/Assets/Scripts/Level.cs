using UnityEngine;

public class Level : MonoBehaviour {
    // [SerializeField] private GameObject [] 
    // memory fragments for this level
    [SerializeField] private GameObject[] memoryFragments;

    public bool [] puzzlesSolved;

    public void SetPuzzleSolved(int i) {
        // activate the appropriate fragment
        memoryFragments[i].SetActive(true);
    }

    public void ShowMemoryFragments() {
        
    }
    
    
}