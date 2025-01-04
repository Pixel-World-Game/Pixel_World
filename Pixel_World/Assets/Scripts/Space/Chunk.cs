using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space{
    public class Chunk{
        public ChunkCoord coord;

        private GameObject chunkObject;
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;

        private int vertexIndex = 0;
        private List<Vector3> vertices = new();
        private List<int> triangles = new();
        private List<Vector2> uvs = new();

        // 3D array storing the ID of each voxel in this chunk.
        public byte[,,] voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

        private World world;

        private bool _isActive;
        public bool isVoxelMapPopulated = false;

        public Chunk(ChunkCoord _coord, World _world, bool generateOnLoad){
            coord = _coord;
            world = _world;
            isActive = true;

            if (generateOnLoad)
                Init();
        }

        public void Init(){
            // Create a new GameObject for this chunk
            chunkObject = new GameObject();
            meshFilter = chunkObject.AddComponent<MeshFilter>();
            meshRenderer = chunkObject.AddComponent<MeshRenderer>();

            // Assign the material from the world
            meshRenderer.material = world.material;

            // Parent to the world object, and position it based on chunk coords
            chunkObject.transform.SetParent(world.transform);
            chunkObject.transform.position = new Vector3(
                coord.x * VoxelData.ChunkWidth,
                0f,
                coord.z * VoxelData.ChunkWidth
            );
            chunkObject.name = "Chunk " + coord.x + ", " + coord.z;

            // Fill voxelMap
            PopulateVoxelMap();
            UpdateChunk();
        }

        // Populate voxelMap by querying the world (GetVoxel) for each coordinate
        private void PopulateVoxelMap(){
            for (var y = 0; y < VoxelData.ChunkHeight; y++)
            for (var x = 0; x < VoxelData.ChunkWidth; x++)
            for (var z = 0; z < VoxelData.ChunkWidth; z++)
                voxelMap[x, y, z] = world.GetVoxel(new Vector3(x, y, z) + position);

            isVoxelMapPopulated = true;
        }

        // Rebuild the mesh for this chunk based on voxelMap
        private void UpdateChunk(){
            ClearMeshData();

            for (var y = 0; y < VoxelData.ChunkHeight; y++)
            for (var x = 0; x < VoxelData.ChunkWidth; x++)
            for (var z = 0; z < VoxelData.ChunkWidth; z++)
                // If this voxel is a solid block, build its faces.
                if (world.blocktypes[voxelMap[x, y, z]].isSolid)
                    UpdateMeshData(new Vector3(x, y, z));

            CreateMesh();
        }

        private void ClearMeshData(){
            vertexIndex = 0;
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
        }

        // Whether this chunk is currently active (enabled) in the scene
        public bool isActive{
            get => _isActive;
            set{
                _isActive = value;
                if (chunkObject != null)
                    chunkObject.SetActive(value);
            }
        }

        // Position of this chunk's GameObject in world space
        public Vector3 position => chunkObject.transform.position;

        // Determines if a local voxel coordinate is within this chunk
        private bool IsVoxelInChunk(int x, int y, int z){
            if (x < 0 || x >= VoxelData.ChunkWidth ||
                y < 0 || y >= VoxelData.ChunkHeight ||
                z < 0 || z >= VoxelData.ChunkWidth)
                return false;
            return true;
        }

        /// <summary>
        /// Edit a voxel inside this chunk at a given world-space position, updating the mesh as needed.
        /// </summary>
        public void EditVoxel(Vector3 pos, byte newID){
            // Convert from world position to local chunk voxel index
            var xCheck = Mathf.FloorToInt(pos.x);
            var yCheck = Mathf.FloorToInt(pos.y);
            var zCheck = Mathf.FloorToInt(pos.z);

            xCheck -= Mathf.FloorToInt(chunkObject.transform.position.x);
            zCheck -= Mathf.FloorToInt(chunkObject.transform.position.z);

            // Update voxelMap
            voxelMap[xCheck, yCheck, zCheck] = newID;

            // Update the mesh of neighboring voxels as well
            UpdateSurroundingVoxels(xCheck, yCheck, zCheck);

            // Rebuild this chunk's mesh
            UpdateChunk();
        }

        private void UpdateSurroundingVoxels(int x, int y, int z){
            var thisVoxel = new Vector3(x, y, z);

            for (var p = 0; p < 6; p++){
                var currentVoxel = thisVoxel + VoxelData.faceChecks[p];

                // If the neighboring voxel is outside this chunk, update the chunk in that direction
                if (!IsVoxelInChunk((int)currentVoxel.x, (int)currentVoxel.y, (int)currentVoxel.z)){
                    var adjacentChunk = world.GetChunkFromVector3(currentVoxel + position);
                    if (adjacentChunk != null)
                        adjacentChunk.UpdateChunk();
                }
            }
        }

        // Check if a voxel (in chunk-local coordinates) is solid. If it's out of this chunk's bounds, query the world.
        private bool CheckVoxel(Vector3 pos){
            var x = Mathf.FloorToInt(pos.x);
            var y = Mathf.FloorToInt(pos.y);
            var z = Mathf.FloorToInt(pos.z);

            // If out of this chunk's local range, check the world
            if (!IsVoxelInChunk(x, y, z))
                return world.blocktypes[world.GetVoxel(pos + position)].isSolid;

            // Otherwise, just check from our voxelMap
            return world.blocktypes[voxelMap[x, y, z]].isSolid;
        }

        /// <summary>
        /// Get the voxel ID at a world-space position (but from this chunk's stored data).
        /// </summary>
        public byte GetVoxelFromGlobalVector3(Vector3 pos){
            var xCheck = Mathf.FloorToInt(pos.x);
            var yCheck = Mathf.FloorToInt(pos.y);
            var zCheck = Mathf.FloorToInt(pos.z);

            xCheck -= Mathf.FloorToInt(chunkObject.transform.position.x);
            zCheck -= Mathf.FloorToInt(chunkObject.transform.position.z);

            return voxelMap[xCheck, yCheck, zCheck];
        }

        // If a face of this voxel is exposed (i.e. neighbor is air or out-of-chunk), add the face to the mesh
        private void UpdateMeshData(Vector3 pos){
            for (var p = 0; p < 6; p++)
                // If the neighbor in this face's direction is NOT solid, we draw this face.
                if (!CheckVoxel(pos + VoxelData.faceChecks[p])){
                    var blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 0]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 1]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 2]]);
                    vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[p, 3]]);

                    AddTexture(world.blocktypes[blockID].GetTextureID(p));

                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 3);
                    vertexIndex += 4;
                }
        }

        // Finalize the mesh and assign it to the MeshFilter
        private void CreateMesh(){
            var mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uvs.ToArray();

            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
        }

        // Assign the correct UVs for the texture ID
        private void AddTexture(int textureID){
            float y = textureID / VoxelData.TextureAtlasSizeInBlocks;
            var x = textureID - y * VoxelData.TextureAtlasSizeInBlocks;

            x *= VoxelData.NormalizedBlockTextureSize;
            y *= VoxelData.NormalizedBlockTextureSize;

            // Flip Y to match your texture packing
            y = 1f - y - VoxelData.NormalizedBlockTextureSize;

            uvs.Add(new Vector2(x, y));
            uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
            uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
            uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
        }
    }

    public class ChunkCoord{
        public int x;
        public int z;

        public ChunkCoord(){
            x = 0;
            z = 0;
        }

        public ChunkCoord(int _x, int _z){
            x = _x;
            z = _z;
        }

        public ChunkCoord(Vector3 pos){
            var xCheck = Mathf.FloorToInt(pos.x);
            var zCheck = Mathf.FloorToInt(pos.z);

            x = xCheck / VoxelData.ChunkWidth;
            z = zCheck / VoxelData.ChunkWidth;
        }

        public bool Equals(ChunkCoord other){
            if (other == null)
                return false;
            return other.x == x && other.z == z;
        }
    }
}