using UnityEngine;

public class TerrainGenerator : MonoBehaviour{
    public int terrainWidth = 50; // Width of the terrain (x-axis)
    public int terrainDepth = 50; // Depth of the terrain (z-axis)
    public int terrainHeight = 10; // Maximum height (y-axis)
    public float noiseScale = 10.0f; // Scale for Perlin noise (affects terrain smoothness)

    public GameObject blockPrefab; // The prefab for each block of the terrain
    public Transform player; // Reference to the player
    public Transform terrainParent; // Parent GameObject for organizing generated blocks

    void Start(){
        GenerateTerrain();
        SetPlayerPosition();
    }

    void GenerateTerrain(){
        for (int x = 0; x < terrainWidth; x++){
            for (int z = 0; z < terrainDepth; z++){
                // Generate height using Perlin noise
                float yValue = Mathf.PerlinNoise(x / noiseScale, z / noiseScale) * terrainHeight;
                int columnHeight = Mathf.FloorToInt(yValue);

                // Generate columns of blocks up to the calculated height
                for (int y = 0; y < columnHeight; y++){
                    Vector3 blockPosition = new Vector3(x, y, z);
                    GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);


                    // Set the block's parent to be the terrainParent
                    block.transform.parent = terrainParent;
                }
            }
        }
    }

    void SetPlayerPosition(){
        // Calculate the center position of the terrain
        float centerX = terrainWidth / 2f;
        float centerZ = terrainDepth / 2f;

        // Find the highest point at the center to place the player
        float yValue = Mathf.PerlinNoise(centerX / noiseScale, centerZ / noiseScale) * terrainHeight;
        int highestPointY = Mathf.FloorToInt(yValue);

        // Set the player's position slightly above the highest point
        Vector3 playerPosition = new Vector3(centerX, highestPointY + 1f, centerZ);
        player.position = playerPosition;
    }
}