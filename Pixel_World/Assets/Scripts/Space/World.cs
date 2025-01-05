using System.Collections;
using System.Collections.Generic;
using Space.Terrain;
using UnityEngine;

namespace Space{
    public class World : MonoBehaviour {

        public int seed;
        public BiomeAttributes biome;

        public Transform player;
        public Vector3 spawnPosition;

        public Material material;
        public List<BlockType> blocktypes = new();

        Chunk[,] chunks = new Chunk[VoxelData.WorldSizeInChunks, VoxelData.WorldSizeInChunks];

        List<ChunkCoord> activeChunks = new List<ChunkCoord>();
        public ChunkCoord playerChunkCoord;
        ChunkCoord playerLastChunkCoord;

        List<ChunkCoord> chunksToCreate = new List<ChunkCoord>();
        private bool isCreatingChunks;

        public GameObject debugScreen;
        
        private void Start(){
            AddBlockTypes();
            biome = ScriptableObject.CreateInstance<BiomeAttributes>();
            biome.biomeName = "Default";
            biome.solidGroundHeight = 42;
            biome.terrainHeight = 42;
            biome.terrainScale = 0.25f;
            biome.lodes = new Lode[2]{
                new("Dirt", 5, 1, 255, 0.1f, 0.5f, 0),
                new("Sand", 4, 30, 60, 0.2f, 0.6f, 500)
            };
            seed = 114514;

            Random.InitState(seed);

            GenerateWorld();
            
            playerLastChunkCoord = GetChunkCoordFromVector3(player.position);
        

        }

        private void Update() {
            playerChunkCoord = GetChunkCoordFromVector3(player.position);
            
            // Only update the chunks if the player has moved from the chunk they were previously on.
            if (!playerChunkCoord.Equals(playerLastChunkCoord))
                CheckViewDistance();

            if (chunksToCreate.Count > 0 && !isCreatingChunks)
                StartCoroutine("CreateChunks");
            
            if (Input.GetKeyDown(KeyCode.F3)) debugScreen.SetActive(!debugScreen.activeSelf);
        }

        private void AddBlockTypes(){
            AddBlockType(new BlockType("Air", false, 0, 0, 0, 0, 0, 0));
            AddBlockType(new BlockType("Bedrock", true, 9, 9, 9, 9, 9, 9));
            AddBlockType(new BlockType("Stone", true, 0, 0, 0, 0, 0, 0));
            AddBlockType(new BlockType("Grass", true, 2, 2, 7, 1, 2, 2));
            AddBlockType(new BlockType("Sand", true, 10, 10, 10, 10, 10, 10));
            AddBlockType(new BlockType("Dirt", true, 1, 1, 1, 1, 1, 1));
        }

        public void AddBlockType(BlockType blockType){
            blocktypes.Add(blockType);
            // Debug.Log($"Block Type '{blockType.blockName}' registered.");
        }

        void GenerateWorld () {

            for (int x = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; x < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; x++) {
                for (int z = (VoxelData.WorldSizeInChunks / 2) - VoxelData.ViewDistanceInChunks; z < (VoxelData.WorldSizeInChunks / 2) + VoxelData.ViewDistanceInChunks; z++) {
                    chunks[x, z] = new Chunk(new ChunkCoord(x, z), this, true);
                    activeChunks.Add(new ChunkCoord(x, z));
                }
            }

        }
        
        IEnumerator CreateChunks () {
            isCreatingChunks = true;

            while (chunksToCreate.Count > 0) {
                chunks[chunksToCreate[0].x, chunksToCreate[0].z].Init();
                chunksToCreate.RemoveAt(0);
                yield return null;
            }

            isCreatingChunks = false;
        }
        
        ChunkCoord GetChunkCoordFromVector3 (Vector3 pos) {
            int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
            int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
            return new ChunkCoord(x, z);
        }
        
        public Chunk GetChunkFromVector3 (Vector3 pos) {

            int x = Mathf.FloorToInt(pos.x / VoxelData.ChunkWidth);
            int z = Mathf.FloorToInt(pos.z / VoxelData.ChunkWidth);
            return chunks[x, z];

        }

        void CheckViewDistance () {
            ChunkCoord coord = GetChunkCoordFromVector3(player.position);
            playerLastChunkCoord = playerChunkCoord;

            List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);

            // Loop through all chunks currently within view distance of the player.
            for (int x = coord.x - VoxelData.ViewDistanceInChunks; x < coord.x + VoxelData.ViewDistanceInChunks; x++) {
                for (int z = coord.z - VoxelData.ViewDistanceInChunks; z < coord.z + VoxelData.ViewDistanceInChunks; z++) {

                    // If the current chunk is in the world...
                    if (IsChunkInWorld (new ChunkCoord (x, z))) {
                        // Check if it active, if not, activate it.
                        if (chunks[x, z] == null) {
                            chunks[x, z] = new Chunk(new ChunkCoord(x, z), this, false);
                            chunksToCreate.Add(new ChunkCoord(x, z));
                        }  else if (!chunks[x, z].isActive) {
                            chunks[x, z].isActive = true;
                        }
                        activeChunks.Add(new ChunkCoord(x, z));
                    }

                    // Check through previously active chunks to see if this chunk is there. If it is, remove it from the list.
                    for (int i = 0; i < previouslyActiveChunks.Count; i++){
                        if (previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
                            previouslyActiveChunks.RemoveAt(i);
                    }
                }
            }

            // Any chunks left in the previousActiveChunks list are no longer in the player's view distance, so loop through and disable them.
            foreach (ChunkCoord c in previouslyActiveChunks)
                chunks[c.x, c.z].isActive = false;
        }

