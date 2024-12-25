using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace Constructor.Space{
/// <summary>
/// A single class named 'Environment' that manages map data, changes, 
/// and serialization (save/load) using a 32-character hash.
/// </summary>
    public class Environment{
        /// <summary>
        /// Nested serializable structure to represent a single change in the map.
        /// </summary>
        [Serializable]
        private struct ChangeRecord
        {
            /// <summary>
            /// The coordinate where the change happened.
            /// </summary>
            public Vector3Int position;

            /// <summary>
            /// The new block ID (or type) placed at that position.
            /// </summary>
            public int newBlockId;

            /// <summary>
            /// Constructor for creating a change record.
            /// </summary>
            public ChangeRecord(Vector3Int position, int newBlockId)
            {
                this.position = position;
                this.newBlockId = newBlockId;
            }
        }

        /// <summary>
        /// Nested serializable class to store all environment data, including the map and its changes.
        /// </summary>
        [Serializable]
        private class EnvironmentData
        {
            /// <summary>
            /// A 32-character hash used to generate the map.
            /// </summary>
            public string seedHash;

            /// <summary>
            /// Dictionary representing map data:
            /// Key: Vector3Int (x, y, z) for the coordinates.
            /// Value: int representing the block type or ID.
            /// </summary>
            public Dictionary<Vector3Int, int> mapData = new Dictionary<Vector3Int, int>();

            /// <summary>
            /// List of all recorded changes (e.g., placing or removing blocks).
            /// </summary>
            public List<ChangeRecord> changes = new List<ChangeRecord>();
        }

        /// <summary>
        /// Holds all map and change data for the environment.
        /// </summary>
        private EnvironmentData data;

        /// <summary>
        /// Constructor that requires a 32-character hash string to initialize the map.
        /// </summary>
        /// <param name="hash">32-character hash string.</param>
        public Environment(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 32)
            {
                Debug.LogError("The provided hash must be exactly 32 characters.");
                return;
            }

            data = new EnvironmentData();
            data.seedHash = hash;

            // Generate the map based on the hash (simple example).
            GenerateMapByHash(hash);
        }

        /// <summary>
        /// Parameterless constructor (optional).
        /// Useful if you plan to call LoadGame(...) afterward 
        /// without needing a hash immediately.
        /// </summary>
        public Environment()
        {
            // Leave empty if you plan to load later.
        }

        /// <summary>
        /// Example method to generate the map using the given hash.
        /// In a real project, you could use noise algorithms or custom logic.
        /// </summary>
        private void GenerateMapByHash(string hash)
        {
            // Example: parse the first 8 characters as a hexadecimal number for a seed.
            string seedString = hash.Substring(0, 8);
            int seed = Convert.ToInt32(seedString, 16);
            UnityEngine.Random.InitState(seed);

            // Generate random blocks in a small area as a demonstration.
            for (int i = 0; i < 10; i++)
            {
                int x = UnityEngine.Random.Range(-5, 6);
                int y = 0; // example Y = 0
                int z = UnityEngine.Random.Range(-5, 6);
                Vector3Int position = new Vector3Int(x, y, z);
                data.mapData[position] = 1; // Assign block ID '1'
            }
        }

        /// <summary>
        /// Records a change to the map (placing/removing a block, etc.).
        /// </summary>
        /// <param name="position">Coordinate where the change occurs.</param>
        /// <param name="newBlockId">Block ID set at that position.</param>
        public void RecordChange(Vector3Int position, int newBlockId)
        {
            // Update the map data.
            if (data == null)
            {
                Debug.LogError("Environment data has not been initialized.");
                return;
            }

            data.mapData[position] = newBlockId;

            // Create and store a change record.
            ChangeRecord record = new ChangeRecord(position, newBlockId);
            data.changes.Add(record);
        }

        /// <summary>
        /// Saves the environment data (map + changes) to the specified file path.
        /// </summary>
        /// <param name="filePath">Path to the save file.</param>
        public void SaveGame(string filePath)
        {
            if (data == null)
            {
                Debug.LogError("No environment data to save. Make sure it is initialized.");
                return;
            }

            try
            {
                // Convert the data to JSON.
                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(filePath, json);
                Debug.Log($"Environment saved to: {filePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save environment: {e.Message}");
            }
        }

        /// <summary>
        /// Loads the environment data from the specified file path.
        /// </summary>
        /// <param name="filePath">Path to the save file.</param>
        public void LoadGame(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Save file not found: {filePath}");
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                EnvironmentData loadedData = JsonUtility.FromJson<EnvironmentData>(json);

                if (loadedData != null)
                {
                    data = loadedData;
                    Debug.Log("Environment successfully loaded from file.");
                }
                else
                {
                    Debug.LogError("Failed to parse the save file. Data is null.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load environment: {e.Message}");
            }
        }

        /// <summary>
        /// Retrieves the current seed hash (if available).
        /// </summary>
        /// <returns>32-character hash or null if not initialized.</returns>
        public string GetSeedHash()
        {
            return data?.seedHash;
        }

        /// <summary>
        /// Gets or sets the dictionary containing the map data.
        /// Key: Vector3Int coordinate, Value: int block ID.
        /// </summary>
        public Dictionary<Vector3Int, int> MapData
        {
            get => data?.mapData;
            set
            {
                if (data != null)
                {
                    data.mapData = value;
                }
                else
                {
                    Debug.LogError("Cannot set MapData because environment data is null.");
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of changes (ChangeRecord structs).
        /// </summary>
        public List<Vector3Int> ChangedPositions
        {
            get
            {
                // Return just the positions if needed.
                if (data == null) return null;

                List<Vector3Int> positions = new List<Vector3Int>();
                foreach (var change in data.changes)
                {
                    positions.Add(change.position);
                }
                return positions;
            }
        }

        /// <summary>
        /// Gets or sets the full list of ChangeRecord instances.
        /// </summary>
        private List<ChangeRecord> Changes
        {
            get => data?.changes;
            set
            {
                if (data != null)
                {
                    data.changes = value;
                }
                else
                {
                    Debug.LogError("Cannot set Changes because environment data is null.");
                }
            }
        }
    }
}