using UnityEngine;
using System.Collections.Generic;
using pw_SaveManage; // For ChunkData, BlockData, etc.

namespace pw_Game.Environment
{
    /// <summary>
    /// Manages generation, loading, unloading, and updating of chunks in a Minecraft-like world.
    /// </summary>
    public class ChunkManager : MonoBehaviour
    {
        /// <summary>
        /// Dictionary or map of currently loaded chunks, keyed by a chunk coordinate (e.g., "x_z").
        /// </summary>
        private Dictionary<string, GameObject> loadedChunkObjects = new Dictionary<string, GameObject>();

        /// <summary>
        /// Initialize the ChunkManager with some seed or other settings if needed.
        /// </summary>
        /// <param name="seed">The world's seed, used for generation.</param>
        public void Initialize(string seed)
        {
            // Example: store the seed locally or parse it for random gen
            Debug.Log($"ChunkManager: Initialized with seed='{seed}'.");
        }

        /// <summary>
        /// Generate or spawn chunks based on data from save (e.g., chunk list).
        /// </summary>
        /// <param name="chunkDataList">List of chunk data loaded from the .pwdat file.</param>
        public void LoadChunksFromData(List<ChunkData> chunkDataList)
        {
            if (chunkDataList == null || chunkDataList.Count == 0)
            {
                Debug.LogWarning("ChunkManager: No chunk data to load.");
                return;
            }

            foreach (var chunkData in chunkDataList)
            {
                SpawnChunk(chunkData);
            }

            Debug.Log($"ChunkManager: Loaded {chunkDataList.Count} chunks from data.");
        }

        /// <summary>
        /// Generate a brand-new set of chunks procedurally (e.g., if no chunks are saved or it's a new world).
        /// The example below just spawns a single chunk at (0,0).
        /// </summary>
        public void GenerateInitialChunks()
        {
            // Example: generate one chunk or a set of chunks around (0,0)
            var chunkData = new ChunkData
            {
                chunkX = 0,
                chunkZ = 0,
                blocks = new List<BlockData>()
                // ... fill with default blocks, or keep empty
            };

            SpawnChunk(chunkData);
            Debug.Log("ChunkManager: Generated initial chunks (example only).");
        }

        /// <summary>
        /// Spawns or loads a chunk in the scene from chunk data.
        /// For example, you might build a mesh or place block objects here.
        /// </summary>
        /// <param name="chunkData">The chunk's coordinate and block data.</param>
        private void SpawnChunk(ChunkData chunkData)
        {
            // Example key for dictionary
            string chunkKey = $"{chunkData.chunkX}_{chunkData.chunkZ}";
            if (loadedChunkObjects.ContainsKey(chunkKey))
            {
                Debug.LogWarning($"ChunkManager: Chunk at ({chunkData.chunkX}, {chunkData.chunkZ}) already loaded.");
                return;
            }

            // Create a placeholder GameObject for the chunk
            GameObject chunkObj = new GameObject($"Chunk_{chunkData.chunkX}_{chunkData.chunkZ}");
            chunkObj.transform.SetParent(this.transform, false);

            // Optionally add a "Chunk" component to handle chunk-specific logic
            // Chunk chunkComponent = chunkObj.AddComponent<Chunk>();
            // chunkComponent.InitializeChunk(chunkData);

            // Example: position it based on chunk coordinates (assuming each chunk is 16x16 in X/Z)
            float chunkSize = 16f; // Example chunk size
            chunkObj.transform.position = new Vector3(
                chunkData.chunkX * chunkSize,
                0f,
                chunkData.chunkZ * chunkSize
            );

            // Insert into dictionary
            loadedChunkObjects[chunkKey] = chunkObj;

            // If we want to build actual meshes for blocks, we could do it here or in a Chunk component
            // e.g., BuildChunkMesh(chunkData, chunkObj);
        }

        /// <summary>
        /// Unload or destroy all currently loaded chunks. 
        /// Example: when unloading the world or changing scenes.
        /// </summary>
        public void UnloadAllChunks()
        {
            foreach (var kvp in loadedChunkObjects)
            {
                var chunkObj = kvp.Value;
                if (chunkObj != null)
                {
                    Destroy(chunkObj);
                }
            }

            loadedChunkObjects.Clear();
            Debug.Log("ChunkManager: Unloaded all chunks.");
        }

        /// <summary>
        /// Example update method to handle chunk streaming or LOD if needed.
        /// </summary>
        /// <param name="playerPosition">Current player position to decide if new chunks are needed.</param>
        public void UpdateChunks(Vector3 playerPosition)
        {
            // Placeholder for advanced logic:
            // - Check how far the player is from existing chunks
            // - Spawn new chunks if the player goes beyond a certain range
            // - Unload distant chunks to save memory
        }
    }
}
