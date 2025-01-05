using UnityEngine;
using System.Collections.Generic;
using pw_Game.Object;
using pw_SaveManage;        // For GameSaveData, pwdat
using pw_Game.Environment; // For World, WorldManager

namespace pw_Game
{
    public class Game : MonoBehaviour
    {
        private World currentWorld;
        private WorldManager worldManager;

        public void LoadGameFromFile(string filePath)
        {
            // 1) Load the GameSaveData from .pwdat
            GameSaveData saveData = pwdat.LoadPwdat(filePath);
            if (saveData == null)
            {
                Debug.LogWarning("Game: Failed to load save data from file.");
                return;
            }

            // 2) Create a GameObject for the World
            GameObject worldObj = new GameObject("GameWorld");
            currentWorld = worldObj.AddComponent<World>();

            // 3) 将存档数据传入 World（名称、种子、chunks）
            currentWorld.LoadFromSaveData(saveData);

            // 4) Create a GameObject for the WorldManager
            GameObject managerObj = new GameObject("WorldManager");
            worldManager = managerObj.AddComponent<WorldManager>();

            // 5) Initialize the WorldManager with our World instance
            worldManager.Initialize(currentWorld);

            // 6) Build or generate the world
            worldManager.BuildWorld();
        }
        
        private void Start()
        {
            // 1. 创建并挂载 ObjectManager
            GameObject managerObj = new GameObject("ObjectManager");
            ObjectManager objManager = managerObj.AddComponent<ObjectManager>();

            // 2. 读取 object.json
            //    路径示例：位于 Assets/Scripts/pw_Game/Object/object.json
            string jsonPath = Application.dataPath + "/Scripts/pw_Game/Object/object.json";
            List<Object.Object> loadedObjects = objManager.LoadObjectsFromJson(jsonPath);

            // 3. 测试 - 输出每个加载到的 Object 信息
            Debug.Log($"[Game] Successfully loaded {loadedObjects.Count} objects.");
            foreach (var block in loadedObjects)
            {
                Debug.Log($"[Game] UID='{block.UID}', Name='{block.Name}', Sound='{block.Sound}', " +
                          $"IsIsotropic={block.IsIsotropic}, FaceTexturesCount={block.FaceTextures.Count}");
            }
        }
        
        private void Update()
        {
            // Let the WorldManager update the current world if initialized
            if (worldManager != null)
            {
                worldManager.UpdateWorld(Time.deltaTime);
            }
        }

        // Optional: If you want a way to unload or destroy the world
        public void UnloadWorld()
        {
            if (worldManager != null)
            {
                worldManager.DestroyWorld();
            }
        }
    }
}