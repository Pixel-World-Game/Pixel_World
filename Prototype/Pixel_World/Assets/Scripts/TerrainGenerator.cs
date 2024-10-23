using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // For AI Navigation (NavMeshBuilder)

public class TerrainGenerator : MonoBehaviour{
    public int terrainWidth = 50; // Width of the terrain (x-axis)
    public int terrainDepth = 50; // Depth of the terrain (z-axis)
    public int terrainHeight = 7; // Maximum height (y-axis)
    public float noiseScale = 5.0f; // Scale for Perlin noise (affects terrain smoothness)

    public GameObject blockPrefab; // The prefab for each block of the terrain
    public Transform player; // Reference to the player
    public Transform terrainParent; // Parent GameObject for organizing generated blocks

    private NavMeshData navMeshData;
    private NavMeshDataInstance navMeshInstance;

    void Start(){
        GenerateTerrain();
        SetPlayerPosition();
        BakeNavMesh();
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

    void BakeNavMesh(){
        if (navMeshData == null){
            navMeshData = new NavMeshData();
        }

        // Set the build settings for the nav mesh
        NavMeshBuildSettings buildSettings = NavMesh.GetSettingsByID(0);
        NavMeshBuildSource[] sources = CollectSources();

        // Define the bounds for the NavMesh calculation (covering the whole terrain)
        Bounds bounds = new Bounds(transform.position, new Vector3(terrainWidth, terrainHeight, terrainDepth));

        // Build the NavMesh using collected sources
        NavMeshBuilder.UpdateNavMeshData(navMeshData, buildSettings, new List<NavMeshBuildSource>(sources), bounds);

        // Add the NavMesh to the scene
        if (navMeshInstance.valid){
            navMeshInstance.Remove();
        }

        navMeshInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    NavMeshBuildSource[] CollectSources(){
        // Collect the sources for NavMesh building - in this case, all children blocks
        var sources = new List<NavMeshBuildSource>();
        foreach (Transform child in transform){
            var meshFilter = child.GetComponent<MeshFilter>();
            if (meshFilter != null){
                NavMeshBuildSource source = new NavMeshBuildSource{
                    shape = NavMeshBuildSourceShape.Mesh,
                    sourceObject = meshFilter.sharedMesh,
                    transform = child.localToWorldMatrix,
                    area = 0 // Use default walkable area
                };
                sources.Add(source);
            }
        }

        return sources.ToArray();
    }
}