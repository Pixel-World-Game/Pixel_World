using System.IO;
using UnityEditor.UI;
using UnityEngine;

namespace AbstractClass.Element{
    public abstract class Element : MonoBehaviour{
        [System.Serializable]
        public class Attributes{
            public Vector3 size = new Vector3(1, 1, 1); // Size of the element
            public Vector3 rotation = Vector3.zero; // Rotation of the element
            public string name = "Basic_Cube"; // Name of the element

            [Header("Folder Path for Textures")] 
            public string folderPath;

            public class Textures{
                public Texture2D front;
                public Texture2D back;
                public Texture2D left;
                public Texture2D right;
                public Texture2D top;
                public Texture2D bottom;
                
                void LoadTexturesFromFolder(string folderPath){
                    if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath)){
                        Debug.LogError("Invalid folder path: " + folderPath);
                        return;
                    }

                    // Load textures from the folder
                    front  = ApplyTexture(Path.Combine(folderPath,  "front.png"));
                    back   = ApplyTexture(Path.Combine(folderPath,   "back.png"));
                    left   = ApplyTexture(Path.Combine(folderPath,   "left.png"));
                    right  = ApplyTexture(Path.Combine(folderPath,  "right.png"));
                    top    = ApplyTexture(Path.Combine(folderPath,    "top.png"));
                    bottom = ApplyTexture(Path.Combine(folderPath, "bottom.png"));
                }
                
                private Texture2D ApplyTexture(string filePath){
                    if (!File.Exists(filePath)){
                        Debug.LogWarning("Texture file not found: " + filePath);
                        return null;
                    }

                    byte[] fileData = File.ReadAllBytes(filePath);
                    Texture2D texture = new Texture2D(2, 2); // Create a placeholder texture
                    if (texture.LoadImage(fileData)){
                        return texture; // Successfully loaded the image
                    }

                    Debug.LogError("Failed to load texture: " + filePath);
                    return null;
                }
            }
            
            [Header("Textures for Each Face")]
            public Textures textures;
            public void Initialize(){ }
        }

        public Attributes attributes;

        void Start(Vector3 position){
            CreateBlockWithTextures(position);
        }





        void CreateBlockWithTextures(Vector3 position){
            // Create a cube GameObject
            GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Apply transformations
            block.transform.position = position;
            block.transform.localScale = this.attributes.size;
            block.transform.rotation = Quaternion.Euler(this.attributes.rotation);

            block.name = this.attributes.name;

            // Assign textures to each face
            AssignTexturesToCube(block);
        }

        void AssignTexturesToCube(GameObject block){
            // Create a new material
            Material material = new Material(Shader.Find("Unlit/Texture")); // Use an unlit shader to apply raw textures

            // Assign a texture array to the material using a texture-atlas approach
            Mesh mesh = block.GetComponent<MeshFilter>().mesh;
            Vector2[] uvs = new Vector2[mesh.vertices.Length];

            // Apply textures to each face of the cube
            Texture2D atlas = CreateTextureAtlas(out Vector2[] atlasUVs);
            material.mainTexture = atlas;

            // Update UV mapping
            mesh.uv = atlasUVs;

            // Apply the material to the block
            Renderer renderer = block.GetComponent<Renderer>();
            renderer.material = material;
        }

        Texture2D CreateTextureAtlas(out Vector2[] atlasUVs){
            int textureSize = 512; // Default texture size for each face
            Texture2D atlas = new Texture2D(textureSize * 3, textureSize * 2); // Create a 3x2 grid for the six faces

            // Copy each texture to the atlas
            CopyTextureToAtlas(this.attributes.textures.front, atlas, 0, 1, textureSize);
            CopyTextureToAtlas(this.attributes.textures.back, atlas, 2, 1, textureSize);
            CopyTextureToAtlas(this.attributes.textures.left, atlas, 0, 0, textureSize);
            CopyTextureToAtlas(this.attributes.textures.right, atlas, 2, 0, textureSize);
            CopyTextureToAtlas(this.attributes.textures.top, atlas, 1, 1, textureSize);
            CopyTextureToAtlas(this.attributes.textures.bottom, atlas, 1, 0, textureSize);
            
            atlas.Apply();

            // Generate UV mapping for the atlas
            atlasUVs = GenerateUVsForAtlas();

            return atlas;
        }

        void CopyTextureToAtlas(Texture2D source, Texture2D atlas, int gridX, int gridY, int textureSize){
            if (source == null) return;

            // Resize the texture if necessary
            Texture2D resized = ResizeTexture(source, textureSize, textureSize);

            // Calculate the offset in the atlas
            int xOffset = gridX * textureSize;
            int yOffset = gridY * textureSize;

            // Copy the pixels to the atlas
            Color[] pixels = resized.GetPixels();
            atlas.SetPixels(xOffset, yOffset, textureSize, textureSize, pixels);
        }

        Texture2D ResizeTexture(Texture2D source, int width, int height){
            RenderTexture rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(source, rt);

            Texture2D result = new Texture2D(width, height);
            RenderTexture.active = rt;
            result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            result.Apply();

            RenderTexture.ReleaseTemporary(rt);
            return result;
        }

        Vector2[] GenerateUVsForAtlas(){
            // Generate UVs for the cube mapped to a 3x2 atlas
            return new Vector2[]{
                // Front face
                new Vector2(0.0f, 0.5f), new Vector2(0.33f, 0.5f), new Vector2(0.0f, 1.0f), new Vector2(0.33f, 1.0f),
                // Back face
                new Vector2(0.66f, 0.5f), new Vector2(1.0f, 0.5f), new Vector2(0.66f, 1.0f), new Vector2(1.0f, 1.0f),
                // Left face
                new Vector2(0.0f, 0.0f), new Vector2(0.33f, 0.0f), new Vector2(0.0f, 0.5f), new Vector2(0.33f, 0.5f),
                // Right face
                new Vector2(0.66f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(0.66f, 0.5f), new Vector2(1.0f, 0.5f),
                // Top face
                new Vector2(0.33f, 0.5f), new Vector2(0.66f, 0.5f), new Vector2(0.33f, 1.0f), new Vector2(0.66f, 1.0f),
                // Bottom face
                new Vector2(0.33f, 0.0f), new Vector2(0.66f, 0.0f), new Vector2(0.33f, 0.5f), new Vector2(0.66f, 0.5f),
            };
        }
    }
}