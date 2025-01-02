using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Material material;
    public List<BlockType> blocktypes = new List<BlockType>();

    void Start(){
        AddBlockTypes();
    }

    void AddBlockTypes(){
        AddBlockType(new BlockType("Stone", true, 0, 0, 0, 0, 0, 0));
        AddBlockType(new BlockType("Grass", true, 2, 2, 7, 1, 2, 2));
        // AddBlockType(new BlockType("Dirt", true, 0, 0, 0, 0, 0, 0));
        // AddBlockType(new BlockType("Wood", true, 0, 0, 0, 0, 0, 0));
        AddBlockType(new BlockType("Bedrock", true, 9, 9, 9, 9, 9, 9));

    }
    
    public void AddBlockType(BlockType blockType)
    {
        blocktypes.Add(blockType);
        Debug.Log($"Block Type '{blockType.blockName}' registered.");
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

    // Back, Front, Top, Bottom, Left, Right
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