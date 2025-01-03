using UnityEngine;

namespace Space.Terrain{
    [CreateAssetMenu(fileName = "BiomeAttributes", menuName = "MinecraftTutorial/Biome Attribute")]
    public class BiomeAttributes : ScriptableObject {

        public string biomeName;
        public int solidGroundHeight;
        public int terrainHeight;
        public float terrainScale;
        public Lode[] lodes;

    }

    [System.Serializable]
    public class Lode {

        public string nodeName;
        public byte blockID;
        public int minHeight;
        public int maxHeight;
        public float scale;
        public float threshold;
        public float noiseOffset;

        public Lode(string _nodeName, byte _blockID, int _minHeight, int _maxHeight, float _scale, float _threshold, float _noiseOffset){
            nodeName = _nodeName;
            blockID = _blockID;
            minHeight = _minHeight;
            maxHeight = _maxHeight;
            scale = _scale;
            threshold = _threshold;
            noiseOffset = _noiseOffset;
        }

    }
}