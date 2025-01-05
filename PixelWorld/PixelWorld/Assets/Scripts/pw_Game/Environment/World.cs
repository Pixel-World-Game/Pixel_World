using UnityEngine;
using System.Collections.Generic;
using pw_SaveManage;  // For GameSaveData, ChunkData, etc.

namespace pw_Game.Environment
{
    /// <summary>
    /// Represents a Minecraft-like world object that can be used by the Game,
    /// fully controlled by code rather than Inspector.
    /// </summary>
    public class World : MonoBehaviour
    {
        private string worldName;
        private string worldSeed;
        private bool isInitialized = false;

        private List<ChunkData> loadedChunks = new List<ChunkData>();

        public void Initialize(string name, string seed)
        {
            worldName = name;
            worldSeed = seed;
            isInitialized = false;

            Debug.Log($"World: Initialized with name='{worldName}' and seed='{worldSeed}'");
        }

        public void LoadFromSaveData(GameSaveData saveData)
        {
            if (saveData == null)
            {
                Debug.LogWarning("World: Cannot load from null saveData.");
                return;
            }
            worldName = saveData.gameName;
            worldSeed = saveData.seed;

            loadedChunks = saveData.chunks != null 
                ? new List<ChunkData>(saveData.chunks)
                : new List<ChunkData>();

            isInitialized = false;
            Debug.Log($"World: LoadFromSaveData -> name='{worldName}', seed='{worldSeed}', chunkCount={loadedChunks.Count}");
        }

        public void LoadWorld()
        {
            if (string.IsNullOrEmpty(worldName) || string.IsNullOrEmpty(worldSeed))
            {
                Debug.LogWarning("World: Cannot load because the world name or seed is not set.");
                return;
            }

            isInitialized = true;
            Debug.Log($"World: '{worldName}' loaded successfully with {loadedChunks.Count} chunks.");
        }

        public void UnloadWorld()
        {
            if (!isInitialized)
            {
                Debug.LogWarning($"World: '{worldName}' is not loaded, cannot unload.");
                return;
            }
            loadedChunks.Clear();
            isInitialized = false;

            Debug.Log($"World: '{worldName}' has been unloaded.");
        }

        public void UpdateWorld(float deltaTime)
        {
            if (!isInitialized)
                return;

            // Example: day/night cycle calculation, weather transitions, etc.
        }

        /// <summary>
        /// Generates a simple Perlin noise map for a given chunk area. This can be used by ChunkManager
        /// to create a 3D terrain or block heights. The return is a 2D array of heights.
        /// </summary>
        /// <param name="chunkX">The chunk coordinate X.</param>
        /// <param name="chunkZ">The chunk coordinate Z.</param>
        /// <param name="chunkSize">Size (width and length) of one chunk in blocks.</param>
        /// <param name="scale">Noise scale factor (the smaller, the smoother; the bigger, the more variation).</param>
        /// <param name="amplitude">Maximum height for the terrain.</param>
        /// <param name="offsetX">Optional offset for noise in X direction.</param>
        /// <param name="offsetZ">Optional offset for noise in Z direction.</param>
        /// <returns>A float 2D array representing heights at each [x,z] within the chunk.</returns>
        public float[,] GenerateNoiseMap(
            int chunkX, 
            int chunkZ, 
            int chunkSize, 
            float scale, 
            float amplitude, 
            float offsetX = 0f, 
            float offsetZ = 0f)
        {
            float[,] heightMap = new float[chunkSize, chunkSize];

            // Convert seed string into a pseudo-random offset
            // (Optional) You can parse the seed as int, or combine its hash code.
            int seedInt = worldSeed.GetHashCode();
            Random.InitState(seedInt);
            // Or we can also add a random offset to noise
            float randomOffsetX = Random.Range(-100000f, 100000f) + offsetX;
            float randomOffsetZ = Random.Range(-100000f, 100000f) + offsetZ;

            // Generate height values
            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    // Convert chunk coords and local coords into "global" positions
                    float worldPosX = (chunkX * chunkSize + x) + randomOffsetX;
                    float worldPosZ = (chunkZ * chunkSize + z) + randomOffsetZ;

                    // Scale down
                    worldPosX *= scale;
                    worldPosZ *= scale;

                    // Get perlin noise value in [0..1], multiply by amplitude
                    float noiseValue = Mathf.PerlinNoise(worldPosX, worldPosZ) * amplitude;

                    heightMap[x, z] = noiseValue;
                }
            }

            return heightMap;
        }

        public string GetWorldName() => worldName;
        public string GetWorldSeed() => worldSeed;
        public bool IsWorldInitialized() => isInitialized;
    }
}
