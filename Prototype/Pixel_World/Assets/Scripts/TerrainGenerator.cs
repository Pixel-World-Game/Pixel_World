using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int terrainWidth = 50; // Width of the terrain (x-axis)
    public int terrainDepth = 50; // Depth of the terrain (z-axis)
    public int terrainHeight = 10; // Maximum height (y-axis)
    public float noiseScale = 10.0f; // Scale for Perlin noise (affects terrain smoothness)

    public GameObject blockPrefab; // The prefab for each block of the terrain

    void Start()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        for (int x = 0; x < terrainWidth; x++)
        {
            for (int z = 0; z < terrainDepth; z++)
            {
                // Generate height using Perlin noise
                float yValue = Mathf.PerlinNoise(x / noiseScale, z / noiseScale) * terrainHeight;
                int columnHeight = Mathf.FloorToInt(yValue);

                // Generate columns of blocks up to the calculated height
                for (int y = 0; y < columnHeight; y++)
                {
                    Vector3 blockPosition = new Vector3(x, y, z);
                    Instantiate(blockPrefab, blockPosition, Quaternion.identity);
                }
            }
        }
    }
}