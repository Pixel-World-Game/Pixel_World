using System;
using System.Collections.Generic;
using UnityEngine;

namespace pw_Game.Object
{
    /// <summary>
    /// Represents a block or item in the game world, storing name, unique ID, 
    /// six-face textures, sound, and custom interaction logic.
    /// </summary>
    [Serializable]
    public class Object
    {
        /// <summary>
        /// The display name of this block/item.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Unique identifier for this block/item, e.g., "minecraft:stone" or custom ID.
        /// </summary>
        public string UID { get; private set; }

        /// <summary>
        /// Sound type or key used for this block when placing, breaking, stepping, etc.
        /// If none specified, can be null or empty.
        /// </summary>
        public string Sound { get; private set; }

        /// <summary>
        /// Indicates whether this block uses the same texture for all faces (isotropic).
        /// Some blocks like "dirt" or "sand" can share a single texture on all sides.
        /// </summary>
        public bool IsIsotropic { get; private set; }

        /// <summary>
        /// Optional brightness factor, e.g. "brightness_gamma".
        /// If not present, can use a default like 1.0.
        /// </summary>
        public float BrightnessGamma { get; private set; } = 1.0f;

        /// <summary>
        /// Dictionary for the six faces (up, down, north, south, west, east).
        /// If the block uses a single texture, you may store the same texture under all keys,
        /// or only store one key if truly isotropic.
        /// </summary>
        public Dictionary<string, string> FaceTextures { get; private set; }

        /// <summary>
        /// Optional "carried_textures" for blocks that have a different texture 
        /// when in inventory or carried by the player. Key usage is similar to FaceTextures.
        /// </summary>
        public Dictionary<string, string> CarriedTextures { get; private set; }

        // -----------------------------------------------------------------------------------
        // Constructors
        // -----------------------------------------------------------------------------------

        /// <summary>
        /// Basic constructor to manually create an Object with just name & ID.
        /// You can then set textures, sound, etc., through methods.
        /// </summary>
        public Object(string name, string uid)
        {
            Name = name;
            UID = uid;
            FaceTextures = new Dictionary<string, string>();
            CarriedTextures = new Dictionary<string, string>();
        }

        /// <summary>
        /// Constructor that initializes this Object from a JSON-derived dictionary.
        /// The dictionary structure is assumed to follow the sample JSON blocks.
        /// </summary>
        public Object(string name, string uid, Dictionary<string, object> jsonData)
        {
            Name = name;
            UID = uid;
            FaceTextures = new Dictionary<string, string>();
            CarriedTextures = new Dictionary<string, string>();

            ParseJsonData(jsonData);
        }

        // -----------------------------------------------------------------------------------
        // Public Methods
        // -----------------------------------------------------------------------------------

        /// <summary>
        /// Virtual method for interacting with this object (besides build/destroy).
        /// Subclasses or specific blocks may override to do custom logic 
        /// (e.g., open a chest GUI, turn on a furnace, etc.).
        /// </summary>
        public virtual void Interact()
        {
            Debug.Log($"Object '{Name}' (UID: {UID}) was interacted with. Override Interact() for custom behavior.");
        }

        // -----------------------------------------------------------------------------------
        // Private Helpers
        // -----------------------------------------------------------------------------------

        /// <summary>
        /// Parse the jsonData (from the blocks.json entry) to fill textures, sound, etc.
        /// Example keys might be: "textures", "sound", "isotropic", "brightness_gamma", 
        /// or nested "textures": { "up": "...", "down": "...", ... }.
        /// </summary>
        private void ParseJsonData(Dictionary<string, object> jsonData)
        {
            // 1) Sound
            if (jsonData.ContainsKey("sound"))
            {
                Sound = jsonData["sound"] as string;
            }

            // 2) isotropic (bool or complex structure)
            // "isotropic": true OR "isotropic": { "up": false, "down": false }
            if (jsonData.ContainsKey("isotropic"))
            {
                var isoVal = jsonData["isotropic"];
                if (isoVal is bool isoBool)
                {
                    IsIsotropic = isoBool;
                }
                // If it is a dictionary specifying up/down, etc., 
                // you can parse further as needed
            }

            // 3) brightness_gamma
            if (jsonData.ContainsKey("brightness_gamma"))
            {
                // Convert to float if possible
                if (float.TryParse(jsonData["brightness_gamma"].ToString(), out float gammaVal))
                {
                    BrightnessGamma = gammaVal;
                }
            }

            // 4) textures 
            //   Could be a string (like "dirt") or a dictionary with up/down/side
            if (jsonData.ContainsKey("textures"))
            {
                var texVal = jsonData["textures"];
                if (texVal is string singleTex)
                {
                    // e.g. "dirt" -> fill all faces or just store a single reference
                    FaceTextures["all"] = singleTex; // Indicate a single texture
                }
                else if (texVal is Dictionary<string, object> texDict)
                {
                    // e.g. "up": "log_top", "down": "log_top", "side": "log_side"
                    foreach (var kvp in texDict)
                    {
                        var faceKey = kvp.Key;      // e.g. "up", "down", "side", "north", ...
                        var faceTexture = kvp.Value as string;
                        if (!string.IsNullOrEmpty(faceTexture))
                        {
                            FaceTextures[faceKey] = faceTexture;
                        }
                    }
                }
            }

            // 5) carried_textures
            if (jsonData.ContainsKey("carried_textures"))
            {
                var cTexVal = jsonData["carried_textures"];
                if (cTexVal is string singleCarried)
                {
                    // e.g. "leaves_carried"
                    CarriedTextures["all"] = singleCarried;
                }
                else if (cTexVal is Dictionary<string, object> cTexDict)
                {
                    // e.g. "up": "dispenser_top", "down": "dispenser_top", "north": ...
                    foreach (var kvp in cTexDict)
                    {
                        var faceKey = kvp.Key;
                        var faceTex = kvp.Value as string;
                        if (!string.IsNullOrEmpty(faceTex))
                        {
                            CarriedTextures[faceKey] = faceTex;
                        }
                    }
                }
            }
        }
    }
}