        public bool CheckForVoxel(float _x, float _y, float _z){
            int xCheck = Mathf.FloorToInt(_x);
            int yCheck = Mathf.FloorToInt(_y);
            int zCheck = Mathf.FloorToInt(_z);

            // Step 1: Check if (xCheck, yCheck, zCheck) is within world bounds.
            // For example, if your entire world is VoxelData.WorldSizeInVoxels wide/high:
            if (xCheck < 0 || xCheck >= VoxelData.WorldSizeInVoxels ||
                yCheck < 0 || yCheck >= VoxelData.ChunkHeight ||
                zCheck < 0 || zCheck >= VoxelData.WorldSizeInVoxels)
            {
                // We can say "no voxel here" or treat out-of-bounds differently.
                return false;
            }

            // Step 2: Convert world coords -> chunk-local coords.
            int xChunk = xCheck / VoxelData.ChunkWidth;
            int zChunk = zCheck / VoxelData.ChunkWidth;

            // Subtract chunk offsets to get voxel indices within that chunk.
            xCheck -= xChunk * VoxelData.ChunkWidth;
            zCheck -= zChunk * VoxelData.ChunkWidth;

            // Step 3: Check if the chunk actually exists and is not null.
            if (chunks[xChunk, zChunk] == null)
                return false;  // or handle chunk creation, if desired.

            // Step 4: Finally, index the chunk’s voxelMap.
            // Make sure 'yCheck' is between 0 and chunk height.
            return blocktypes[chunks[xChunk, zChunk].voxelMap[xCheck, yCheck, zCheck]].isSolid;
        }

        public byte GetVoxel (Vector3 pos) {

            int yPos = Mathf.FloorToInt(pos.y);

            /* IMMUTABLE PASS */

            // If outside world, return air.
            if (!IsVoxelInWorld(pos))
                return 0;

            // If bottom block of chunk, return bedrock.
            if (yPos == 0)
                return 1;

            /* BASIC TERRAIN PASS */

            int terrainHeight = Mathf.FloorToInt(biome.terrainHeight * Noise.Get2DPerlin(new Vector2(pos.x, pos.z), 0, biome.terrainScale)) + biome.solidGroundHeight;
            byte voxelValue = 0;

            if (yPos == terrainHeight)
                voxelValue = 3;
            else if (yPos < terrainHeight && yPos > terrainHeight - 4)
                voxelValue = 5;
            else if (yPos > terrainHeight)
                return 0;
            else
                voxelValue = 2;

            /* SECOND PASS */

            if (voxelValue == 2) {

                foreach (Lode lode in biome.lodes) {

                    if (yPos > lode.minHeight && yPos < lode.maxHeight)
                        if (Noise.Get3DPerlin(pos, lode.noiseOffset, lode.scale, lode.threshold))
                            voxelValue = lode.blockID;
                }
            }
            return voxelValue;
        }

        public void SetVoxel(int x, int y, int z, byte blockID)
        {
            // 1) Check if the position is within world bounds
            if (!IsVoxelInWorld(new Vector3(x, y, z)))
                return;

            // 2) Determine which chunk this voxel belongs to
            int xChunk = x / VoxelData.ChunkWidth;
            int zChunk = z / VoxelData.ChunkWidth;

            // 3) Local position inside that chunk
            int xInChunk = x - (xChunk * VoxelData.ChunkWidth);
            int zInChunk = z - (zChunk * VoxelData.ChunkWidth);

            // 4) If the chunk doesn't exist (possible in infinite or procedural worlds), create it
            if (chunks[xChunk, zChunk] == null)
            {
                chunks[xChunk, zChunk] = new Chunk(new ChunkCoord(xChunk, zChunk), this, true);
            }

            // 5) Use the chunk's EditVoxel(...) to update the voxel and rebuild the mesh
            chunks[xChunk, zChunk].EditVoxel(
                new Vector3(xInChunk, y, zInChunk),
                blockID
            );
        }
        
        bool IsChunkInWorld (ChunkCoord coord) {

            if (coord.x > 0 && coord.x < VoxelData.WorldSizeInChunks - 1 && coord.z > 0 && coord.z < VoxelData.WorldSizeInChunks - 1)
                return true;
            else
                return false;
        }

        bool IsVoxelInWorld (Vector3 pos) {
            if (pos.x >= 0 && pos.x < VoxelData.WorldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.ChunkHeight && pos.z >= 0 && pos.z < VoxelData.WorldSizeInVoxels)
                return true;
            else
                return false;
        }
    }

    [System.Serializable]
    public class BlockType {

        public string blockName;
        public bool isSolid;

        [Header("Texture Values")]
        public int backFaceTexture;
        public int frontFaceTexture;
        public int topFaceTexture;
        public int bottomFaceTexture;
        public int leftFaceTexture;
        public int rightFaceTexture;

        public BlockType(string name, bool solid, int back, int front, int top, int bottom, int left, int right){
            blockName = name;
            isSolid = solid;
            backFaceTexture = back;
            frontFaceTexture = front;
            topFaceTexture = top;
            bottomFaceTexture = bottom;
            leftFaceTexture = left;
            rightFaceTexture = right;
        }

        public int GetTextureID (int faceIndex) {

            switch (faceIndex) {

                case 0:
                    return backFaceTexture;
                case 1:
                    return frontFaceTexture;
                case 2:
                    return topFaceTexture;
                case 3:
                    return bottomFaceTexture;
                case 4:
                    return leftFaceTexture;
                case 5:
                    return rightFaceTexture;
                default:
                    Debug.Log("Error in GetTextureID; invalid face index");
                    return 0;
            }
        }
    }
}