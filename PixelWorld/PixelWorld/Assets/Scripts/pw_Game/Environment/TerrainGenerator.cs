using UnityEngine;
using System.Collections.Generic;
using pw_SaveManage; // For ChunkData, BlockData

namespace pw_Game.Environment
{
    /// <summary>
    /// Generates terrain data (heights, block types) for a given chunk, based on a seed.
    /// ChunkManager can call methods from this class to build actual chunks.
    /// </summary>
    public static class TerrainGenerator
    {
        /// <summary>
        /// Generates a ChunkData for the specified chunk coordinates using the given seed.
        /// E.g., calculates heights, assigns blocks, etc.
        /// </summary>
        /// <param name="chunkX">Chunk coordinate in X</param>
        /// <param name="chunkZ">Chunk coordinate in Z</param>
        /// <param name="seed">World seed</param>
        /// <param name="chunkSize">Number of blocks per chunk (width/length)</param>
        /// <param name="worldHeight">Max world height, e.g., 128 or 256</param>
        /// <returns>A fully populated ChunkData object</returns>
        public static ChunkData GenerateChunkData(int chunkX, int chunkZ, string seed, int chunkSize = 16, int worldHeight = 128)
        {
            var chunkData = new ChunkData
            {
                chunkX = chunkX,
                chunkZ = chunkZ,
                blocks = new List<BlockData>()
            };

            // 1) Convert seed into a stable random offset (or parse as int, etc.)
            int seedVal = seed.GetHashCode();
            Random.InitState(seedVal);

            // 2) For demonstration, use Perlin Noise to decide terrain height for each (localX, localZ)
            float noiseScale = 0.03f; // Adjust to control horizontal terrain frequency
            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    // worldPos = chunkCoord * chunkSize + localCoord
                    float worldPosX = (chunkX * chunkSize + x) * noiseScale;
                    float worldPosZ = (chunkZ * chunkSize + z) * noiseScale;

                    // use PerlinNoise [0..1], scale up to worldHeight
                    float noiseValue = Mathf.PerlinNoise(worldPosX, worldPosZ);
                    int terrainHeight = Mathf.RoundToInt(noiseValue * (worldHeight * 0.5f)) + (worldHeight / 4);

                    // 3) Fill from bottom up to terrainHeight with blocks 
                    //    (here we simply set "stone" or "dirt" as an example)
                    for (int y = 0; y <= terrainHeight; y++)
                    {
                        string blockId = (y < terrainHeight - 3) ? "stone" : "dirt";

                        // You could do a top grass layer if near surface
                        if (y == terrainHeight && y > 1) blockId = "grass_block";

                        var block = new BlockData
                        {
                            localX = x,
                            localY = y,
                            localZ = z,
                            blockId = blockId
                        };
                        chunkData.blocks.Add(block);
                    }
                }
            }

            return chunkData;
        }

        /// <summary>
        /// Example method to generate a 2D height map for a chunk, if you prefer 
        /// separate height calculation from block assignment.
        /// </summary>
        public static float[,] GenerateHeightMap(int chunkX, int chunkZ, string seed, int chunkSize = 16, float noiseScale = 0.03f)
        {
            var heightMap = new float[chunkSize, chunkSize];
            int seedVal = seed.GetHashCode();
            Random.InitState(seedVal);

            for (int x = 0; x < chunkSize; x++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    float worldX = (chunkX * chunkSize + x) * noiseScale;
                    float worldZ = (chunkZ * chunkSize + z) * noiseScale;

                    float noiseValue = Mathf.PerlinNoise(worldX, worldZ); // 0..1
                    heightMap[x, z] = noiseValue;
                }
            }
            return heightMap;
        }
    }
}
