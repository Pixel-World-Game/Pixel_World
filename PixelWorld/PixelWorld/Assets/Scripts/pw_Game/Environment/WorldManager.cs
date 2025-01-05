using UnityEngine;

namespace pw_Game.Environment
{
    /// <summary>
    /// Manages a World instance, handling chunk generation, loading, and unloading.
    /// </summary>
    public class WorldManager : MonoBehaviour
    {
        private World currentWorld;
        private ChunkManager chunkManager;

        /// <summary>
        /// Assign an existing World object to this manager.
        /// </summary>
        /// <param name="world">The World instance created in Game.cs or elsewhere.</param>
        public void Initialize(World world)
        {
            if (world == null)
            {
                Debug.LogWarning("WorldManager: Cannot initialize with a null World reference.");
                return;
            }

            currentWorld = world;
            Debug.Log($"WorldManager: Initialized with World '{currentWorld.GetWorldName()}' (Seed: {currentWorld.GetWorldSeed()}).");
        }

        /// <summary>
        /// Builds or generates the world. 
        /// For example, you might call currentWorld.LoadWorld() or do chunk generation logic here.
        /// </summary>
        public void BuildWorld()
        {
            if (currentWorld == null)
            {
                Debug.LogWarning("WorldManager: No currentWorld assigned, cannot build world.");
                return;
            }

            if (!currentWorld.IsWorldInitialized())
            {
                currentWorld.LoadWorld();
            }

            // Create a GameObject for ChunkManager
            GameObject chunkManagerObj = new GameObject("ChunkManager");
            chunkManager = chunkManagerObj.AddComponent<ChunkManager>();
            chunkManagerObj.transform.SetParent(this.transform, false);

            // If we have chunk data from currentWorld
            //   chunkManager.Initialize(currentWorld.GetWorldSeed());
            //   chunkManager.LoadChunksFromData(...);

            // Or if brand-new world:
            //   chunkManager.GenerateInitialChunks();

            Debug.Log("WorldManager: World build process complete.");
        }

        /// <summary>
        /// Optionally update the world each frame or at certain intervals.
        /// </summary>
        /// <param name="deltaTime">Time elapsed since last frame.</param>
        public void UpdateWorld(float deltaTime)
        {
            if (currentWorld == null || !currentWorld.IsWorldInitialized())
                return;

            // Forward the update call to the World
            currentWorld.UpdateWorld(deltaTime);

            // If you have additional manager logic, you can place it here as well:
            // e.g., chunk streaming, environment transitions, etc.
        }

        /// <summary>
        /// Destroys or unloads the current world.
        /// </summary>
        public void DestroyWorld()
        {
            if (currentWorld == null)
            {
                Debug.LogWarning("WorldManager: No currentWorld to destroy.");
                return;
            }

            currentWorld.UnloadWorld();
            Debug.Log("WorldManager: Unloaded the current world.");
        }
    }
}
