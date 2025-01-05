using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace pw_SaveManage
{
    /// <summary>
    /// Static utility class for reading and writing .pwdat files.
    /// </summary>
    public static class pwdat
    {
        /// <summary>
        /// Create a new GameSaveData with default values, except for gameName and seed.
        /// </summary>
        /// <param name="gameName">Name of the new game/save</param>
        /// <param name="seed">Seed for world generation</param>
        /// <returns>A new initialized GameSaveData object</returns>
        public static GameSaveData CreateNewGameSaveData(string gameName, string seed)
        {
            // Create an empty GameSaveData with default constructor
            GameSaveData newData = new GameSaveData();

            // Assign variables from method arguments
            newData.gameName = gameName;
            newData.seed = seed;

            // Set creation time to current date/time
            newData.creationTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Optionally set or override any other default fields
            // newData.version is "0.1" by default from constructor

            // Initialize playerData with some default starting values
            newData.playerData = new PlayerData
            {
                health = 20f,  // Full health (example)
                posX = 0f,
                posY = 100f,   // Example spawn height
                posZ = 0f,
                rotX = 0f,
                rotY = 0f,
                inventory = new List<ItemData>()  // Empty inventory
            };

            // If needed, you can add default chunks or leave it empty:
            // newData.chunks = new List<ChunkData>();

            return newData;
        }

        /// <summary>
        /// Write game save data to a .pwdat file in JSON format.
        /// </summary>
        /// <param name="filePath">Absolute path to the .pwdat file.</param>
        /// <param name="data">GameSaveData object to serialize and save.</param>
        public static void SavePwdat(string filePath, GameSaveData data)
        {
            if (data == null)
            {
                Debug.LogWarning("pwdat: SavePwdat -> data is null, cannot save.");
                return;
            }

            // Convert data to JSON
            string jsonContent = JsonUtility.ToJson(data, true);

            // Ensure parent directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Write JSON to file
            File.WriteAllText(filePath, jsonContent);
            Debug.Log($"pwdat: SavePwdat -> File saved at {filePath}");
        }

        /// <summary>
        /// Read game save data from a .pwdat file in JSON format.
        /// </summary>
        /// <param name="filePath">Absolute path to the .pwdat file.</param>
        /// <returns>GameSaveData object or null if read fails.</returns>
        public static GameSaveData LoadPwdat(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"pwdat: LoadPwdat -> File not found at {filePath}");
                return null;
            }

            try
            {
                // Read file content as JSON
                string jsonContent = File.ReadAllText(filePath);
                var data = JsonUtility.FromJson<GameSaveData>(jsonContent);
                Debug.Log($"pwdat: LoadPwdat -> Successfully loaded file {filePath}");
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"pwdat: LoadPwdat -> Error reading file: {ex.Message}");
                return null;
            }
        }
    }

    /// <summary>
    /// The main save data class for a Minecraft-like game.
    /// </summary>
    [System.Serializable]
    public class GameSaveData
    {
        // Basic info
        public string gameName;       // Name of the save/game
        public string seed;           // World seed
        public string creationTime;   // Creation timestamp
        public string version;        // Data version, e.g. "0.1"

        // Player-related data
        public PlayerData playerData;

        // World-related data, e.g. chunk-based structures
        public List<ChunkData> chunks;

        // Constructor to initialize default values
        public GameSaveData()
        {
            version = "0.1";  // Example version
            chunks = new List<ChunkData>();
        }
    }

    /// <summary>
    /// Stores player-related data, such as position, health, inventory, etc.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        public float health = 20f;

        // Position and rotation
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;

        // Simple inventory
        public List<ItemData> inventory;

        public PlayerData()
        {
            inventory = new List<ItemData>();
        }
    }

    /// <summary>
    /// Represents a chunk in a voxel world.
    /// </summary>
    [System.Serializable]
    public class ChunkData
    {
        public int chunkX;
        public int chunkZ;

        public List<BlockData> blocks;

        public ChunkData()
        {
            blocks = new List<BlockData>();
        }
    }

    /// <summary>
    /// Represents a single block in a chunk.
    /// </summary>
    [System.Serializable]
    public class BlockData
    {
        public int localX;
        public int localY;
        public int localZ;
        public string blockId;  // Could be an enum or string ID
    }

    /// <summary>
    /// Represents an item in the player's inventory.
    /// </summary>
    [System.Serializable]
    public class ItemData
    {
        public string itemId;
        public int quantity;
        // Additional fields: durability, enchantments, etc.
    }
}
