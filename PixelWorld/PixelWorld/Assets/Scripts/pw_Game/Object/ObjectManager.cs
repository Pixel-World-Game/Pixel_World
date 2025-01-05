using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace pw_Game.Object
{
    public class ObjectManager : MonoBehaviour
    {
        private List<Object> allObjects = new List<Object>();

        /// <summary>
        /// Load object.json and parse each entry into an Object instance.
        /// </summary>
        public List<Object> LoadObjectsFromJson(string jsonFilePath)
        {
            allObjects.Clear();

            if (!File.Exists(jsonFilePath))
            {
                Debug.LogWarning($"ObjectManager: file not found -> {jsonFilePath}");
                return allObjects;
            }

            string jsonContent = File.ReadAllText(jsonFilePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Debug.LogWarning($"ObjectManager: file is empty -> {jsonFilePath}");
                return allObjects;
            }

            var rawDict = ParseObjectJson(jsonContent);
            if (rawDict == null)
            {
                Debug.LogError("ObjectManager: failed to parse object.json.");
                return allObjects;
            }

            // Build Object instances
            foreach (var kvp in rawDict)
            {
                string uid = kvp.Key;
                var blockData = kvp.Value;

                // displayName 不一定与 uid 相同，如果缺失就用 uid
                string displayName = uid;
                if (blockData.ContainsKey("displayName"))
                    displayName = blockData["displayName"].ToString();

                // Create & parse
                var obj = new Object(displayName, uid, blockData);
                allObjects.Add(obj);
            }

            Debug.Log($"ObjectManager: Loaded {allObjects.Count} objects from {jsonFilePath}.");
            return allObjects;
        }

        /// <summary>
        /// Return the cached list of all loaded Object
        /// </summary>
        public List<Object> GetAllObjects()
        {
            return allObjects;
        }

        /// <summary>
        /// A dedicated method to parse the object.json string
        /// into Dictionary<string, Dictionary<string, object>>
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> ParseObjectJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"ParseObjectJson: JSON parse error - {ex.Message}");
                return null;
            }
        }
    }
}
