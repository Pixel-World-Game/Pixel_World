using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour{
    public Image[] slotImages; // UI images representing inventory slots
    public Color highlightColor = Color.grey;
    private Inventory inventory;

    void Start(){
        inventory = FindObjectOfType<Inventory>();
        UpdateUI();
    }

    void Update(){
        UpdateUI();
    }

    void UpdateUI(){
        for (int i = 0; i < slotImages.Length; i++){
            if (i == inventory.selectedBlockIndex){
                slotImages[i].color = highlightColor; // Highlight selected slot
            }
            else{
                slotImages[i].color = Color.white; // Default color for unselected slots
            }
        }
    }
}