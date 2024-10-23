using UnityEngine;

namespace Data{
    [System.Serializable]
    public class BlockData
    {
        public string blockType;   // Block type identifier (e.g., "Dirt", "Stone")
        public Vector3 position;   // Position of the block in the world
    }
}