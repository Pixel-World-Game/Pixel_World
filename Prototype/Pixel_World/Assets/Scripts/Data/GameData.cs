using System.Collections.Generic;
using Data;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<BlockData> blocks = new List<BlockData>(); // List of all blocks in the world
    public Vector3 playerPosition;                         // Player's position
    public int[] inventoryQuantities;                      // Quantities of each block type in the inventory
}