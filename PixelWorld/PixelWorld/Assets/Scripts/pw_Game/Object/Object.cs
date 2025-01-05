using System;
using System.Collections.Generic;
using UnityEngine;

namespace pw_Game.Object
{
    [Serializable]
    public class Object
    {
        public string Name { get; private set; }
        public string UID { get; private set; }
        public string Sound { get; private set; }
        public bool IsIsotropic { get; private set; }
        public float BrightnessGamma { get; private set; } = 1.0f;

        public Dictionary<string, string> FaceTextures { get; private set; }
        public Dictionary<string, string> CarriedTextures { get; private set; }

        public Object(string name, string uid)
        {
            Name = name;
            UID = uid;
            FaceTextures = new Dictionary<string, string>();
            CarriedTextures = new Dictionary<string, string>();
        }

        public Object(string name, string uid, Dictionary<string, object> jsonData)
        {
            Name = name;
            UID = uid;
            FaceTextures = new Dictionary<string, string>();
            CarriedTextures = new Dictionary<string, string>();
            ParseJsonData(jsonData);
        }

        public virtual void Interact()
        {
            Debug.Log($"Object '{Name}' (UID: {UID}) was interacted with.");
        }

        private void ParseJsonData(Dictionary<string, object> data)
        {
            if (data.ContainsKey("sound"))
                Sound = data["sound"] as string;

            if (data.ContainsKey("isotropic"))
            {
                var iso = data["isotropic"];
                if (iso is bool b) IsIsotropic = b;
            }

            if (data.ContainsKey("brightness_gamma"))
            {
                if (float.TryParse(data["brightness_gamma"].ToString(), out float val))
                    BrightnessGamma = val;
            }

            if (data.ContainsKey("textures"))
            {
                var texVal = data["textures"];
                if (texVal is string singleTex)
                {
                    FaceTextures["all"] = singleTex;
                }
                else if (texVal is Dictionary<string, object> dict)
                {
                    foreach (var kvp in dict)
                    {
                        var faceTex = kvp.Value as string;
                        if (!string.IsNullOrEmpty(faceTex))
                            FaceTextures[kvp.Key] = faceTex;
                    }
                }
            }

            if (data.ContainsKey("carried_textures"))
            {
                var cVal = data["carried_textures"];
                if (cVal is string singleCarried)
                {
                    CarriedTextures["all"] = singleCarried;
                }
                else if (cVal is Dictionary<string, object> cDict)
                {
                    foreach (var kvp in cDict)
                    {
                        var cTex = kvp.Value as string;
                        if (!string.IsNullOrEmpty(cTex))
                            CarriedTextures[kvp.Key] = cTex;
                    }
                }
            }
        }
    }
}
