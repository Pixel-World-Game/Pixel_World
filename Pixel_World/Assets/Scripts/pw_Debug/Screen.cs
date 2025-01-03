using UnityEngine;
using UnityEngine.UI;
using Space;

namespace pw_Debug{
    public class Screen : MonoBehaviour {
        World world;
        Text debugText;

        float frameRate;
        float timer;

        int halfWorldSizeInVoxels;
        int halfWorldSizeInChunks;

        void Start() {
        
            world = GameObject.Find("World").GetComponent<World>();
            debugText = GetComponent<Text>();

            halfWorldSizeInVoxels = VoxelData.WorldSizeInVoxels / 2;
            halfWorldSizeInChunks = VoxelData.WorldSizeInChunks / 2;

        }

        void Update() {

            string update_DebugText = "Pixel World Debug: ";
            update_DebugText += "\n";
            update_DebugText += frameRate + " fps";
            update_DebugText += "\n";
            update_DebugText += "Position: " + (Mathf.FloorToInt(world.player.transform.position.x) - halfWorldSizeInVoxels) + " / " + Mathf.FloorToInt(world.player.transform.position.y) + " / " + (Mathf.FloorToInt(world.player.transform.position.z) - halfWorldSizeInVoxels);
            update_DebugText += "\n";
            update_DebugText += "Chunk: " + (world.playerChunkCoord.x - halfWorldSizeInChunks) + " / " + (world.playerChunkCoord.z - halfWorldSizeInChunks);
            
            debugText.text = update_DebugText;

            if (timer > 1f) {
                frameRate = (int)(1f / Time.unscaledDeltaTime);
                timer = 0;

            } else
                timer += Time.deltaTime;

        }
    }
}
