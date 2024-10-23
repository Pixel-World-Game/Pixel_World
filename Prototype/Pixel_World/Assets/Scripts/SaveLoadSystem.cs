using UnityEngine;
using System.IO;
using Data;

public class SaveLoadSystem : MonoBehaviour{
    public Inventory inventory; // Reference to the Inventory script
    public Transform playerTransform; // Reference to the player's Transform

    public string saveFileName = "savegame.json"; // File name for saving the game

    void Start(){
        // Optional: Load the saved game data on start
        LoadGame();
    }

    public void SaveGame(){
        GameData gameData = new GameData();

        // Save block information
        foreach (var block in FindObjectsOfType<Block>()){
            BlockData blockData = new BlockData{
                blockType = block.blockType, // Identifier for the type of block
                position = block.transform.position
            };
            gameData.blocks.Add(blockData);
        }

        // Save player information
        gameData.playerPosition = playerTransform.position;

        // Save inventory quantities
        gameData.inventoryQuantities = new int[inventory.blockQuantities.Length];
        for (int i = 0; i < inventory.blockQuantities.Length; i++){
            gameData.inventoryQuantities[i] = inventory.blockQuantities[i];
        }

        // Convert GameData to JSON
        string jsonData = JsonUtility.ToJson(gameData, true);

        // Save JSON to file
        string path = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(path, jsonData);

        Debug.Log("Game Saved to " + path);
    }

    public void LoadGame(){
        string path = Path.Combine(Application.persistentDataPath, saveFileName);

        // Check if the save file exists
        if (File.Exists(path)){
            string jsonData = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(jsonData);

            // Clear existing blocks
            foreach (var block in FindObjectsOfType<Block>()){
                Destroy(block.gameObject);
            }

            // Load blocks from saved data
            foreach (var blockData in gameData.blocks){
                GameObject blockPrefab = inventory.GetBlockPrefabByType(blockData.blockType);
                Instantiate(blockPrefab, blockData.position, Quaternion.identity);
            }

            // Load player position
            playerTransform.position = gameData.playerPosition;

            // Load inventory quantities
            for (int i = 0; i < inventory.blockQuantities.Length; i++){
                inventory.blockQuantities[i] = gameData.inventoryQuantities[i];
            }

            Debug.Log("Game Loaded from " + path);
        }
        else{
            Debug.LogWarning("Save file not found!");
        }
    }
}