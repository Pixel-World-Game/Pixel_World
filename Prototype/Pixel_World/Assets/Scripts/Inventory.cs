using UnityEngine;

public class Inventory : MonoBehaviour{
    public GameObject[] blockPrefabs; // Array holding different block types
    public int[] blockQuantities; // Quantities for each block type
    public int selectedBlockIndex = 0; // Currently selected block

    void Start(){
        // Initialize block quantities (for example, start with 10 blocks of each type)
        blockQuantities = new int[blockPrefabs.Length];
        for (int i = 0; i < blockQuantities.Length; i++){
            blockQuantities[i] = 10; // Initial quantity for each block
        }
    }

    void Update(){
        HandleInput();
    }

    void HandleInput(){
        // Switch block type based on number keys (1-9)
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            selectedBlockIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)){
            selectedBlockIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)){
            selectedBlockIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)){
            selectedBlockIndex = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)){
            selectedBlockIndex = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)){
            selectedBlockIndex = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)){
            selectedBlockIndex = 6;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8)){
            selectedBlockIndex = 7;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)){
            selectedBlockIndex = 8;
        }
        // Continue as needed for more block types

        // Scroll wheel to change the selection
        if (Input.GetAxis("Mouse ScrollWheel") > 0f){
            selectedBlockIndex = (selectedBlockIndex + 1) % blockPrefabs.Length;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f){
            selectedBlockIndex--;
            if (selectedBlockIndex < 0) selectedBlockIndex = blockPrefabs.Length - 1;
        }
    }

    public GameObject GetSelectedBlockPrefab(){
        return blockPrefabs[selectedBlockIndex];
    }

    public int GetSelectedBlockIndex(){
        return selectedBlockIndex;
    }

    public void UseBlock(){
        if (blockQuantities[selectedBlockIndex] > 0){
            blockQuantities[selectedBlockIndex]--;
        }
    }

    public void GetBlock(){
        blockQuantities[selectedBlockIndex]++;
    }

    public int GetBlockQuantity(int index){
        return blockQuantities[index];
    }
}